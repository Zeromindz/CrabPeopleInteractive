using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    [SerializeField] private float recordFrequency;         // The desired frequency 
    
    private float timer;                                    // Makes sure it records at the desired frequency
    private float timevalue;                                // The delta time between each path position
    private bool isRecording = false;                       // The recording state
    public bool recording = false;
    public List<GhostData> path = new List<GhostData>();   // The list path 
    private static GhostRecorder m_Instance;                // Current Private instance
    public static GhostRecorder Instance                    // Current public instance
    {
        get { return m_Instance; }
    }

    /// <summary>
    /// Called on the loading of the script
    /// </summary>
    private void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;

        // if true records more data
        //if (isRecording)
        //{
        //    ResetData();
        //    timevalue = 0;
        //    timer = 0;
        //}
    }

    /// <summary>
    /// Called once a frame
    /// Records the ghost if recording state it true
    /// </summary>
    private void Update()
    {
        if (isRecording)
		{
            timer += Time.unscaledDeltaTime;
            timevalue += Time.unscaledDeltaTime;
		}

        if (isRecording & timer >= 1 / recordFrequency)
        {
            GhostData pathing = new GhostData(timevalue, gameObject.transform.position, gameObject.transform.rotation);
    
            path.Add(pathing);

            timer = 0;
        }
    }

    /// <summary>
    /// Called when starting a new run
    /// Clears / resets the ghost path data
    /// </summary>
    public void ResetData()
    {
        path.Clear();
    }

    /// <summary>
    /// Called during the start or unpause of the run
    /// Changes the recording state to true
    /// </summary>
    public void StartRecording()
    {
        isRecording = true;
        recording = true;
    }

    /// <summary>
    /// Called during pause or completion of the track
    /// Changes the recording state to false
    /// </summary>
    public void StopRecording()
    {
        isRecording = false;

    }

    /// <summary>
    /// Clled when saving the ghostspath -> GetPath function is referenced when saving now
    /// </summary>
    public void SaveRecording()
    {
        isRecording = false;
        GhostSave.Instance.SaveGhost(path);
    }

    /// <summary>
    /// Called when the path is tobe saved
    /// Gets the path of 
    /// </summary>
    /// <returns>The recorded ghosts path</returns>
    public List<GhostData> GetPath()
	{
        return path;
	}

    /// <summary>
    /// Called to check the recording state
    /// </summary>
    /// <returns>A bool of the recording state</returns>
    public bool IsRecording()
    {
        return isRecording;
    }

    public void SetRecording(bool value)
	{
        recording = value;
	}
}
