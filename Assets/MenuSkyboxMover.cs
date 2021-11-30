using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSkyboxMover : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    [SerializeField] private GameObject m_Skybox;
    [SerializeField] private float m_Height;
    private PortalManager m_PortalManager;
    private bool m_SkyboxHidden = false;


    private void Start()
    {
        m_PortalManager = PortalManager.m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = m_Player.position;
        pos.y += m_Height;
        m_Skybox.transform.position = pos;
        if (m_PortalManager.GetState() != 2)
        {
            if(m_SkyboxHidden)
            {
                m_Skybox.SetActive(true);
                m_SkyboxHidden = false;

            }

        }
        else
        {
            if(!m_SkyboxHidden)
            {
                m_Skybox.SetActive(false);
                m_SkyboxHidden = true;
            }
        }
    }
}
