using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GhostPlayer : MonoBehaviour
{
    [Header("Current Ghost")]
    public string name = "";
    public float score = 0;

    private bool isPlaying = false;
    private float timeValue;
    private int index1;
    private int index2;
    public bool isReplaying;
    public List<GhostData> path = new List<GhostData>();
    [SerializeField] TMP_Text m_Nametag = null;

    public int currentMove = 0;

    private static GhostPlayer m_Instance;               // Current Private instance
    public static GhostPlayer Instance                   // Current public instance
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
        timeValue = 0;
    }

    private void Update()
    {
        if (isReplaying)
        {
            timeValue += Time.unscaledDeltaTime;
            GetIndex();
            SetTransform();
        }
    }

    
    public bool LoadGhost(int index)
    {
        LeaderboardData save = LeaderboardIO.Instance.LoadLeaderBoardData(index);
        if(save != null)
		{
            List<GhostData> ghost = new List<GhostData>();

            for (int i = 0; i < save.replayPath.Length; i++)
            {
                ghost.Add(save.replayPath[i]);
            }
            path = ghost;
            m_Nametag.text = save.playerName;
            name = save.playerName;
            score = save.playerScore;
            return true;
		}
        return false;
        //old loading and saving
       //path = GhostSave.Instance.LoadGhost();
        
    }

    private void GetIndex()
    {
        for (int i = 0; i < path.Count - 2; i++)
        {

            if (path[i].timeStamp == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }

            else if (path[i].timeStamp < timeValue && timeValue < path[i + 1].timeStamp)
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = path.Count - 1;
        index2 = path.Count - 1;
    }

    private void SetTransform()
    {
        Vector3 pos1, pos2;
        Quaternion rot1, rot2;
        pos1.x = path[index1].posX;
        pos1.y = path[index1].posY;
        pos1.z = path[index1].posZ;
        pos2.x = path[index2].posX;
        pos2.y = path[index2].posY;
        pos2.z = path[index2].posZ;

        rot1.x = path[index1].rotX;
        rot1.y = path[index1].rotY;
        rot1.z = path[index1].rotZ;
        rot1.w = path[index1].rotW;
        rot2.x = path[index2].rotX;
        rot2.y = path[index2].rotY;
        rot2.z = path[index2].rotZ;
        rot2.w = path[index2].rotW;

        if (index1 == index2)
        {
            gameObject.transform.position = pos1;
            gameObject.transform.rotation = rot1;
        }

		else
		{
            float interpolationFactor = (timeValue - path[index1].timeStamp) / (path[index2].timeStamp - path[index1].timeStamp);   
            gameObject.transform.position = Vector3.Lerp(pos1, pos2, interpolationFactor);
            gameObject.transform.rotation = Quaternion.Lerp(rot1, rot2, interpolationFactor);
		}
        currentMove = index1;
    }

    /// <summary>
    /// Called when the the run starts
    /// Starts playing the recording
    /// </summary>
    public void Play()
    {
        isReplaying = true;
        isPlaying = true;
    }

    /// <summary>
    /// Called to reset and play the ghostplayer
    /// Resets tothe starting position and starts playing
    /// </summary>
    public void ResetAndPlay()
    {
        index1 = 0;
        index2 = 0;
        timeValue = 0;
        isReplaying = true;
        isPlaying = true;
    }

    /// <summary>
    /// Called when paused or finished run
    /// Stops the ghost from playing more of it's path
    /// </summary>
    public void Stop()
    {
        isReplaying = false;
    }

    /// <summary>
    /// Called tpo check the state of the Ghost player
    /// </summary>
    /// <returns>Thestate of the ghost player playing</returns>
    public bool IsPlaying() 
    {
        return isPlaying;
    }

    /// <summary>
    /// Called to manually change the IsPlaying state
    /// </summary>
    /// <param name="value">True to play, False to not play</param>
    public void SetIsPlaying(bool value)
	{
        isPlaying = value;
	}
}
