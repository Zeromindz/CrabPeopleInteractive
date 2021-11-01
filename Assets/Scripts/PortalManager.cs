using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private InputManager playerInput;

    private GameObject m_Player;
    public GameObject m_Portal;

    public Camera m_PortalCam;
    public Material m_PortalMat;

    public float m_SpawnDist = 30.0f;

    int layerMask;

    public bool m_PortalSpawned = false;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = m_Player.gameObject.GetComponent<InputManager>();
        layerMask = 1 << 10;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.PPressed() > 0)
        {
            if(!m_PortalSpawned)
            {
                SpawnPortal();
            }
        }
    }

    void SpawnPortal()
    {
        Vector3 moveDir = m_Player.GetComponent<PlayerController>().playerMovement.GetCurrentVel().normalized;

        Vector3 spawnPos = m_Player.transform.position + (moveDir * m_SpawnDist);
        RaycastHit hit;
        if(Physics.Raycast(spawnPos, -Vector3.up, out hit, layerMask))
        {
            Debug.DrawRay(spawnPos, -Vector3.up);
            
        }

        GameObject portal = Instantiate(m_Portal, spawnPos, Quaternion.LookRotation(-moveDir, Vector3.up));


        m_PortalCam.gameObject.GetComponent<PortalCamera>().SetOffset(portal.transform);

        // Set up portal texture
        if (m_PortalCam.targetTexture != null)
        {
            m_PortalCam.targetTexture.Release();
        }

        m_PortalCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_PortalMat.mainTexture = m_PortalCam.targetTexture;

        m_PortalSpawned = true;
    }
}
