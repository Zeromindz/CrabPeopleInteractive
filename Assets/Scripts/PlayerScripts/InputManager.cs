using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//===================================================
// Checks for button input
//________________________________________________/

public class InputManager : MonoBehaviour
{
    internal PlayerController playerController;
    private PlayerInputActions playerInput;

    private Vector2 movementInput;
    public Vector2 GetMovementInput() { return movementInput; }

    private float acceleration;
    public float GetAcceleration() { return acceleration; }

    private float shiftPressed;
    public float ShiftPressed() { return shiftPressed; }

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

        playerInput.Player.EnableTrick.started += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableTrick.performed += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableTrick.canceled += ctx => OnShiftPressed(ctx);
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

    private void OnShiftPressed(InputAction.CallbackContext ctx)
	{
        shiftPressed = ctx.ReadValue<float>();
        Debug.Log("Shift pressed!" + shiftPressed);
      //  playerController.playerMovement.
	}

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        Debug.Log($"Movement Input {movementInput} ");
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

