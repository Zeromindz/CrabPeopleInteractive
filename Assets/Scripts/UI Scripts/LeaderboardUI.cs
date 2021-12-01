using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A class for containing information on a row of the leaderboard
/// </summary>
public class LeaderBoardElement : MonoBehaviour
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
        NameLabel.transform.parent = canvas.transform;
        ScoreLabel.transform.parent = canvas.transform;
        GhostButton.transform.parent = canvas.transform;
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
    /// Sets the value of the ghost buttons text
    /// </summary>
    /// <param name="text">The string to be set as the ghost buttons text</param>
    public void SetButtonText(string text)
    {
        GhostButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
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
    /// Moves the objects on the Y position
    /// </summary>
    /// <param name="yDirection"> The direction in the Y position to be moved</param>
    public void MoveYDir(float yDirection)
    {
        Vector3 direction = new Vector3(0.0f, yDirection, 0.0f);
        NameLabel.transform.localPosition += direction;
        ScoreLabel.transform.localPosition += direction;
        GhostButton.transform.localPosition += direction;
    }

    /// <summary>
    /// Gets the Y position of the name label 
    /// </summary>
    /// <returns>The Y position of the name label</returns>
    public float GetYPos()
    {
        return NameLabel.transform.localPosition.y;
    }
    public void Delete()
    {
        Destroy(NameLabel);
        Destroy(ScoreLabel);
        Destroy(GhostButton);
    }
}

/// <summary>
/// A script for everything related to the LeaderboardUI
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    public LeaderBoard m_leaderBoard = null;

    [SerializeField] private GameObject m_ButtonPrefab = null;                     // The prefab for the ghost button
    [SerializeField] private GameObject m_LabelPrefab = null;                      // The prefab for the label prefab
    [SerializeField] private GameObject m_ScrollviewContent = null;
    //[SerializeField] private Canvas m_Canvas = null;                               // The canvas which the buttons will be childs of 
    //[SerializeField, Range(0, 10)] private int m_ElementsPerPage;           // The amount of rows in the leaderboard
   /* [SerializeField, Range(-530, 530)]*/ private int m_TopYPosition;          // The top most Y position of the first label
    /*[SerializeField, Range(-810, 810)]*/ private int m_leftmostXPosition = 0; // The left most X position of the first label

    private int m_TotalElements = 0;                                            // The total amount of rows saved
    private LeaderBoardElement[] m_Elements = null;                          // The rows in the leaderboard
    //private int m_TopIndex, m_BottomIndex;                                  // The top and bottom index the current index at the top and bottom possition 
    //private int m_TopMostIndex, m_BottomMostIndex;                          // The top index (0) and the bottom most index (m_ElementsPerPage - 1)
    //private int m_CountingIndex;                                            // The index of the top row not being wrapped within the row range
    public List<int> m_ChosenIndices;
    bool LeaderboardExists;
    //private bool buttonChanged = true;

    /// <summary>
    /// Called when script is loaded
    /// Caches needed information
    /// </summary>
    private void Awake()
    {
    }

    /// <summary>
    /// Called before the first update is called
    /// Creates all the rows and sets all the values
    /// </summary>
    void Start()
    {
    }

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
            m_Elements[value].SetY(m_TopYPosition - (i * m_Elements[value].GetHeight()));
            m_Elements[value].SetX(m_leftmostXPosition);

            m_Elements[value].GhostButton.GetComponent<Toggle>().onValueChanged.AddListener(delegate { AddToReplayList(value, m_Elements[value].GhostButton.GetComponent<Toggle>()); });
            //m_Element[value].GhostButton.GetComponent<Button>().onClick.AddListener(() => { OnbuttonPress(value); });
        }
    }
    
    /// <summary>
    /// Called when the leaderboard is first created,
    /// Loads all the entire leaderboard
    /// </summary>
    public void Load()
    {
        m_leaderBoard = LeaderboardIO.Instance.LoadLeaderBoard();
        m_ChosenIndices = new List<int>();

        if (m_Elements != null)
        {
            for (int i = 0; i < m_Elements.Length; i++)
            {
                m_Elements[i].Delete();
                m_Elements[i] = null;
            }
        }
        if (m_leaderBoard != null)
        {

            m_Elements = new LeaderBoardElement[m_leaderBoard.datas.Count];
            m_TotalElements = m_leaderBoard.datas.Count;
		}
		else
		{
            m_TotalElements = 0;
		}
        CreateRows();
        for (int i = 0; i < m_TotalElements; i++)
        {
            if (i < m_TotalElements)
            {  
                if(m_TotalElements > i)
				{
                    m_Elements[i].SetElementValues(m_leaderBoard.datas[i].playerName, "" + ScoreToString(m_leaderBoard.datas[i].playerScore));
                }
            }
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
        int minutes = (int)(score / 60);
        int seconds = (int)(score - (minutes * 60));
        string scoreString = "" + minutes + ":" + seconds;
        return scoreString;
    }

    /// <summary>
    /// Called when toggle box is pressed.
    /// Adds the index of the toggle box to a list of replays that will be played for the next run.
    /// </summary>
    /// <param name="index">Index of the togglebox</param>
    /// <param name="toggle">The toggle component of the togglebox</param>
    public void AddToReplayList(int index, Toggle toggle)
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
}
