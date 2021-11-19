using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The Scrip for everything related to the GameUI
/// </summary>
public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimeUI = null;               // The label displaying the timer
    [SerializeField] private TextMeshProUGUI m_CurrentSpeedText = null;     // The label displaying the current speed of the vehicle
    [SerializeField] private TextMeshProUGUI m_MaxSpeedUI = null;           // The label displaying the max speed of the vehicle
    [SerializeField] private float m_CheckpointPointTimeValue;              // The value in time that will be negated 

    private bool m_Counting = false;                                        // A bool which value determing if the timer is counting
    private float m_Time;                                                   // The value of the timer

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        UpdateUI();
        if (m_Counting)
        {
            UpdateTime();
        }
    }

    /// <summary>
    /// Called every updater
    /// Updates all the labels to display accurate information
    /// </summary>
    void UpdateUI()
    {
        // Current speed display
        string currentSpeedText = "" + Mathf.Floor(PlayerMovement.Instance.GetSpeeds().x);
        m_CurrentSpeedText.text = currentSpeedText;
        
        // Max speed display
        string maxSpeedText = "" + Mathf.Round(PlayerMovement.Instance.GetSpeeds().y);
        m_MaxSpeedUI.text = maxSpeedText;

        // Timer display
        int centseconds = (int)(m_Time * 100);
        int wholeSeconds = (centseconds / 100);
        int leftover = (centseconds % 100);

        if (m_Time > 0)
		{
            string time = "" + wholeSeconds + (leftover < 10 ? ".0" : ".") + leftover;
            m_TimeUI.text = time;
		}
		else
		{
            string time = "" + wholeSeconds + ((-1 * leftover) < 10 ? ".0" : ".") + (-1* leftover);
            m_TimeUI.text = time;
        }
    }
    
    /// <summary>
    /// Called when the timer is counting,
    /// Adds deltatime to the time
    /// </summary>
    public void UpdateTime()
    {
        m_Time += Time.deltaTime;
    }

    /// <summary>
    /// Called when the game is to be reset,
    /// Reets the time value back to 0
    /// </summary>
    public void ResetTime()
    {
        m_Time = 0.0f;
    }

    /// <summary>
    /// Called whenever a checkpoint is hit,
    /// Takes X amount of time away from the timer
    /// </summary>
    public void TakeTime()
	{
        m_Time -= m_CheckpointPointTimeValue;
	}
    
    /// <summary>
    /// Called at the End screen to display the time value
    /// </summary>
    /// <returns>The timers value of time</returns>
    public float GetTime()
	{
        return m_Time;
	}

    /// <summary>
    /// Called whenever the timer needs to switch states, 
    /// Enables counting or disables counting
    /// </summary>
    /// <param name="state"></param>
    public void TimerCounting(bool state)
	{
        m_Counting = state;
	}
}
