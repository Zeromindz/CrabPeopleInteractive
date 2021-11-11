using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCam : MonoBehaviour
{

    [SerializeField] private Transform m_Playercam = null;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerRot = m_Playercam.rotation.eulerAngles;

        Quaternion newRot = Quaternion.Euler(0.0f, playerRot.y, 0.0f);
        this.transform.rotation = newRot;
    }
}
