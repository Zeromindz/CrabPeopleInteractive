using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

    private PortalManager m_PortalManager;

    private void Start()
    {
        m_PortalManager = PortalManager.m_Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_PortalManager.m_PlayerOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PortalManager.m_PlayerOverlapping = false;
        }
    }
}
