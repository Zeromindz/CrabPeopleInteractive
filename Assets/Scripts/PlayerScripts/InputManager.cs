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
    public LeaderboardUI leaderboardUI;

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

        playerInput.Player.EnableBoost.started += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableBoost.performed += ctx => OnShiftPressed(ctx);
        playerInput.Player.EnableBoost.canceled += ctx => OnShiftPressed(ctx);

        playerInput.Player.EnableTrick.started += ctx => OnSpacePressed(ctx);
        playerInput.Player.EnableTrick.performed += ctx => OnSpacePressed(ctx);
        playerInput.Player.EnableTrick.canceled += ctx => OnSpacePressed(ctx);

        playerInput.Player.RecordReplay.started += ctx => OnRecordReplay(ctx);
        // playerInput.Player.RecordReplay.performed += ctx => OnSpacePressed(ctx);
        //playerInput.Player.RecordReplay.canceled += ctx => OnSpacePressed(ctx);

        //playerInput.Player.Leaderboard.started += ctx => LeaderboardNav(ctx);
        //playerInput.Player.Leaderboard.performed += ctx => LeaderboardNav(ctx);
        //playerInput.Player.Leaderboard.canceled += ctx => LeaderboardNav(ctx);

        playerInput.Player.Temp.started += ctx => OnSpawnPortal(ctx);
    }

    private void OnEscapePressed(InputAction.CallbackContext ctx)
    {
		//Debug.Log("Button Pressed: Escape");
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
        //Debug.Log("Shift pressed!" + shiftPressed);
	}

    private void OnSpacePressed(InputAction.CallbackContext ctx)
    {
        spacePressed = ctx.ReadValue<float>();
        //Debug.Log("Space pressed!" + spacePressed);
    }

    private void OnMovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        //Debug.Log($"Movement Input {movementInput} ");
    }

    private void OnRecordReplay(InputAction.CallbackContext ctx)
    {
        recordGhost = ctx.ReadValue<float>();
        //Debug.Log(recordGhost);
        GhostPlayer.Instance.LoadGhost();
        GhostPlayer.Instance.Play();
    }

    private void OnSpawnPortal(InputAction.CallbackContext ctx)
    {
        pPressed = ctx.ReadValue<float>();
        //Debug.Log($"P Pressed {pPressed} ");
    }

    private void LeaderboardNav(InputAction.CallbackContext ctx)
	{
        float i = ctx.ReadValue<float>();
        //Debug.Log(i);
        leaderboardUI.WrapElements(i);
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

