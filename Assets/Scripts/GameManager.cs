using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Canvas;
    [SerializeField] private TextMeshProUGUI m_TimerUI;
    [SerializeField] private TextMeshProUGUI m_SpeedUI;
    [SerializeField] private TextMeshProUGUI m_PassengerUI;
    private BoatController m_Boat;

    public float m_TimeLimit = 50.0f;
    private float m_CurrentTime = 0f;

    void Start()
    {
        m_Boat = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<BoatController>();
        m_CurrentTime = m_TimeLimit; 
    }

    
    void Update()
    {
        // Time limit
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_CurrentTime = m_TimeLimit;
        }
        if(m_CurrentTime > 0)
        {
            m_CurrentTime -= Time.deltaTime;
        }
        else 
        {
            Debug.Log("Times up");
        }
        
        UpdateUI();
    }

    void UpdateUI()
    {
        string timerText = "Time: " + m_CurrentTime;
        m_TimerUI.text = timerText;

        string speedText = "Speed: " + m_Boat.GetSpeed();
        m_SpeedUI.text = speedText;

        string passengerText = "Passengers: " + m_Boat.GetPassengers();
        m_PassengerUI.text = passengerText;
    }
}
