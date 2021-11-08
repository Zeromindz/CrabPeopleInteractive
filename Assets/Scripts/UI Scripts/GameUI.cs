using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private TextMeshProUGUI m_TimerUI;
    [SerializeField] private TextMeshProUGUI m_CurrentSpeedText;
    [SerializeField] private TextMeshProUGUI m_MaxSpeedUI;

    void Start()
    {

        m_GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        UpdateUI();

    }

    // To be changed to the gamemanager singleton
    void UpdateUI()
    {
        //string timerText = "Time: " + (int)GameManager.Instance.GetCurrentTime();
        //m_TimerUI.text = timerText;

        //string speedText = "Speed: " + Mathf.Round(Player);
        //m_SpeedUI.text = speedText;

        //string passengerText = "Passengers: " + GameManager.Instance.m_Passengers;
        //m_PassengerUI.text = passengerText;

        //string trickText = "Trick: " + GameManager.Instance.m_CurrentTrickRotation;
        //m_TrickUI.text = trickText;
    }
}
