using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Canvas;

    private UIController m_UIController = null;

    [Header("Player Stats")]
    //private PlayerController m_Player;
    public float m_BoatSpeed;
    public float m_CurrentTrickRotation;
    public int m_Passengers;
    public List<GameObject> m_GhostPickups;
    public Transform m_StartPos;
    public GameObject m_Player;

    public float m_TimeLimit = 50.0f;
    private float m_CurrentTime = 0f;
    public int m_ChosenGhostIndex = 0;
    public bool m_ChoseGhost = false;
    public float GetCurrentTime() { return m_CurrentTime; }



    private static GameManager m_Instance;                       // The current instance of MenuController
    public static GameManager Instance                           // The public current instance of MenuController
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;


    }

    void Start()
    {
      ///  m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_CurrentTime = m_TimeLimit;
        m_UIController = UIController.Instance;

        if(m_UIController != null)
        {
            m_UIController.MenuController.LoadMenu();

        }
    }

    void Update()
    {
        // Time limit
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    m_CurrentTime = m_TimeLimit;
        //}

        //if(m_CurrentTime > 0)
        //{
        //    m_CurrentTime -= Time.deltaTime;
        //}
        //else 
        //{
        //    Debug.Log("Times up");
        //}

      //  m_BoatSpeed = m_Player.playerMovement.GetSpeed();
        //m_CurrentTrickRotation = m_Player.trickManager.currentRotation;
      //  m_Passengers = m_Player.GetPassengers();
        
    }

    public void StartGame()
    { 
        Debug.Log("Start point hit");
        if (m_ChoseGhost && GhostPlayer.Instance.LoadGhost(m_ChosenGhostIndex))
        { 
            GhostPlayer.Instance.ResetAndPlay();   

            GhostRecorder.Instance.StartRecording();
        }

        else
        {
            GhostRecorder.Instance.StartRecording();
		}

		SoundManager.Instance.PlayBGM(0);

        m_UIController.EndScreenUI.Reset();
        m_UIController.GameUI.TimerCounting(true);
    
	}

    public void EndGame()
    {
        Debug.Log("End point hit");
        // GhostRecorder.Instance.SaveRecording();
        GhostRecorder.Instance.StopRecording();
        GhostPlayer.Instance.Stop();
        m_UIController.MenuController.LoadEndScreen();
        m_UIController.GameUI.TimerCounting(false);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.BoostFadeToStop();
        m_ChoseGhost = false;
        ResetGame();
    }

    public void ResetGame()
	{
        for (int i = 0; i < m_GhostPickups.Count; i++)
		{
            m_GhostPickups[i].SetActive(true);
		}

        m_Player.transform.position = m_StartPos.position;
        m_Player.transform.rotation = m_StartPos.rotation;
        PlayerMovement.Instance.ResetMovement();

        m_UIController.GameUI.ResetTime();
	}
}
