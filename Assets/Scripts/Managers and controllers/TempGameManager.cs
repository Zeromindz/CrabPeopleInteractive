using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempGameManager : MonoBehaviour
{
    public GameObject m_Player;
    private Transform m_StartPoint;
    private Transform m_EndPoint;
    private KeybindManager m_Keybinds;
    private static TempGameManager m_Instance;                       // The current instance of MenuController
    public static TempGameManager Instance                           // The public current instance of MenuController
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
       // m_StartPoint = GameObject.FindGameObjectWithTag("Start").transform;
       // m_StartPoint = GameObject.FindGameObjectWithTag("End").transform;
        m_Keybinds = KeybindManager.Instance;
	}

    // Start is called before the first frame update
    void Start()
    {
        MenuController.Instance.LoadMenu();
    }

    // Update is called once per frame
    void Update()
    {

  //      // if it's in the game state
  //      if(MenuController.Instance.m_State == MenuState.GAME)
		//{

  //          if (Input.GetKeyDown(m_Keybinds.Forward))
  //          {
  //              Debug.Log("Pressed: Forward");
  //          }

  //          if (Input.GetKeyDown(m_Keybinds.TurnLeft))
  //          {
  //              Debug.Log("Pressed: TurnLeft");
  //          }

  //          if (Input.GetKeyDown(m_Keybinds.TurnRight))
  //          {
  //              Debug.Log("Pressed: TurnRight");
  //          }

  //          if (Input.GetKeyDown(m_Keybinds.BackwardOrStop))
  //          {
  //              Debug.Log("Pressed: BackwardOrStop");
  //          }

  //          if (Input.GetKeyDown(KeyCode.R))
		//	{
  //              if(!GhostRecorder.Instance.IsRecording())
		//		{
  //                  Debug.Log("Recording started");
  //                  GhostRecorder.Instance.StartRecording();
		//		}
		//		else
		//		{
  //                  GhostRecorder.Instance.SaveRecording();
  //                  Debug.Log("Recording stopped");
		//		}
		//	}

  //          if (Input.GetKeyDown(KeyCode.P))
		//	{
  //              GhostPlayer.Instance.LoadGhost();
  //              GhostPlayer.Instance.Play();
  //              CameraController.Instance.WatchGhost();
  //              Debug.Log("Playing recording");
		//	}

		//	if (Input.GetKeyDown(KeyCode.Space))
  //          {
  //              MenuController.Instance.LoadEndScreen();
		//	}
  //      }
    }

    public void RestartGame()
	{
        m_Player.transform.position = m_StartPoint.position;
        m_Player.transform.rotation = m_StartPoint.rotation;
	}
}
