using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
	public string playerName;
	public int playerScore;
	public GhostData[] replayPath;

	public LeaderboardData(string name, int score, GhostData[] path)
	{
		playerName = name;
		playerScore = score;
		replayPath = path;
	}
}
