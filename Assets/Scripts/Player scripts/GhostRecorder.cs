using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    private float timer;
    private float timevalue;
    private bool isRecording = false;
    public float recordFrequency;
    public List<GhostData> path = new List<GhostData>();
    
    private static GhostRecorder m_Instance;               // Current Private instance
    public static GhostRecorder Instance                   // Current public instance
    {
        get { return m_Instance; }
    }
    private void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;

        if (isRecording)
        {
            ResetData();
            timevalue = 0;
            timer = 0;
        }
    }

    public void ResetData()
    {
        path.Clear();
    }

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

    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void SaveRecording()
    {
        isRecording = false;
        GhostSave.Instance.SaveGhost(path);
    }

    public bool IsRecording()
    {
        return isRecording;
    }
}
