using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

    private PortalManager m_PortalManager;

    //private Transform m_Player;
    //private Transform m_Exit;

    //private bool m_PlayerOverlapping = false;

    private void Start()
    {
        //m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        //m_Exit = GameObject.FindGameObjectWithTag("Exit").transform;

        m_PortalManager = PortalManager.m_Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_PlayerOverlapping)
        //{
        //    Vector3 portalToPlayer = m_Player.position - transform.position;
        //    float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
        //
        //    if(dotProduct < 0f)
        //    {
        //        float rotDifference = -Quaternion.Angle(transform.rotation, m_Exit.rotation);
        //        //rotDifference += 180;
        //        m_Player.Rotate(Vector3.up, rotDifference);
        //
        //        Vector3 posOffset = Quaternion.Euler(0f, rotDifference, 0f) * portalToPlayer;
        //        m_Player.position = m_Exit.position + posOffset;
        //
        //        m_PlayerOverlapping = false;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.parent.tag == "Player")
        {
            m_PortalManager.m_PlayerOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.parent.tag == "Player")
        {
            m_PortalManager.m_PlayerOverlapping = false;
        }
    }
}
