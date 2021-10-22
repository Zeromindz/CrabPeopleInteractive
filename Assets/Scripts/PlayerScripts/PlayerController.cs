using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerState
{
    Idle,
    Moving
}


public class PlayerController : MonoBehaviour
{
    //[SerializeField] PlayerInput playerInput;
    internal PlayerMovement playerMovement;
    internal PlayerCollision playerCollision;
    internal InputManager playerInput;
    internal TrickManager trickManager;

    [Header("GFX")]
    [SerializeField] private ParticleSystem[] m_RocketTrails;
    [SerializeField] private ParticleSystem m_GroundedTrail;
    private PlayerState currentState;

    private int m_Passengers = 0;
    public int GetPassengers() { return m_Passengers; }
    public void ClearPassengers() { m_Passengers = 0; }


	void Start()
    {
        Initialization();
        
    }

    private void Update()
    {
        // Set vfx emissions
        int rocketEmissionRate = 0;
        if (playerMovement.m_Boosting)
        {
            rocketEmissionRate = 10;

        }
        foreach (var trails in m_RocketTrails)
        {
            var rocketEmission = trails.emission;
        //    rocketEmission.rateOverTime = new ParticleSystem.MinMaxCurve(rocketEmissionRate);
        }

        int groundEmissionRate = 0;
        if (playerMovement.m_Grounded)
        {
            groundEmissionRate = 10;

        }

        var groundEmission = m_GroundedTrail.emission;
       // groundEmission.rateOverTime = new ParticleSystem.MinMaxCurve(groundEmissionRate);

        
    }

    void ChangeState(PlayerState newState)
    {
        if(newState != currentState)
        {
            currentState = newState;
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
        playerInput = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollision>();
        trickManager = GetComponent<TrickManager>();

        currentState = PlayerState.Idle;
    }
}
