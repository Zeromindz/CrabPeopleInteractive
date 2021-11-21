using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class holding the information for needed 
/// </summary>
[System.Serializable]
public class LeaderboardData
{
	public string playerName;
	public float playerScore;
	public GhostData[] replayPath;

	/// <summary>
	/// Custom constructor
	/// </summary>
	/// <param name="name">The Name of the player</param>
	/// <param name="score">The score of the player</param>
	/// <param name="path">The path</param>
	public LeaderboardData(string name, float score, GhostData[] path)
	{
		playerName = name;
		playerScore = score;
		replayPath = path;
	}
}
