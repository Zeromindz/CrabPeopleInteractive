using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    public static WaveManager m_Instance;
    public int m_Dimension = 10;
    public Octave[] m_Octaves;
    protected MeshFilter m_MeshFilter;
    protected Mesh m_Mesh;

    public float m_UVScale;


    [Header("Wave Settings")]
    public float m_Amplitude = 1.0f;
    public float m_Length = 2.0f;
    public float m_Speed = 1.0f;
    public float m_Offset = 0f;

    private void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = this;
        }
        else if(m_Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Mesh = new Mesh();
        m_Mesh.name = gameObject.name;
        m_Mesh.vertices = GenerateVerts();
        m_Mesh.triangles = GenerateTris();
        m_Mesh.uv = GenerateUVs();
        m_Mesh.RecalculateBounds();
        m_Mesh.RecalculateNormals();

        m_MeshFilter = gameObject.AddComponent<MeshFilter>();
        m_MeshFilter.mesh = m_Mesh;
    }

    

    private void Update()
    {
        //m_Offset += Time.deltaTime * m_Speed; 

        var verts = m_Mesh.vertices;
        for (int x = 0; x <= m_Dimension; x++)
        {
            for (int z = 0; z <= m_Dimension; z++)
            {
                var y = 0f;
                for(int o = 0; o < m_Octaves.Length; o++)
                {
                    if(m_Octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * m_Octaves[o].scale.x) / m_Dimension, (z * m_Octaves[o].scale.y) / m_Dimension) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + m_Octaves[o].speed.magnitude * Time.time) * m_Octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * m_Octaves[o].scale.x + Time.time * m_Octaves[o].speed.x / m_Dimension), (z * m_Octaves[o].scale.y + Time.time * m_Octaves[o].speed.y) / m_Dimension) -0.5f;
                        y += perl * m_Octaves[o].height;
                    }
                }
                verts[Index(x, z)] = new Vector3(x, y, z);
            }
        }

        m_Mesh.vertices = verts;
        m_Mesh.RecalculateNormals();
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(m_Dimension + 1) * (m_Dimension + 1)];
        for (int x = 0; x <= m_Dimension; x++)
        {
            for (int z = 0; z <= m_Dimension; z++)
            {
                verts[Index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;

    }

    private int[] GenerateTris()
    {
        var tris = new int[m_Mesh.vertices.Length * 6];

        for(int x = 0; x < m_Dimension; x++)
        {
            for(int z = 0; z < m_Dimension; z++)
            {
                tris[Index(x, z) * 6 + 0] = Index(x, z);
                tris[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);
                tris[Index(x, z) * 6 + 2] = Index(x + 1, z);
                tris[Index(x, z) * 6 + 3] = Index(x, z);
                tris[Index(x, z) * 6 + 4] = Index(x, z + 1);
                tris[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);
            }
        }

        return tris;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[m_Mesh.vertices.Length];

        for (int x = 0; x <= m_Dimension; x++)
        {
            for (int z = 0; z <= m_Dimension; z++)
            {
                var vec = new Vector2((x / m_UVScale) % 2, (z / m_UVScale) % 2);
                uvs[Index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }
        return uvs;
    }

    private int Index(int _x, int _z)
    {
          return _x * (m_Dimension + 1) + _z;
    }

    private int Index(float _x, float _z)
    {
        return Index((int)_x, (int)_z);
    }

    //public float GetWaveHeight(float _x)
    //{
    //    return m_Amplitude * Mathf.Sin(_x / m_Length + m_Offset);
    //
    //
    //}
    public float GetWaveHeight(Vector3 _position)
    {
        // Scale factor and positition in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((_position - transform.position), scale);

        // Get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        // clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, m_Dimension);
        p1.z = Mathf.Clamp(p1.z, 0, m_Dimension);
        p2.x = Mathf.Clamp(p2.x, 0, m_Dimension);
        p2.z = Mathf.Clamp(p2.z, 0, m_Dimension);
        p3.x = Mathf.Clamp(p3.x, 0, m_Dimension);
        p3.z = Mathf.Clamp(p3.z, 0, m_Dimension);
        p4.x = Mathf.Clamp(p4.x, 0, m_Dimension);
        p4.z = Mathf.Clamp(p4.z, 0, m_Dimension);


        // Get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        // weighted sum
        var height = m_Mesh.vertices[Index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + m_Mesh.vertices[Index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + m_Mesh.vertices[Index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + m_Mesh.vertices[Index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));


        // scale
        return height * transform.lossyScale.y / dist;
    }


}
