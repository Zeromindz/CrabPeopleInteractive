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
	public LeaderboardData(string name, float score, List<GhostData> path)
	{
		playerName = name;
		playerScore = score;
		replayPath = path.ToArray();
	}
}

[System.Serializable]
public class LeaderBoard
{
	List<LeaderboardData> datas = null;

	public LeaderBoard (List<LeaderboardData> dataList)
	{
		datas = dataList;
	}

	public void Add(LeaderboardData data)
	{
		if(datas == null)
		{
			datas = new List<LeaderboardData>();
		}

		datas.Add(data);
		Sort();
	}

	public void RemoveAt(int index)
	{
		datas.RemoveAt(index);
	}

	public void Sort()
	{
		LeaderboardData temp = null;

		for(int j = 0; j <= datas.Count - 2; j++) 
		{
			for (int i = 0; i <= datas.Count - 2; i++)
			{
				if (datas[i].playerScore > datas[i + 1].playerScore)
				{
					temp = datas[i + 1];
					datas[i + 1] = datas[i];
					datas[i] = temp;
				}
			}
		}
		Debug.Log("Sorted save");
	}
}