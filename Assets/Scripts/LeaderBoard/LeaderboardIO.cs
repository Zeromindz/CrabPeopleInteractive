using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class RowAmount
{
	public int rowAmount;
}

public class LeaderboardIO : MonoBehaviour
{
	private static LeaderboardIO m_Instance;               // Current Private instance
	public static LeaderboardIO Instance                   // Current public instance
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
	}
	private void Start()
	{
		//SaveRowAmount(0);
	}

	private void SaveRowAmount(int amount)
	{
		string path = Application.dataPath + "/Leaderboard/RowAmount.dat";
		//if (!File.Exists(path)) { }
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		RowAmount rows = new RowAmount();
		rows.rowAmount = amount;

		formatter.Serialize(stream, rows);
		Debug.Log("Saving Row Amount: " + amount + " At: " + path );
		stream.Close();
	}

	private RowAmount LoadRowAmount()
	{
		string path = Application.dataPath + "/LeaderBoard/RowAmount.dat";
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
			return null;
		}
	}


	public void SaveLeaderBoardRow(string name, float score, List<GhostData> replayData)
	{
		RowAmount rows = LoadRowAmount();

		BinaryFormatter formatter = new BinaryFormatter();
		//string path = Application.persistentDataPath + "LeaderBoardRow" + rows.rowAmount + ".dat"; saves to users Personal files
	
		string path = Application.dataPath + "/LeaderBoard/LeaderBoardRow" + rows.rowAmount + ".dat";
		Debug.Log("" + path);
		FileStream stream = new FileStream(path, FileMode.Create);

		GhostData[] replayDataArray = replayData.ToArray();
		LeaderboardData save = new LeaderboardData(name, score, replayDataArray);
		formatter.Serialize(stream, save);
		stream.Close();
		SaveRowAmount(rows.rowAmount + 1);
		Debug.Log("Saving Row to " + path);
	}

	public LeaderboardData LoadLeaderBoardData(int index)
	{
		RowAmount rows = LoadRowAmount();
		
		if(index > rows.rowAmount - 1)
		{
			index = 0;
		}
		//string path = Application.persistentDataPath + "LeaderBoardRow" + rows.rowAmount + ".dat";
		string path = Application.dataPath + "/LeaderBoard/LeaderBoardRow" + index + ".dat";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			LeaderboardData save = formatter.Deserialize(stream) as LeaderboardData;

			return save;
		}
		else
		{
			Debug.Log("Save file not found in " + path);
			return null;
		}
	}


}
