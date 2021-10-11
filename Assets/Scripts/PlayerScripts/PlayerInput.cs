using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

//===================================================
// Checks for button input
//________________________________________________/

public class PlayerInput : MonoBehaviour
{
    internal PlayerController playerController;

    private float verticalInput;
    private float horizontalInput;
    private float verticalArrows;
    private float horizontalArrows;
    private bool shiftPressed;

    public float GetVertical() { return verticalInput; }
    public float GetHorizontal() { return horizontalInput; }
    public bool ShiftPressed() { return shiftPressed;  }
    private float m_DeadZone = 0.1f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();    
    }

    void Update()
    {
        verticalInput = 0;
        float y = Input.GetAxis("Vertical");
        if (y > m_DeadZone)
        {
            verticalInput = y;
        }
        else if (y < -m_DeadZone)
        {
            // steer
            verticalInput = y;
        }
        horizontalInput = 0;
        float x = Input.GetAxis("Horizontal");
        if (x > m_DeadZone)
        {
            // steer
            horizontalInput = x;
        }
        else if (x < -m_DeadZone)
        {
            // steer
            horizontalInput = x;
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
