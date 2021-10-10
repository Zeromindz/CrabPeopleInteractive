using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameManager : MonoBehaviour
{
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

            if (Input.GetKeyDown(KeybindManager.Instance.Forward))
            {
                Debug.Log("Pressed: Forward");
            }

            if (Input.GetKeyDown(KeybindManager.Instance.TurnLeft))
            {
                Debug.Log("Pressed: TurnLeft");
            }

            if (Input.GetKeyDown(KeybindManager.Instance.TurnRight))
            {
                Debug.Log("Pressed: TurnRight");
            }

            if (Input.GetKeyDown(KeybindManager.Instance.BackwardOrStop))
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
