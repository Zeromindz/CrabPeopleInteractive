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

	/// <summary>
	/// Called when a new save is made
	/// Updaets the file to the new value
	/// </summary>
	/// <param name="amount">The amount of Leaderboard rows</param>
	private void SaveRowAmount(int amount)
	{
		string path = Application.streamingAssetsPath + "/LeaderBoard/RowAmount.dat";
		//if (!File.Exists(path)) { }
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		RowAmount rows = new RowAmount();
		rows.rowAmount = amount;

		formatter.Serialize(stream, rows);
		Debug.Log("Saving Row Amount: " + amount + " At: " + path);
		stream.Close();
	}

	/// <summary>
	/// Called when saving the row amount
	/// Loads a row amount
	/// </summary>
	/// <returns>The class holding an int of row amounts</returns>
	public RowAmount LoadRowAmount()
	{
		string path = Application.streamingAssetsPath + "/LeaderBoard/RowAmount.dat";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			RowAmount rowAmount = formatter.Deserialize(stream) as RowAmount;

			Debug.Log("Loading Row amount: " + rowAmount.rowAmount);
			stream.Close();
			return rowAmount;
		}
		else
		{
			Debug.Log("Row Amount save file not found! creating file...");
			SaveRowAmount(0);
			LoadRowAmount();
			RowAmount rows = new RowAmount();
			rows.rowAmount = 0;
			return rows;
		}
	}

	/// <summary>
	/// Called when saving to the leaderboard
	/// Saves a new row to the leaderboard
	/// </summary>
	/// <param name="name">The players name</param>
	/// <param name="score">The players score</param>
	/// <param name="replayData">The The path of the ghost replay</param>
	public void SaveLeaderBoardRow(string name, float score, List<GhostData> replayData)
	{
		RowAmount rows = LoadRowAmount();

		// This limits the amount of files that can be created
		//if (rows.rowAmount == 5)
		//{
		//	rows.rowAmount = 0;
		//}

		BinaryFormatter formatter = new BinaryFormatter();
		//string path = Application.persistentDataPath + "LeaderBoardRow" + rows.rowAmount + ".dat"; saves to users Personal files

		string path = Application.streamingAssetsPath + "/LeaderBoard/LeaderBoardRow" + rows.rowAmount + ".dat";
		Debug.Log("" + path);
		FileStream stream = new FileStream(path, FileMode.Create);

		GhostData[] replayDataArray = replayData.ToArray();
		//LeaderboardData save = new LeaderboardData(name, score, replayDataArray.);
	//	formatter.Serialize(stream, save);
		stream.Close();
		SaveRowAmount(rows.rowAmount + 1);
		Debug.Log("Saving Row to " + path);
	}

	/// <summary>
	/// Called when loading the leaderboard data
	/// Loads a leaderboard row from a file
	/// </summary>
	/// <param name="index">The row index to load</param>
	/// <returns></returns>
	public LeaderboardData LoadLeaderBoardData(int index)
	{
		RowAmount rows = LoadRowAmount();

		if (index > rows.rowAmount - 1)
		{
			index = 0;
		}
		//string path = Application.persistentDataPath + "LeaderBoardRow" + rows.rowAmount + ".dat";
		string path = Application.streamingAssetsPath + "/LeaderBoard/LeaderBoardRow" + index + ".dat";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			LeaderboardData save = formatter.Deserialize(stream) as LeaderboardData;

			stream.Close();
			return save;
		}

		else
		{
			Debug.Log("Save file not found in " + path);
			return null;
		}
	}

	/// <summary>
	/// Called when the ClearLeaxderboard funtion is run
	/// Sets the Row amount to 0
	/// </summary>
	public void ClearRows()
	{
		SaveRowAmount(0);
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

