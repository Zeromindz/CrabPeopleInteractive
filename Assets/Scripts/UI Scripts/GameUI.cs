using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private TextMeshProUGUI m_TimeUI = null;
    [SerializeField] private TextMeshProUGUI m_CurrentSpeedText = null;
    [SerializeField] private TextMeshProUGUI m_MaxSpeedUI = null;

    private bool m_Counting = false;
    private float m_Time;

    void Update()
    {
        UpdateUI();
        if (m_Counting)
        {
            UpdateTime();
        }

    }

    void UpdateUI()
    {
        string currentSpeedText = "" + Mathf.Floor(PlayerMovement.Instance.GetSpeeds().x);
        m_CurrentSpeedText.text = currentSpeedText;

        string maxSpeedText = "" + Mathf.Round(PlayerMovement.Instance.GetSpeeds().y);
        m_MaxSpeedUI.text = maxSpeedText;

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
            string time = "Time: " + wholeSeconds + ((-1 * leftover) < 10 ? ".0" : ".") + (-1* leftover);
            m_TimeUI.text = time;
        }
    }
    
    public void UpdateTime()
    {
        m_Time += Time.deltaTime;
    }

    public void ResetTime()
    {
        m_Time = 0.0f;
    }

    public void TakeTime()
	{
        m_Time -= 3.0f;
	}
    
    public float GetTime()
	{
        return m_Time;
	}

    public void TimerCounting(bool state)
	{
        m_Counting = state;
	}
}
