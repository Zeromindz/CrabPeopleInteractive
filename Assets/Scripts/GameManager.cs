using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Canvas;
    
    [Header("Player Stats")]
    private PlayerController m_Player;
    public float m_BoatSpeed;
    public float m_CurrentTrickRotation;
    public int m_Passengers;

    public float m_TimeLimit = 50.0f;
    private float m_CurrentTime = 0f;
    public float GetCurrentTime() { return m_CurrentTime; }

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_CurrentTime = m_TimeLimit; 
    }

    
    void Update()
    {
        // Time limit
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    m_CurrentTime = m_TimeLimit;
        //}

        if(m_CurrentTime > 0)
        {
            m_CurrentTime -= Time.deltaTime;
        }
        else 
        {
            Debug.Log("Times up");
        }

        m_BoatSpeed = m_Player.playerMovement.GetSpeed();
        m_CurrentTrickRotation = m_Player.trickManager.currentRotation;
        m_Passengers = m_Player.GetPassengers();
        
    }

    
}
