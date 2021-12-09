using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A class for containing information on a row of the leaderboard
/// </summary>
public class LeaderBoardElement
{
    public GameObject GhostButton;      // The ghost button object
    public GameObject NameLabel;        // The name label object
    public GameObject ScoreLabel;       // The score label object

    /// <summary>
    /// Custom constructor for a Leaderboard row
    /// </summary>
    /// <param name="nameLabel">Prefab for name label</param>
    /// <param name="scoreLabel">Prefab for the score label</param>
    /// <param name="ghostButton">Prefab for the ghost button</param>
	public void SetElements(GameObject nameLabel, GameObject scoreLabel, GameObject ghostButton)
    {
        NameLabel = nameLabel;
        ScoreLabel = scoreLabel;
        GhostButton = ghostButton;
    }

    /// <summary>
    /// Called on the initial creation of the rows
    /// Instantiates all of the object for the row
    /// </summary>
    /// <param name="canvas">The canvas the obects will be childs of </param>
    public void SetParent(Transform canvas)
    {
        NameLabel.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>().transform);
        ScoreLabel.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>().transform);
        GhostButton.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>().transform);
    }

    public void SetScale()
	{
        NameLabel.GetComponent<RectTransform>().localScale = Vector3.one;
        ScoreLabel.GetComponent<RectTransform>().localScale = Vector3.one;
        GhostButton.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    /// <summary>
    /// Sets the value of the name text
    /// </summary>
    /// <param name="text">The string to be set as the name text</param>
	public void SetNameText(string text)
    {
        NameLabel.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// Sets the value of the score text
    /// </summary>
    /// <param name="text">The string to be set as the score text</param>
    public void SetScoreText(string text)
    {
        ScoreLabel.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// Sets the values for all the elements
    /// </summary>
    /// <param name="nameText">The string to be the name text</param>
    /// <param name="scoreText">The string to be set as the score text</param>
    /// <param name="ghostText">The string to be set as the ghost buttons text</param>
    public void SetElementValues(string nameText, string scoreText)
    {
        SetNameText(nameText);
        SetScoreText(scoreText);
    }

    /// <summary>
    /// Sets the X position of the object
    /// </summary>
    /// <param name="xPos"> The X position that the objects are to be set to</param>
    public void SetX(float xPos)
    {
        Vector3 offset = Vector3.zero;
        offset.x = xPos;
        NameLabel.transform.localPosition += offset;
        offset.x += NameLabel.GetComponent<RectTransform>().rect.width;
        ScoreLabel.transform.localPosition += offset;
        offset.x += (ScoreLabel.GetComponent<RectTransform>().rect.width);
        GhostButton.transform.localPosition += offset;
    }

    /// <summary>
    /// Sets the Y position of the objects
    /// </summary>
    /// <param name="yPos">The Y position that the objects are to be set to</param>
    public void SetY(float yPos)
    {
        NameLabel.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
        ScoreLabel.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
        GhostButton.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
    }

    /// <summary>
    /// Gets the height of the name label
    /// </summary>
    /// <returns>The height position of the name label</returns>
    public float GetHeight()
    {
        return NameLabel.GetComponent<RectTransform>().rect.height;
    }

    /// <summary>
    /// Called when the leaderboard is loaded.
    /// Destroys previous rows
    /// </summary>
    //public void Delete()
    //{
    //    Destroy(NameLabel);
    //    Destroy(ScoreLabel);
    //    Destroy(GhostButton);
    //}
}

/// <summary>
/// A script for everything related to the LeaderboardUI
/// </summary>
public class LeaderboardUI : MonoBehaviour
{

    [SerializeField] private GameObject m_ButtonPrefab = null;              // The prefab for the ghost button
    [SerializeField] private GameObject m_LabelPrefab = null;               // The prefab for the label prefab
    [SerializeField] private GameObject m_ScrollviewContent = null;         // The content object of the scrollview which the leaderboard rows are peranted to

    private LeaderBoard m_leaderBoard = null;                                // A class containing the saved data for the leaderboard
    private int m_TotalElements = 0;                                        // The total amount of rows saved
    private LeaderBoardElement[] m_Elements = null;                         // The rows in the leaderboard
   
    public List<int> m_ChosenIndices;                                       // The chosen replay ghost indecies to be spawned on the next run

    /// <summary>
    /// Called on first frame
    /// Loads the leaderboard
    /// </summary>
	public void Start()
	{
        m_leaderBoard = LeaderboardIO.Instance.LoadLeaderBoard();
    }

	/// <summary>
	/// Called when loading the leaderboard,
	/// Instantiates and positions all of the rows
	/// </summary>
	private void CreateRows()
    {
        for (int i = 0; i < m_TotalElements; i++)
        {
            int value = i;
            LeaderBoardElement element = new LeaderBoardElement();
            GameObject label1 = Instantiate(m_LabelPrefab);
            GameObject label2 = Instantiate(m_LabelPrefab);
            GameObject tickBox1 = Instantiate(m_ButtonPrefab);
            element.SetElements(label1, label2, tickBox1);
            m_Elements[value] = element;
            m_Elements[value].SetParent(m_ScrollviewContent.transform);
            m_Elements[value].SetY(0 - (i * m_Elements[value].GetHeight()));
            m_Elements[value].SetX(0);
            m_Elements[value].GhostButton.GetComponent<Toggle>().onValueChanged.AddListener(delegate { AddToReplayList(value, m_Elements[value].GhostButton.GetComponent<Toggle>()); });
            m_Elements[value].SetElementValues(m_leaderBoard.datas[i].playerName, "" + ScoreToString(m_leaderBoard.datas[i].playerScore));
            m_Elements[value].SetScale();
        }
    }
    
    /// <summary>
    /// Called when the "leaderboard" button is pressed,
    /// Loads all the entire leaderboard
    /// </summary>
    public void Load()
    {
        m_leaderBoard = LeaderboardIO.Instance.LoadLeaderBoard();
        m_ChosenIndices = new List<int>();

        // Destroys current leaderboard
        if (m_Elements != null)
        {
            for (int i = 0; i < m_Elements.Length; i++)
            {
                Destroy(m_Elements[i].NameLabel.gameObject);
                Destroy(m_Elements[i].ScoreLabel.gameObject);
                Destroy(m_Elements[i].GhostButton.gameObject);
                m_Elements[i] = null;
            }
        }

        // Resets values
        if (m_leaderBoard != null)
        {
            m_Elements = new LeaderBoardElement[m_leaderBoard.datas.Count];
            m_TotalElements = m_leaderBoard.datas.Count;
            CreateRows();
		}
    }

    /// <summary>
    /// Called when the players saved score needs to be formatted.
    /// Formats the players score and converts to string
    /// </summary>
    /// <param name="score">The players score as a float</param>
    /// <returns></returns>
    private string ScoreToString(float score)
    {
        string time = "";      
        int minutes = (int)(score / 60);
        int milliseconds = (int)(score * 100);
        int wholeSeconds = (milliseconds / 100) - minutes * 60;
        int leftover = (milliseconds % 100);

        if (minutes > 0)
        {
            time += minutes + ":";
        }

        if (score > 0)
        {
            time += (wholeSeconds < 10 ? "0" : "") + wholeSeconds + (leftover < 10 ? ".<size=80%>0" : ".<size=80%>") + leftover;
        }

        else
        {
            time += (wholeSeconds < 10 ? "0" : "") + wholeSeconds + ((-1 * leftover) < 10 ? ".<size=80%>0" : ".<size=80%>") + (-1 * leftover);
        }
        return time;
    }

    /// <summary>
    /// Called when toggle box is pressed.
    /// Adds the index of the toggle box to a list of replays that will be played for the next run.
    /// </summary>
    /// <param name="index">Index of the togglebox</param>
    /// <param name="toggle">The toggle component of the togglebox</param>
    private void AddToReplayList(int index, Toggle toggle)
	{
        if (toggle.isOn)
        {
            m_ChosenIndices.Add(index);
            //toggle.isOn = true;
            Debug.Log("Added ghost at index: " + index);
        }
        else
        {
            m_ChosenIndices.Remove(index);
            //toggle.isOn = false;
            Debug.Log("Removed ghost at index: " + index);
        }
    }

    /// <summary>
    /// Called when the current leaderboard needs to be used.
    /// </summary>
    /// <returns>The current Leaderboard</returns>
    public LeaderBoard GetLeaderboard()
	{
        return m_leaderBoard;
	}
}
