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
    
    [Header("Player Stats")]
    //private PlayerController m_Player;
    public float m_BoatSpeed;
    public float m_CurrentTrickRotation;
    public int m_Passengers;
    public List<GameObject> m_CheckpointObjects;
    public List<GameObject> m_GhostPickups;
    public Transform m_StartPos;
    public GameObject m_Player;

    public float m_TimeLimit = 50.0f;
    private float m_CurrentTime = 0f;
    private Stack<GameObject> m_checkPoints;
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

        m_checkPoints = new Stack<GameObject>();
        m_CheckpointObjects.Reverse();

        for (int i = 0; i < m_CheckpointObjects.Count; i++)
        {
            m_checkPoints.Push(m_CheckpointObjects[i]);
        }
    }

    void Start()
    {
      ///  m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_CurrentTime = m_TimeLimit;

        MenuController.Instance.LoadMenu();
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


    public void RestartGame()
    {
        //m_Player.transform.position = m_StartPoint.position;
        //m_Player.transform.rotation = m_StartPoint.rotation;
    }
    public void StartGame()
    { 
        Debug.Log("Start point hit");

        if (GhostSave.Instance.DoesSaveExist())
        {
            GhostPlayer.Instance.LoadGhost();
            GhostPlayer.Instance.Play();
            GhostRecorder.Instance.StartRecording();
        }
        else
        {
            GhostRecorder.Instance.StartRecording();
        }

    }

    private void SpawnNextCheckpoint()
    {
        GameObject nextLocation = m_checkPoints.Pop();
        nextLocation.SetActive(true);
    }

    public void EndGame()
    {
        Debug.Log("End point hit");
        // GhostRecorder.Instance.SaveRecording();
        GhostRecorder.Instance.SaveRecording();
        GhostPlayer.Instance.Stop();
        MenuController.Instance.LoadEndScreen();
        ResetGame();
    }

    public void CheckPointHit()
    {
        Debug.Log("CheckPoint hit");
        SpawnNextCheckpoint();
    }
    public void ResetGame()
	{
        for (int i = 0; i < m_GhostPickups.Count; i++)
		{
            m_GhostPickups[i].SetActive(true);
		}
        m_Player.transform.position = m_StartPos.transform.position;
	}

}
