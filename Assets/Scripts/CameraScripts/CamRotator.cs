using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{
    public GameObject boat;

    public float m_TargetRotX;
    public float m_TargetRotY;
    public float m_TargetRotZ;

    void FixedUpdate()
    {
        // Store the boats rotation angles
        m_TargetRotX = boat.transform.eulerAngles.x;
        m_TargetRotY = boat.transform.eulerAngles.y;
        m_TargetRotZ = boat.transform.eulerAngles.z;

        // Set rotation to the inverse of the x and z components
        transform.rotation = Quaternion.Euler(m_TargetRotX - m_TargetRotX, m_TargetRotY, m_TargetRotZ - m_TargetRotZ);
    }
}
