using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    private KeybindManager m_Keybinds;

    void Awake()
	{
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

        // if it's in the game state
        if(MenuController.Instance.m_State == MenuState.GAME)
		{

            if (Input.GetKeyDown(m_Keybinds.Forward))
            {
                Debug.Log("Pressed: Forward");
            }

            if (Input.GetKeyDown(m_Keybinds.TurnLeft))
            {
                Debug.Log("Pressed: TurnLeft");
            }

            if (Input.GetKeyDown(m_Keybinds.TurnRight))
            {
                Debug.Log("Pressed: TurnRight");
            }

            if (Input.GetKeyDown(m_Keybinds.BackwardOrStop))
            {
                Debug.Log("Pressed: BackwardOrStop");
            }

            if (Input.GetKeyDown(KeyCode.R))
			{
                if(!GhostRecorder.Instance.IsRecording())
				{
                    Debug.Log("Recording started");
                    GhostRecorder.Instance.StartRecording();
				}
				else
				{
                    GhostRecorder.Instance.SaveRecording();
                    Debug.Log("Recording stopped");
				}
			}

            if (Input.GetKeyDown(KeyCode.P))
			{
                GhostPlayer.Instance.LoadGhost();
                GhostPlayer.Instance.Play();
                CameraController.Instance.WatchGhost();
                Debug.Log("Playing recording");
			}

			if (Input.GetKeyDown(KeyCode.Space))
            {
                MenuController.Instance.LoadEndScreen();
			}
        }
    }
}
