using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// A class that holds information on the amount of rows saved
/// </summary>
[System.Serializable]
public class RowAmount
{
	public int rowAmount;									// How many rows 
}

public class LeaderboardIO : MonoBehaviour
{
	private static LeaderboardIO m_Instance;               // Current Private instance
	public static LeaderboardIO Instance                   // Current public instance
	{
		get { return m_Instance; }
	}

	/// <summary>
	/// Called on script loading
	/// </summary>
	private void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;
		CreateDirectory();
	}

	private void CreateDirectory()
	{
		if (!Directory.Exists(Application.streamingAssetsPath + "/LeaderBoard"))
		{
			Directory.CreateDirectory(Application.streamingAssetsPath + "/LeaderBoard");
		}
	}

	public void SaveLeaderBoard(LeaderBoard leaderBoard)
	{
		string path = Application.streamingAssetsPath + "/LeaderBoard/LeaderBoard.dat";
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, leaderBoard);
		stream.Close();
	}

	public LeaderBoard LoadLeaderBoard()
	{
		string path = Application.streamingAssetsPath + "/LeaderBoard/LeaderBoard.dat";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			LeaderBoard leaderBoard = formatter.Deserialize(stream) as LeaderBoard;

			Debug.Log("Loading LeaderBoard" + path);
			stream.Close();
			return leaderBoard;
		}

		else
		{
			Debug.Log("LeaderBoard doesn't exist at: " + path);
			return null;
		}
	}
}

