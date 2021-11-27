﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum GameState
{
    InRun,
    NotInRun
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform m_Camera = null;
    private UIController m_UIController = null;

    [Header("Player Stats")]
    //private PlayerController m_Player;
    public float m_BoatSpeed;
    public float m_CurrentTrickRotation;
    public int m_Passengers;
    public List<GameObject> m_GhostPickups;
    public Transform m_StartPos;
    public GameObject m_Player;

    [SerializeField] GameObject m_ReplayGhostPrefab = null;
    public List<int> m_ChosenGhostIndices;
    public List<GameObject> m_ReplayGhosts = null;
    private bool IsPlaying = false;

    public float m_TimeLimit = 50.0f;
    private float m_CurrentTime = 0f;
    public bool m_ChoseGhost = false;
    public float GetCurrentTime() { return m_CurrentTime; }
    private GameState m_State = GameState.NotInRun;
    public GameState State
    {
        get { return m_State; }
        set { m_State = value; }
    }


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
        m_ReplayGhosts = new List<GameObject>();
    }

    void Start()
    {
      ///  m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_CurrentTime = m_TimeLimit;
        m_UIController = UIController.Instance;

        m_ChosenGhostIndices = new List<int>();

        if(m_UIController != null)
        {
            m_UIController.MenuController.LoadMenu();
        }
    }

    void Update()
    {
        if (m_State == GameState.InRun)
		{
            if (m_ReplayGhosts[1] == null)
			{

			}
		}
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
        if (InstantiateReplays())
        {
            ResetAndPlayReplays();  

            GhostRecorder.Instance.StartRecording();
        }

        else
        {
            GhostRecorder.Instance.StartRecording();
		}

        m_State = GameState.InRun;
        m_UIController.GameUI.TimerCounting(true);
	}

    public void EndGame()
    {
        Debug.Log("End point hit");
        m_State = GameState.NotInRun;
        // GhostRecorder.Instance.SaveRecording();
        GhostRecorder.Instance.StopRecording();
        StopReplays();
        m_UIController.MenuController.LoadEndScreen();
        m_UIController.GameUI.TimerCounting(false);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.BoostFadeToStop();
        m_ChoseGhost = false;        
    }

    public void ResetGame()
	{
        for (int i = 0; i < m_GhostPickups.Count; i++)
		{
            m_GhostPickups[i].SetActive(true);
		}

        m_Player.transform.position = m_StartPos.position;
        m_Player.transform.rotation = m_StartPos.rotation;
        m_Camera.transform.position = m_StartPos.position;
        m_Camera.transform.rotation = m_StartPos.rotation;
        PlayerMovement.Instance.ResetMovement();
        PlayerController.Instance.passengerManager.ResetPassengers();

        m_UIController.GameUI.ResetTime();
	}

    public bool InstantiateReplays()
 	{
        bool loaded = true; 

        for(int i = 0; i < m_ChosenGhostIndices.Count; i++)
		{
            GameObject obj = Instantiate(m_ReplayGhostPrefab, m_Player.transform.position, m_Player.transform.rotation) as GameObject;
            loaded = obj.GetComponent<GhostPlayer>().LoadGhost(m_ChosenGhostIndices[i]);
            if(obj != null)
			{
                Debug.Log(loaded + "" + i);
			}

			if (!loaded)
			{
                return loaded;
			}

			else
			{
				m_ReplayGhosts.Add(obj);
                MiniMap.Instance.AddGhost(obj.transform);
			}
		}
        return loaded;
	}

    public void PlayReplays()
	{
        for(int i = 0; i < m_ReplayGhosts.Count; i++)
		{
            m_ReplayGhosts[i].GetComponent<GhostPlayer>().Play();
		}
        IsPlaying = true;
	}

    public void StopReplays()
	{
        for (int i = 0; i < m_ReplayGhosts.Count; i++)
        {
            m_ReplayGhosts[i].GetComponent<GhostPlayer>().Stop();
        }
        IsPlaying = false;
    }

	public void ResetAndPlayReplays()
	{
        for (int i = 0; i < m_ReplayGhosts.Count; i++)
        {
            m_ReplayGhosts[i].GetComponent<GhostPlayer>().ResetAndPlay();
        }
        IsPlaying = true;
    }

    public bool GetIsPlaying()
	{
        return IsPlaying;
	}

	public void DestroyGhost()
	{
        MiniMap.Instance.RemoveGhosts();
        for (int i = 0; i < m_ChosenGhostIndices.Count; i++)
        {
            Destroy(m_ReplayGhosts[i]);
        }
        m_ChosenGhostIndices.Clear();
    }

    public Transform GetCamera()
	{
        return m_Camera;
	}
}
