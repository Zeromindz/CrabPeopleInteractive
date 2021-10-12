using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]

//===================================================
// Checks for button input
//________________________________________________/

public class PlayerInput : MonoBehaviour
{
    internal PlayerController playerController;
    private PlayerInputActions playerInput;

    private Vector2 movementInput;

    private float wasdVertical;
    private float wasdHorizontal;
    private float arrowsVertical;
    private float arrowsHorizontal;
    private bool shiftPressed;

    private float acceleration; 

    public float GetAcceleration() { return acceleration; }

    public float GetWASDVertical() { return wasdVertical; }
    public float GetWASDHorizontal() { return wasdHorizontal; }
    public float GetArrowsVertical() { return arrowsVertical; }
    public float GetArrowsHorizontal() { return arrowsHorizontal; }
    public bool ShiftPressed() { return shiftPressed;  }
    private float m_DeadZone = 0.1f;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        playerInput = new PlayerInputActions();
        playerInput.Player.Movement.started += ctx => OnMovementInput(ctx);
        playerInput.Player.Movement.performed += ctx => OnMovementInput(ctx);
        playerInput.Player.Movement.canceled += ctx => OnMovementInput(ctx);

        playerInput.Player.PausePreviousUI.started += ctx => OnEscapePressed(ctx); 
        playerInput.Player.PausePreviousUI.performed += ctx => OnEscapePressed(ctx);
        playerInput.Player.PausePreviousUI.canceled += ctx => OnEscapePressed(ctx);
    }

    private void OnEscapePressed(InputAction.CallbackContext ctx)
	{
        Debug.Log("Button Pressed: Escape");
        // Pauses the game
        if (MenuController.Instance.IsInGame)
        {
           MenuController.Instance.PauseGame();
        }

        // Returns to the previous UI
        else
        {
            MenuController.Instance.ReturnToPreviousUI();
        }
        
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
	{
        movementInput = ctx.ReadValue<Vector2>();
        Debug.Log($"Movement Input {movementInput} " );

	}

    void Update()
    {

        //Debug.Log("Acceleration: " + acceleration);
        //wasdVertical = 0;
        //float y = Input.GetAxis("WASDVertical");
        //if (y > m_DeadZone)
        //{
        //    wasdVertical = y;
        //}
        //else if (y < -m_DeadZone)
        //{
        //    // steer
        //    wasdVertical = y;
        //}

        //wasdHorizontal = 0;
        //float x = Input.GetAxis("WASDHorizontal");
        //if (x > m_DeadZone)
        //{
        //    // steer
        //    wasdHorizontal = x;
        //}
        //else if (x < -m_DeadZone)
        //{
        //    // steer
        //    wasdHorizontal = x;
        //}

        //arrowsVertical = 0;
        //float u = Input.GetAxis("ArrowsVertical");
        //if (u > m_DeadZone)
        //{
        //    arrowsVertical = u;
        //}
        //else if (u < -m_DeadZone)
        //{
        //    // steer
        //    arrowsVertical = u;
        //}

        //arrowsHorizontal = 0;
        //float v = Input.GetAxis("ArrowsHorizontal");
        //if (v > m_DeadZone)
        //{
        //    // steer
        //    arrowsHorizontal = v;
        //}
        //else if (v < -m_DeadZone)
        //{
        //    // steer
        //    arrowsHorizontal = v;
        //}

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    shiftPressed = true;
        //}
        //else
        //{
        //    shiftPressed = false;
        //}
    }

    private void OnEnable()
	{
        playerInput.Enable();
	}

    private void OnDisable()
	{
        playerInput.Disable();
	}
}
