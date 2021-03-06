using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The Scrip for everything related to the EndScreenUI
/// </summary>
public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText = null;               // The label displaying the score text
    [SerializeField] private TMP_Text m_ErrorMessage = null;          // Th label for displaying an error message
    [SerializeField] private TMP_InputField m_InputField = null;        // The input field for the players name
    [SerializeField] private GameObject m_SaveScoreButton = null;       // The button for saving the score

    /// <summary>
    /// Called when the EndScreen loads again
    /// Resets the EndScreenUI
    /// </summary>
	public void Reset()
	{
        m_SaveScoreButton.SetActive(true);
        m_InputField.text = "";
        m_ErrorMessage.text = "";
	}

    /// <summary>
    /// Called after every input to the inputfield
    /// Changes the text from lowercase to uppercase
    /// </summary>
    /// <param name="name"> The current value of the inputfield</param>
	public void ChangeToUpperCase(string name)
	{
        string upperName;
        upperName = name.ToUpper();
        m_InputField.text = upperName;
	}

    /// <summary>
    /// Called when the Endscreen UI is loaded
    /// Sets the score players score value
    /// </summary>
    public void SetScore()
	{
        m_ScoreText.text = "" + ScoreToString(UIController.Instance.GameUI.GetTime());
	}

    private string ScoreToString(float score)
    {
        int minutes = (int)(score / 60);
        int seconds = (int)(score - (minutes * 60));
        string scoreString = "" + minutes + ":" + seconds;
        return scoreString;
    }

    /// <summary>
    /// Called when the "Save Score" button is pressed
    /// Saves the input
    /// </summary>
    public void SaveScore()
	{
        // Makes sure the player filled the input field
        if(m_InputField.text.Length == 3)
		{
            LeaderboardData data = new LeaderboardData(m_InputField.text, UIController.Instance.GameUI.GetTime(), GhostRecorder.Instance.GetPath());
            UIController.Instance.LeaderboardUI.m_leaderBoard.Add(data);
            LeaderboardIO.Instance.SaveLeaderBoard(UIController.Instance.LeaderboardUI.m_leaderBoard);
            GhostRecorder.Instance.ResetData();
            m_SaveScoreButton.SetActive(false);
            m_ErrorMessage.text = "Score saved!";
        }

        else
		{
            DisplayError(0);
		}
	}

    /// <summary>
    /// Called when the player tryes to save score witthout filling input box
    /// Displays an error message
    /// </summary>
    /// <param name="index">The index value of the error message</param>
    private void DisplayError(int index)
	{
        string message;
        
        // Error mesage one
        if(index == 0)
		{
            message = "Must have three characters";
		}

		else
		{
            message = "";
		}

        m_ErrorMessage.text = message;
	}
}
