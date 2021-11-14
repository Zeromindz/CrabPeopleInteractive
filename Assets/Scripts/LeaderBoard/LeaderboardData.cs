using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
	public string playerName;
	public float playerScore;
	public GhostData[] replayPath;

	public LeaderboardData(string name, float score, GhostData[] path)
	{
		playerName = name;
		playerScore = score;
		replayPath = path;
	}
}
