using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    private float timeValue;
    private int index1;
    private int index2;
    private bool isReplaying;
    public List<GhostData> path = new List<GhostData>();
    public int currentMove = 0;

    private static GhostPlayer m_Instance;               // Current Private instance
    public static GhostPlayer Instance                   // Current public instance
    {
        get { return m_Instance; }
    }

    private void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;
        timeValue = 0;
    }

    private void Update()
    {
        if (isReplaying)
        {
            timeValue += Time.unscaledDeltaTime;
            GetIndex();
            SetTransform();
        }
    }

    public void LoadGhost()
    {
        path = GhostSave.Instance.LoadGhost();
        
    }

    private void GetIndex()
    {
        for (int i = 0; i < path.Count - 2; i++)
        {

            if (path[i].timeStamp == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }

            else if (path[i].timeStamp < timeValue & timeValue < path[i + 1].timeStamp)
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = path.Count - 1;
        index2 = path.Count - 1;
    }

    private void SetTransform()
    {
        Vector3 pos1, pos2;
        Quaternion rot1, rot2;
        pos1.x = path[index1].posX;
        pos1.y = path[index1].posY;
        pos1.z = path[index1].posZ;
        pos2.x = path[index2].posX;
        pos2.y = path[index2].posY;
        pos2.z = path[index2].posZ;

        rot1.x = path[index1].rotX;
        rot1.y = path[index1].rotY;
        rot1.z = path[index1].rotZ;
        rot1.w = path[index1].rotW;
        rot2.x = path[index2].rotX;
        rot2.y = path[index2].rotY;
        rot2.z = path[index2].rotZ;
        rot2.w = path[index2].rotW;

        if (index1 == index2)
        {
            gameObject.transform.position = pos1;
            gameObject.transform.rotation = rot1;
        }

		else
		{
            float interpolationFactor = (timeValue - path[index1].timeStamp) / (path[index2].timeStamp - path[index1].timeStamp);   
            gameObject.transform.position = Vector3.Lerp(pos1, pos2, interpolationFactor);
            gameObject.transform.rotation = Quaternion.Lerp(rot1, rot2, interpolationFactor);
		}
    }

    public void Play()
    {
        isReplaying = true;
    }

    public void Stop()
    {
        isReplaying = false;
    }
}
