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
    [SerializeField] private TextMeshProUGUI m_Hint = null;

    private bool m_Counting = false;                                        // A bool which value determing if the timer is counting
    private float m_Time;                                                   // The value of the timer
    public bool ShowDefalt { get; set; } = true;                            // If the values should be updated or be defaulted

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        if(UIController.Instance.MenuController.m_State == MenuState.GAME && ShowDefalt == false)
        {
            if (m_Counting)
            {
                UpdateUI();
                UpdateTime();
            }
        }
    }

    /// <summary>
    /// Called every updater
    /// Updates all the labels to display accurate information
    /// </summary>
    void UpdateUI()
    {
        // Current speed display
        int value = (int)Mathf.Ceil(PlayerMovement.Instance.GetSpeeds().x);
        string currentSpeedText;
        if (value > PlayerMovement.Instance.m_MaxSpeed - 2.0f)
        {
            value = (int)PlayerMovement.Instance.m_MaxSpeed;
            currentSpeedText = "" + Mathf.Abs(value) + " <size=70%>km/h";
        }
        else
        {
            value = Mathf.Abs((int)Mathf.Ceil(PlayerMovement.Instance.GetSpeeds().x));
            currentSpeedText = "" + value + " <size=70%>km/h";
        }
        m_CurrentSpeedText.text = currentSpeedText;
        
        // Max speed display
        string maxSpeedText = "" + Mathf.Round(PlayerMovement.Instance.GetSpeeds().y);
        m_MaxSpeedUI.text = maxSpeedText + " <size=70%>km/h";

		 //Timer display
		int minutes = (int)(m_Time / 60);
		int milliseconds = (int)(m_Time * 100);
		int wholeSeconds = (milliseconds / 100) - minutes * 60;
		int leftover = (milliseconds % 100);

        string time = "";

        if (minutes > 0)
		{
            time += minutes + ":";
		}

		if (m_Time > 0)
		{
			time += (wholeSeconds < 10 ? "0" : "") + wholeSeconds + (leftover < 10 ? ".<size=80%>0" : ".<size=80%>") + leftover;
			m_TimeUI.text = time;
		}
		else
		{
			time += (wholeSeconds < 10 ? "0" : "") + wholeSeconds + ((-1 * leftover) < 10 ? ".<size=80%>0" : ".<size=80%>") + (-1 * leftover);
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


    public void StopAndResetTime()
    {
        m_Counting = false;
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

    public void ResetGameUI()
	{
        m_TimeUI.text = "Time";
		m_MaxSpeedUI.text = "Max";
        m_CurrentSpeedText.text = "Current";
        m_Counting = false;
        m_Time = 0.0f;
	}

    public void HintSetActive(bool v)
	{
        m_Hint.gameObject.SetActive(v);
	}
   
    IEnumerator ShowTrickControls()
	{
        m_Hint.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        m_Hint.gameObject.SetActive(false);
    }

    public void SetActive(bool v)
	{
        m_TimeUI.transform.parent.gameObject.SetActive(v);
        m_MaxSpeedUI.transform.parent.gameObject.SetActive(v);
        m_CurrentSpeedText.transform.parent.gameObject.SetActive(v); 
    }

    public void ShowTrickTip()
	{
        StartCoroutine(ShowTrickControls());
	}
}
