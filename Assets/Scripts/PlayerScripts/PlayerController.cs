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
[RequireComponent(typeof(TrickManager))]


public class PlayerController : MonoBehaviour
{
    internal PlayerInput playerInput;
    internal PlayerMovement playerMovement;
    internal PlayerCollision playerCollision;
    internal TrickManager trickManager;

    private PlayerState currentState;

    private int m_Passengers = 0;
    public int GetPassengers() { return m_Passengers; }
    public void ClearPassengers() { m_Passengers = 0; }

    void Start()
    {
        Initialization();
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
    public void AddPassenger() 
    { 
        m_Passengers++; 
        playerMovement.m_MaxSpeed *= 1.1f;
        playerMovement.m_HorsePower *= 1.01f;
    }

    private void ResetBoat()
    {
        Vector3 spawnPoint = transform.position + (Vector3.up * 10.0f);
        transform.position = spawnPoint;
        transform.SetPositionAndRotation(spawnPoint, Quaternion.Euler(0f, 0f, 0f));
    }

    void Initialization()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollision>();
        trickManager = GetComponent<TrickManager>();

        currentState = PlayerState.Idle;
    }
}
