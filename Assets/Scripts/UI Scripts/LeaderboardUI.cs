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
	public LeaderBoardElement(GameObject nameLabel, GameObject scoreLabel, GameObject ghostButton)
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
    public void InstantiateElement(Canvas canvas)
    {
        NameLabel = Instantiate(NameLabel, canvas.transform, false);
        ScoreLabel = Instantiate(ScoreLabel, canvas.transform, false);
        GhostButton = Instantiate(GhostButton, canvas.transform, false);
    }

    /// <summary>
    /// Sets the value of the name text
    /// </summary>
    /// <param name="text">The string to be set as the name text</param>
	public void SetNameText(string text)
    {
        NameLabel.GetComponent<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// Sets the value of the score text
    /// </summary>
    /// <param name="text">The string to be set as the score text</param>
    public void SetScoreText(string text)
    {
        ScoreLabel.GetComponent<TextMeshProUGUI>().text = text;
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
    public void SetElementValues(string nameText, string scoreText, string ghostText)
    {
        SetNameText(nameText);
        SetScoreText(scoreText);
        SetButtonText(ghostText);
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
        offset.x += ScoreLabel.GetComponent<RectTransform>().rect.width;
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
}

/// <summary>
/// A script for everything related to the LeaderboardUI
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonPrefab;                     // The prefab for the ghost button
    [SerializeField] private GameObject m_LabelPrefab;                      // The prefab for the label prefab
    [SerializeField] private Canvas m_Canvas;                               // The canvas which the buttons will be childs of 
    [SerializeField, Range(0, 10)] private int m_ElementsPerPage;           // The amount of rows in the leaderboard
    [SerializeField, Range(-530, 530)] private int m_TopYPosition;          // The top most Y position of the first label
    [SerializeField, Range(-810, 810)] private int m_leftmostXPosition;     // The left most X position of the first label

    private int m_TotalElements;                                            // The total amount of rows saved
    private LeaderBoardElement[] m_Element = null;                          // The rows in the leaderboard
    private int m_TopIndex, m_BottomIndex;                                  // The top and bottom index the current index at the top and bottom possition 
    private int m_TopMostIndex, m_BottomMostIndex;                          // The top index (0) and the bottom most index (m_ElementsPerPage - 1)
    private int m_CountingIndex;                                            // The index of the top row not being wrapped within the row range              

    /// <summary>
    /// Called when script is loaded
    /// Caches needed information
    /// </summary>
    private void Awake()
    {
        m_Element = new LeaderBoardElement[m_ElementsPerPage];
    }

    /// <summary>
    /// Called before the first update is called
    /// Creates all the rows and sets all the values
    /// </summary>
    void Start()
    {
        for (int i = 0; i < m_Element.Length; i++)
        {
            int value = i;
            m_Element[value] = new LeaderBoardElement(m_LabelPrefab, m_LabelPrefab, m_ButtonPrefab);
            m_Element[value].InstantiateElement(m_Canvas);
            m_Element[value].SetY(m_TopYPosition - (i * m_Element[i].GetHeight()));
            m_Element[value].SetX(m_leftmostXPosition);

            if (value < m_TotalElements)
            {
                m_Element[value].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
            }

            else
            {
                m_Element[value].SetElementValues("N/A", "N/A", "N/A");
            }

            m_Element[value].GhostButton.GetComponent<Button>().onClick.AddListener(() => { OnbuttonPress(value); });
        }

        m_TopIndex = 0;
        m_TopMostIndex = m_TopIndex;
        m_BottomIndex = m_ElementsPerPage - 1;
        m_BottomMostIndex = m_BottomIndex;
        m_CountingIndex = m_TopIndex;
        LoadElementAmount();
    }

    /// <summary>
    /// Function that runs on the button being pressed 
    /// </summary>
    /// <param name="index"></param>
	private void OnbuttonPress(int index)
    {
        Debug.Log("Button " + index + " Pressed!");
        LoadGhost(index);
    }

    private void LoadGhost(int index)
    {
        GameManager.Instance.m_ChosenGhostIndex = index;
        GameManager.Instance.m_ChoseGhost = true;
        GameManager.Instance.ResetGame();
        UIController.Instance.MenuController.LoadGame();
    }

    private void LoadElementAmount()
    {
        m_TotalElements = LeaderboardIO.Instance.LoadRowAmount().rowAmount;
    }

    /// <summary>
    /// Called when wrapping
    /// moves all rows on the Y direction
    /// </summary>
    /// <param name="direction"></param>
	public void Move(float direction)
    {
        float yMovement = direction * m_Element[0].GetHeight();
        Debug.Log("Move: " + yMovement);

        for (int i = 0; i < m_Element.Length; i++)
        {
            m_Element[i].MoveYDir(yMovement);
        }
    }

    /// <summary>
    /// Called when the leaderboard navigation button is pressed,
    /// Wraps the elements  desending on the value
    /// </summary>
    /// <param name="yValue">The value that determins the direction of the wrap</param>
	public void WrapElements(float yValue)
    {
        if (yValue > 0)
        {
            // Does nothing if the index is out of range
            if (m_CountingIndex - 1 < 0)
            {

            }
            else
            {
                m_Element[m_BottomIndex].SetY(m_Element[m_TopIndex].GetYPos() + m_Element[m_TopIndex].GetHeight());
                m_Element[m_BottomIndex].SetX(m_leftmostXPosition);
                int value = m_CountingIndex - 1;
                //  m_Element[m_BottomIndex].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
                m_Element[m_BottomIndex].GhostButton.GetComponent<Button>().onClick.RemoveAllListeners();
                m_Element[m_BottomIndex].GhostButton.GetComponent<Button>().onClick.AddListener(() => { OnbuttonPress(value); });

                // The original top row index is now at the bottom, resets to top 
                if (m_BottomIndex == m_TopMostIndex)
                {
                    m_BottomIndex = m_BottomMostIndex;
                    m_TopIndex = m_TopMostIndex;
                    m_CountingIndex--;
                }

                // Scrolls down indecies by one
                else
                {
                    m_TopIndex = m_BottomIndex;
                    m_BottomIndex = m_BottomIndex - 1;
                    m_CountingIndex--;
                }

                LeaderboardData save = LeaderboardIO.Instance.LoadLeaderBoardData(value);
                m_Element[m_TopIndex].SetElementValues(save.playerName, "" + ScoreToString(save.playerScore), "");
                Move(-1);

            }
        }

        else
        {
            // Does nothing if the index is out of range
            if ((m_CountingIndex + m_ElementsPerPage + 1) > m_TotalElements)
            {

            }
            else
            {
                m_Element[m_TopIndex].SetY(m_Element[m_BottomIndex].GetYPos() - m_Element[m_BottomIndex].GetHeight());
                m_Element[m_TopIndex].SetX(m_leftmostXPosition);

                int value = m_CountingIndex + m_ElementsPerPage;
                if (value == 11)
                {
                    Debug.Log("11");
                }
                m_Element[m_TopIndex].SetElementValues("Name: " + value, "Score: " + value, "");
                m_Element[m_TopIndex].GhostButton.GetComponent<Button>().onClick.RemoveAllListeners();
                m_Element[m_TopIndex].GhostButton.GetComponent<Button>().onClick.AddListener(() => { OnbuttonPress(value); });

                // The original bottom row is now at the top, resets to bottom
                if (m_TopIndex == m_BottomMostIndex)
                {
                    m_TopIndex = m_TopMostIndex;
                    m_BottomIndex = m_BottomMostIndex;
                }

                // Scrolls up indecies by one
                else
                {
                    m_BottomIndex = m_TopIndex;
                    m_TopIndex = m_TopIndex + 1;
                }

                m_CountingIndex++;
                LeaderboardData save = LeaderboardIO.Instance.LoadLeaderBoardData(value);
                m_Element[m_BottomIndex].SetElementValues(save.playerName, "" + ScoreToString(save.playerScore), "");
                Move(1);
            }
        }
    }

    /// <summary>
    /// Called when the leaderboard is first created,
    /// Loads the first page of elements
    /// </summary>
    public void Load()
    {
        RowAmount rows = LeaderboardIO.Instance.LoadRowAmount();
        for (int i = 0; i < rows.rowAmount; i++)
        {
            if (i < m_ElementsPerPage)
            {
                LeaderboardData save = LeaderboardIO.Instance.LoadLeaderBoardData(i);
                m_Element[i].SetElementValues(save.playerName, "" + save.playerScore, "");
            }
        }

        LoadElementAmount();
    }

    public void ClearLeaderboard()
    {
        LeaderboardIO.Instance.ClearRows();
        Reload();
    }

    public void Reload()
    {
        Load();

        m_TopIndex = 0;
        m_TopMostIndex = m_TopIndex;
        m_BottomIndex = m_ElementsPerPage - 1;
        m_BottomMostIndex = m_BottomIndex;
        m_CountingIndex = m_TopIndex;
    }

    private string ScoreToString(float score)
    {
        int minutes = (int)(score / 60);
        int seconds = (int)(score - (minutes * 60));
        string scoreString = "" + minutes + ":" + seconds;
        return scoreString;
    }
}
