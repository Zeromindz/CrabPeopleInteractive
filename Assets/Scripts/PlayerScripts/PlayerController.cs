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
    internal VFXController VFXController;
    internal PassengerManager passengerManager;

    
    private PlayerState currentState;

    private Stack<GameObject> m_checkPoints;


    private int m_Passengers = 0;
    public int GetPassengers() { return m_Passengers; }
    public void ClearPassengers() { m_Passengers = 0; }

    private static PlayerController m_Instance;                       // The current instance of MenuController
    public static PlayerController Instance                           // The public current instance of MenuController
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;
        passengerManager = GetComponent<PassengerManager>();
    }

    void Start()
    {
        Initialization();
    }

    private void Update()
    {
        
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
        playerMovement.IncreaseMaxSpeed();

        passengerManager.SpawnPassenger();
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
        VFXController = GetComponent<VFXController>();
        currentState = PlayerState.Idle;
    }
}
