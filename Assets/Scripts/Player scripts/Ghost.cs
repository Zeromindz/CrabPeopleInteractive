using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference https://www.youtube.com/watch?v=c5G2jv7YCxM
public class GhostData
{
   public float timeStamp;
   public Vector3 position;
   public Vector3 rotation;
}

[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    public bool isRecording;
    public bool isReplaying;
    public float recordFrequency;

    public List<GhostData> path = new List<GhostData>();

    public void ResetData()
	{
        path.Clear();
	}
}

public class GhostRecorder : MonoBehaviour
{
    private Ghost ghost;
    private float timer;
    private float timevalue;

	private void Awake()
	{
		if (ghost.isRecording)
		{
            ghost.ResetData();
            timevalue = 0;
            timer = 0;
		}
	}

	private void Update()
	{
        timer += Time.unscaledDeltaTime;
        timevalue += Time.unscaledDeltaTime;

        if(ghost.isRecording & timer >= 1/ghost.recordFrequency)
		{
            GhostData pathing = new GhostData();
            pathing.timeStamp = timevalue;
            pathing.position = gameObject.transform.position;
            pathing.rotation = gameObject.transform.eulerAngles;
            ghost.path.Add(pathing);

            timer = 0;
		}
	}
}

public class GhostPLayer : MonoBehaviour
{
    public Ghost ghost;
    private float timeValue;
    private int index1;
    private int index2;

	private void Awake()
	{
        timeValue = 0;
	}
	private void Update()
	{
        timeValue = Time.unscaledDeltaTime;
		if (ghost.isReplaying)
		{
            GetIndex();
            SetTransform();
		}
	}

    private void GetIndex()
	{
        for(int i = 0; i < ghost.path.Count - 2; i++)
		{

            if(ghost.path[i].timeStamp == timeValue)
			{
                index1 = i;
                index2 = i;
			}
            else if(ghost.path[i].timeStamp < timeValue & timeValue < ghost.path[i + 1].timeStamp)
			{
                index1 = i;
                index2 = i + 1;
			}
		}

        index1 = ghost.path.Count - 1;
        index2 = ghost.path.Count - 1;
	}

    private void SetTransform()
	{
        float interpolationFactor = (timeValue - ghost.path[index1].timeStamp) / (ghost.path[index2].timeStamp - ghost.path[index1].timeStamp);
        if(index1 == index2)
		{
            gameObject.transform.position = Vector3.Lerp(ghost.path[index1].position, ghost.path[index2].position, interpolationFactor);
            gameObject.transform.eulerAngles = Vector3.Lerp(ghost.path[index1].rotation, ghost.path[index2].rotation, interpolationFactor);
		}
	}
}