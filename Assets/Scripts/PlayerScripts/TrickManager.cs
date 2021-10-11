using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class TrickManager : MonoBehaviour
{
    internal PlayerController controller;

    private bool inAir = false;
    float angle;
    int fullRotations;
    public  float currentRotation;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        inAir = controller.playerMovement.m_Grounded;

        Vector3 dir = transform.forward;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        if (inAir)
        {
            TrackSpin();
        }
        else
        {
            currentRotation = 0f;
        }
    }

    public void TrackSpin()
    {
        currentRotation += controller.transform.localRotation.x;
    }
}
