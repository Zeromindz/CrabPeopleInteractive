using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText = null;
    [SerializeField] private TMP_InputField m_InputField = null;
    [SerializeField] private GameObject m_SaveScoreButton = null;

    private void Awake()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Reset()
	{
        m_SaveScoreButton.SetActive(true);
        m_InputField.text = "";
	}

	public void ChangeToUpperCase(string name)
	{
        string upperName;
        upperName = name.ToUpper();
        m_InputField.text = upperName;
	}

    public void SetScore()
	{
        m_ScoreText.text = "" + UIController.Instance.GameUI.GetTime();
	}

    public void SaveScore()
	{
        LeaderboardIO.Instance.SaveLeaderBoardRow(m_InputField.text, UIController.Instance.GameUI.GetTime(), GhostRecorder.Instance.GetPath());
        GhostRecorder.Instance.ResetData();
        m_SaveScoreButton.SetActive(false);

	}
}
