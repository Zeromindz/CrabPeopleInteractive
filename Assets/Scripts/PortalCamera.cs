using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform m_PlayerCam;
    public Transform m_Entrance;
    public Transform m_Exit;

    public bool m_Active;

    public void SetOffset(Transform _entrance)
    {
        m_Entrance = _entrance;
        m_Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Active)
        {
            Vector3 playerOffsetFromPortal = m_PlayerCam.position - m_Entrance.position;
            transform.position = m_Exit.position + playerOffsetFromPortal;

            float angularDiffBetweenPortalRotations = Quaternion.Angle(m_Exit.rotation, m_Entrance.rotation);
            
            Quaternion rotationalDiff = Quaternion.AngleAxis(angularDiffBetweenPortalRotations, Vector3.up);
            Vector3 newDirection = rotationalDiff * m_PlayerCam.forward;
            transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);

        }
    }
}
