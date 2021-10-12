using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================================
// Checks for button input
//________________________________________________/

public class PlayerInput : MonoBehaviour
{
    internal PlayerController playerController;

    private float wasdVertical;
    private float wasdHorizontal;
    private float arrowsVertical;
    private float arrowsHorizontal;
    private bool shiftPressed;

    public float GetWASDVertical() { return wasdVertical; }
    public float GetWASDHorizontal() { return wasdHorizontal; }
    public float GetArrowsVertical() { return arrowsVertical; }
    public float GetArrowsHorizontal() { return arrowsHorizontal; }
    public bool ShiftPressed() { return shiftPressed;  }
    private float m_DeadZone = 0.1f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();    
    }

    void Update()
    {
        

        wasdVertical = 0;
        float y = Input.GetAxis("WASDVertical");
        if (y > m_DeadZone)
        {
            wasdVertical = y;
        }
        else if (y < -m_DeadZone)
        {
            // steer
            wasdVertical = y;
        }

        wasdHorizontal = 0;
        float x = Input.GetAxis("WASDHorizontal");
        if (x > m_DeadZone)
        {
            // steer
            wasdHorizontal = x;
        }
        else if (x < -m_DeadZone)
        {
            // steer
            wasdHorizontal = x;
        }

        arrowsVertical = 0;
        float u = Input.GetAxis("ArrowsVertical");
        if (u > m_DeadZone)
        {
            arrowsVertical = u;
        }
        else if (u < -m_DeadZone)
        {
            // steer
            arrowsVertical = u;
        }

        arrowsHorizontal = 0;
        float v = Input.GetAxis("ArrowsHorizontal");
        if (v > m_DeadZone)
        {
            // steer
            arrowsHorizontal = v;
        }
        else if (v < -m_DeadZone)
        {
            // steer
            arrowsHorizontal = v;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftPressed = true;
        }
        else
        {
            shiftPressed = false;
        }
    }
}
