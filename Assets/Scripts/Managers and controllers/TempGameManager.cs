using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempGameManager : MonoBehaviour
{
    public GameObject m_Player;
    public List<GameObject> m_CheckpointObjects;
    private KeybindManager m_Keybinds;
    private Stack<GameObject> m_checkPoints;
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

        // m_Keybinds = KeybindManager.Instance;
        m_checkPoints = new Stack<GameObject>();
        m_CheckpointObjects.Reverse();

        for(int i = 0; i < m_CheckpointObjects.Count; i++)
		{
            m_checkPoints.Push(m_CheckpointObjects[i]);
		}

    }

    // Start is called before the first frame update
    void Start()
    {
      //  MenuController.Instance.LoadMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RestartGame()
	{
        //m_Player.transform.position = m_StartPoint.position;
        //m_Player.transform.rotation = m_StartPoint.rotation;
	}
    public void StartGame()
    {
        Debug.Log("Start point hit");
        SpawnNextCheckpoint();
        GhostRecorder.Instance.StartRecording();
    }

    private void SpawnNextCheckpoint()
    {
        GameObject nextLocation = m_checkPoints.Pop();
        nextLocation.SetActive(true);
    }

    public void EndGame()
    {
        Debug.Log("End point hit");
        GhostRecorder.Instance.SaveRecording();
    }

    public void CheckPointHit()
	{
        Debug.Log("CheckPoint hit");
        SpawnNextCheckpoint();
	}

}
