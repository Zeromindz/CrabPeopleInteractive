using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardElement : MonoBehaviour
{
    public GameObject GhostButton;
    public GameObject NameLabel;
    public GameObject ScoreLabel;

    public LeaderBoardElement(GameObject nameLabel, GameObject scoreLabel, GameObject ghostButton)
	{
        NameLabel = nameLabel;
        ScoreLabel = scoreLabel;
        GhostButton = ghostButton;
	}

   public void InstantiateElement(Canvas canvas)
   {
       NameLabel = Instantiate(NameLabel, canvas.transform, false);
       ScoreLabel = Instantiate(ScoreLabel, canvas.transform, false);
	   GhostButton = Instantiate(GhostButton, canvas.transform, false); 
	}

	public void SetNameText(string text)
	{
        NameLabel.GetComponent<TextMeshProUGUI>().text = text;
	}
    
    public void SetScoreText(string text)
    {
        ScoreLabel.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetButtonText(string text)
	{
        GhostButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void SetElementValues(string nameText, string scoreText, string ghostText)
	{
        SetNameText(nameText);
        SetScoreText(scoreText);
        SetButtonText(ghostText);
	}
    
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

    public void SetY(float yPos)
	{
        NameLabel.transform.localPosition += new Vector3(0.0f, yPos, 0.0f);
        ScoreLabel.transform.localPosition += new Vector3(0.0f, yPos, 0.0f);
        GhostButton.transform.localPosition += new Vector3(0.0f, yPos, 0.0f);
    }

    public float GetHeight()
	{
        return NameLabel.GetComponent<RectTransform>().rect.height;
	}
}

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField]
    public GameObject m_ButtonPrefab;

    [SerializeField]
    public GameObject m_LabelPrefab;

    [SerializeField]
    public Canvas m_Canvas;

    [SerializeField, Range(0, 10)]
    int m_ElementsPerPage;

    [SerializeField, Range(0, 20)]
    int m_TotalElements;

    [SerializeField, Range(-530, 530)]
    int m_TopYPosition;

    [SerializeField, Range(-810, 810)]
    int m_leftmostXPosition;

    private LeaderBoardElement[] m_Element = null;

	private void Awake()
	{
        m_Element = new LeaderBoardElement[m_ElementsPerPage];
    }

	// Start is called before the first frame update
	void Start()
    {
       for (int i = 0; i < m_Element.Length; i++)
	   {
            int value = i;
            m_Element[value] = new LeaderBoardElement(m_LabelPrefab, m_LabelPrefab, m_ButtonPrefab);
            m_Element[value].InstantiateElement(m_Canvas);
            m_Element[value].SetY(m_TopYPosition - (i * m_Element[i].GetHeight()));
            m_Element[value].SetX(m_leftmostXPosition);

            if(value + 1 < m_ElementsPerPage)
			{
                m_Element[value].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
			}

			else
			{
                m_Element[value].SetElementValues("N/A", "N/A", "N/A");
            }

            m_Element[value].GhostButton.GetComponent<Button>().onClick.AddListener(() => {OnbuttonPress(value); });
	   }
    }

	private void OnbuttonPress(int index)
	{
        Debug.Log("Button " + index + " Pressed!");
	}

    private void LoadGhost(int index)
	{
        //Loads a chosen ghost
	}

    private void LoadElementAmount()
	{
        // loads the amount of elements from file
	} 
}
