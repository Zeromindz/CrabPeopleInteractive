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
        NameLabel.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
        ScoreLabel.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
        GhostButton.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
    }

    public float GetHeight()
	{
        return NameLabel.GetComponent<RectTransform>().rect.height;
	}

    public void MoveYDir(float yDirection)
	{
        Vector3 direction = new Vector3(0.0f, yDirection, 0.0f);
        NameLabel.transform.localPosition += direction;
        ScoreLabel.transform.localPosition += direction;
        GhostButton.transform.localPosition += direction;  
    }

    public float GetYPos()
	{
        return NameLabel.transform.localPosition.y;
	}
}

public class LeaderboardUI : MonoBehaviour
{
    public GameObject counter;

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
    
    private int m_TopIndex, m_BottomIndex;

    private int m_TopMostIndex, m_BottomMostIndex;

    private int m_CountingIndex;


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
            
            if(value < m_TotalElements)
			{
                m_Element[value].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
			}

			else
			{
                m_Element[value].SetElementValues("N/A", "N/A", "N/A");
            }

            m_Element[value].GhostButton.GetComponent<Button>().onClick.AddListener(() => {OnbuttonPress(value); });
	   }

        m_TopIndex = 0;
        m_TopMostIndex = m_TopIndex;
        m_BottomIndex = m_ElementsPerPage - 1;
        m_BottomMostIndex = m_BottomIndex;
        m_CountingIndex = m_TopIndex;
    }

	private void Update()
	{
        //TextMeshProUGUI text = counter.GetComponent<TextMeshProUGUI>();
      //  text.text = "" + m_CountingIndex;
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

	public void Move(float direction)
	{
        float yMovement = direction *  m_Element[0].GetHeight();
        Debug.Log("Move: " + yMovement);

        for(int i = 0; i < m_Element.Length; i++)
		{
            m_Element[i].MoveYDir(yMovement);
		}
	}

	public void WrapElements(float yValue)
	{
		if (yValue > 0)
		{
            if (m_CountingIndex - 1 < 0)
            {

            }
            else
            {
                m_Element[m_BottomIndex].SetY(m_Element[m_TopIndex].GetYPos() + m_Element[m_TopIndex].GetHeight());
                m_Element[m_BottomIndex].SetX(m_leftmostXPosition);
                int value = m_CountingIndex - 1;
                m_Element[m_BottomIndex].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
                m_Element[m_BottomIndex].GhostButton.GetComponent<Button>().onClick.RemoveListener(() => { OnbuttonPress(value - 2); });
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

                Move(yValue);
            }
        }

        else
		{
            if((m_CountingIndex + m_ElementsPerPage + 1) > m_TotalElements)
			{

			}
            else
			{
			    m_Element[m_TopIndex].SetY(m_Element[m_BottomIndex].GetYPos() - m_Element[m_BottomIndex].GetHeight());
                m_Element[m_TopIndex].SetX(m_leftmostXPosition);
                int value = m_CountingIndex + m_ElementsPerPage;
                m_Element[m_TopIndex].SetElementValues("Name: " + value, "Score: " + value, "Button: " + value);
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
                    m_CountingIndex++;
			    }

                Move(yValue);
			}
			
		}
	}

    private void UpdateText(float yValue)
	{
        if (yValue > 0)
        {
            int num = m_CountingIndex;
            m_Element[m_BottomIndex].SetElementValues("Name: " + num, "Score: " + num, "Button: " + num);
            Debug.Log("Counting index: " + m_CountingIndex);
            Debug.Log("Num" + num);
        }

        else
        {
            int num = m_CountingIndex - m_ElementsPerPage;
            m_Element[m_TopIndex].SetElementValues("Name: " + num, "Score: " + num, "Button: " + num);
            Debug.Log("Counting index: " + m_CountingIndex);
            Debug.Log("Num: " + num);

        }
    }
}
