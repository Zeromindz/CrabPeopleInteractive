using System.Collections;
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

public enum FloatingObj
{
    Crown,
    Duck
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
    public Transform m_MenuPos;
    public Transform m_StartPos;
    public GameObject m_Player;
    public OpenGate m_GatesOfHell = null;
    [SerializeField] private Transform m_Skybox = null;

    [SerializeField] GameObject m_ReplayGhostPrefab = null;
    public List<int> m_ChosenGhostIndices;
    public List<GameObject> m_ReplayGhosts = null;
    [SerializeField] private GameObject m_CrownPrefab;
    [SerializeField] private GameObject m_DuckPrefab;

    private List<GameObject> m_FloatingObj = null;
    private List<GameObject> m_ReplayWithFloatingObj = null;

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
        m_FloatingObj = new List<GameObject>();
        m_ReplayWithFloatingObj = new List<GameObject>();
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
        for (int i = 0; i < m_FloatingObj.Count; i++)
		{
            Vector3 pos;
            pos = m_ReplayGhosts[i].transform.position;
            pos.y += 10;

            m_FloatingObj[i].transform.position = pos;
		}

        Vector3 newPos = m_Player.transform.position;
        newPos.y -= 600;
        m_Skybox.position = newPos;
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
        m_Player.transform.rotation = Quaternion.Euler(Vector3.zero);
        m_Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_Camera.transform.position = m_StartPos.position;
        m_Camera.transform.rotation = m_StartPos.rotation;
        PlayerMovement.Instance.ResetMovement();
        PlayerController.Instance.passengerManager.ResetPassengers();

        m_UIController.GameUI.StopAndResetTime();
        m_UIController.GameUI.ResetGameUI();
        m_FloatingObj.Clear();
        m_UIController.MenuController.ReturnToPreviousUI();
        SoundManager.Instance.PlayMusic(0);
        PortalManager.m_Instance.SetState(PortalManager.PortalStates.ENDSPAWNED);
        m_GatesOfHell.Reset();
	}
   
    public void SetMenu()
	{
        for (int i = 0; i < m_GhostPickups.Count; i++)
        {
            m_GhostPickups[i].SetActive(true);
        }

        m_Player.transform.position = m_MenuPos.position;
        m_Player.transform.rotation = m_MenuPos.rotation;
        m_Camera.transform.position = m_MenuPos.position;
        m_Camera.transform.rotation = m_MenuPos.rotation;
        PlayerMovement.Instance.ResetMovement();
        PlayerController.Instance.passengerManager.ResetPassengers();

        m_UIController.GameUI.ResetTime();
        m_FloatingObj.Clear();
        m_ReplayWithFloatingObj.Clear();
        DestroyGhost();
    }

    public bool InstantiateReplays()
 	{
        bool loaded = true; 

        for(int i = 0; i < m_ChosenGhostIndices.Count; i++)
		{
            GameObject obj = Instantiate(m_ReplayGhostPrefab, m_Player.transform.position, m_Player.transform.rotation) as GameObject;
            loaded = obj.GetComponent<GhostPlayer>().LoadGhost(m_ChosenGhostIndices[i]);

			if (!loaded)
			{
                return loaded;
			}

			else
			{
				m_ReplayGhosts.Add(obj);
                //MiniMap.Instance.AddGhost(obj.transform);
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
        //MiniMap.Instance.RemoveGhosts();
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

    public void AddFloatingObj(FloatingObj obj, GameObject replayObject)
	{
        GameObject gameObject;
        switch (obj)
		{
            case FloatingObj.Crown:

                gameObject = Instantiate(m_CrownPrefab);
                m_FloatingObj.Add(gameObject);
                m_ReplayWithFloatingObj.Add(replayObject);
                break;

            case FloatingObj.Duck:

                gameObject = Instantiate(m_DuckPrefab);
                m_FloatingObj.Add(gameObject);
                m_ReplayWithFloatingObj.Add(replayObject);
                break;
        }
	}
}
