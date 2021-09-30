using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Idle,
    Moving
}

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCollision))]


public class PlayerController : MonoBehaviour
{
    internal PlayerInput playerInput;
    internal PlayerMovement playerMovement;
    internal PlayerCollision playerCollision;

    private PlayerState currentState;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>(); ;
        playerCollision = GetComponent<PlayerCollision>(); ;

        currentState = PlayerState.Idle;
    }

    void ChangeState(PlayerState newState)
    {
        if(newState != currentState)
        {
            currentState = newState;
        }
    }

    void Update()
    {
        // Testing
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetBoat();
        }
    }

    private void ResetBoat()
    {
        Vector3 spawnPoint = transform.position + (Vector3.up * 10.0f);
        transform.position = spawnPoint;
        transform.SetPositionAndRotation(spawnPoint, Quaternion.Euler(0f, 0f, 0f));
    }
}
