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
    private UIController m_UIController = null;

    private Vector2 movementInput;
    public Vector2 GetMovementInput() { return movementInput; }

    private float shiftPressed;
    public float ShiftPressed() { return shiftPressed; }

    private float spacePressed;
    public float SpacePressed() { return spacePressed; }

    private float acceleration;
    public float GetAcceleration() { return acceleration; }

    private float recordGhost;
    public float RecordGhost() { return recordGhost; }

    private float pPressed;
    public float PPressed() { return pPressed; }

    private float m_DeadZone = 0.1f;

	private void Start()
	{
        m_UIController = UIController.Instance;
	}

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        //Debug.Log("HI");
        playerInput = new PlayerInputActions();
        // playerInput.Player.Movement.started += ctx => OnMovementInput(ctx);
        playerInput.Player.Movement.performed += ctx => OnMovementInput(ctx);
        playerInput.Player.Movement.canceled += ctx => OnMovementInput(ctx);

        //playerInput.Player.PausePreviousUI.started += ctx => OnEscapePressed(ctx);
        playerInput.Player.PausePreviousUI.performed += ctx => OnEscapePressed(ctx);
        //playerInput.Player.PausePreviousUI.canceled += ctx => OnEscapePressed(ctx);

        //playerInput.Player.EnableBoost.started += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableBoost.performed += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableBoost.canceled += ctx => OnShiftPressed(ctx);

        //playerInput.Player.EnableTrick.started += ctx => OnSpacePressed(ctx);
        playerInput.Player.EnableTrick.performed += ctx => OnSpacePressed(ctx);
        playerInput.Player.EnableTrick.canceled += ctx => OnSpacePressed(ctx);

    }

    private void OnEscapePressed(InputAction.CallbackContext ctx)
    {
		Debug.Log("Button Pressed: Escape" + ctx.started);

		// Pauses the game
		if (m_UIController.MenuController.IsInGame)
		{
			m_UIController.MenuController.PauseGame();
		}

		// Returns to the previous UI
		else
		{
			m_UIController.MenuController.ReturnToPreviousUI();
		}
	}

    private void OnShiftPressed(InputAction.CallbackContext ctx)
	{
        shiftPressed = ctx.ReadValue<float>();
        //Debug.Log("Shift pressed!" + shiftPressed);
        PlayerMovement.Instance.ShiftPressed(shiftPressed);
	}

    private void OnSpacePressed(InputAction.CallbackContext ctx)
    {
        spacePressed = ctx.ReadValue<float>();
        //Debug.Log("Space pressed!" + spacePressed);
        PlayerMovement.Instance.SpacePressed(spacePressed);
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        //Debug.Log($"Movement Input {movementInput} ");
        PlayerMovement.Instance.Movement(movementInput);
    }

    private void OnEnable()
    {
        if(playerInput != null)
        {
            playerInput.Enable();
        }
    }

    private void OnDisable()
    {
        if(playerInput != null)
        {
            playerInput.Disable();
        }
    }
}

