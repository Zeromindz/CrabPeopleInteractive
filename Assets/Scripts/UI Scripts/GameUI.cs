using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private TextMeshProUGUI m_TimerUI;
    [SerializeField] private TextMeshProUGUI m_SpeedUI;
    [SerializeField] private TextMeshProUGUI m_PassengerUI;
    [SerializeField] private TextMeshProUGUI m_TrickUI;
    [SerializeField] private TextMeshProUGUI m_CountDown;

    private bool m_CountingDown = false;                            // IF the coundown is happening
    private int m_CountDownTime = 4;                                // The time it takes to resume the game always set to the time + 1 so the player can see the full countdown
    private float m_Timer = 0;										// The time that has elapsed since the countdown has started

    void Start()
    {
        m_CountDown.SetText("");
        m_GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        UpdateUI();

        if (m_CountingDown)
        {
            CountDown();
        }
    }

    // To be changed to the gamemanager singleton
    void UpdateUI()
    {
        string timerText = "Time: " + (int)GameManager.Instance.GetCurrentTime();
        m_TimerUI.text = timerText;

        string speedText = "Speed: " + Mathf.Round(GameManager.Instance.m_BoatSpeed);
        m_SpeedUI.text = speedText;

        string passengerText = "Passengers: " + GameManager.Instance.m_Passengers;
        m_PassengerUI.text = passengerText;

        string trickText = "Trick: " + GameManager.Instance.m_CurrentTrickRotation;
        m_TrickUI.text = trickText;
    }

    public void StartCountDown()
	{
        m_CountingDown = true;
	}

    /// <summary>
	/// Called when starting or resuming the game
	/// Counts down from 3
	/// </summary>
	private void CountDown()
    {
        m_Timer += Time.unscaledDeltaTime;
        if (m_Timer >= m_CountDownTime)
        {
            m_CountingDown = false;
            m_CountDown.SetText("");
            Time.timeScale = 1;
            m_Timer = 0;
        }

        else
        {
            m_CountDown.SetText("" + ((int)(m_CountDownTime - m_Timer)));
        }
    }
}
