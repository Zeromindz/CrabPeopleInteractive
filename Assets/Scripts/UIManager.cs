using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private TextMeshProUGUI m_TimerUI;
    [SerializeField] private TextMeshProUGUI m_SpeedUI;
    [SerializeField] private TextMeshProUGUI m_PassengerUI;
    [SerializeField] private TextMeshProUGUI m_TrickUI;

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        string timerText = "Time: " + m_GameManager.GetCurrentTime();
        m_TimerUI.text = timerText;

        string speedText = "Speed: " + Mathf.Round(m_GameManager.m_BoatSpeed);
        m_SpeedUI.text = speedText;

        string passengerText = "Passengers: " + m_GameManager.m_Passengers;
        m_PassengerUI.text = passengerText;

        string trickText = "Trick: " ;
        m_TrickUI.text = trickText;
    }
}
