using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Reference https://www.youtube.com/watch?v=c5G2jv7YCxM
/// <summary>
/// A class holding the data for and segment of the final replay
/// </summary>
[System.Serializable]
public class GhostData
{
	public float timeStamp;
	public float posX, posY, posZ;
	public float rotX, rotY, rotZ, rotW;

	public GhostData(float timeStamps, Vector3 pos, Quaternion rot)
	{
		timeStamp = timeStamps;
		posX = pos.x;
		posY = pos.y;
		posZ = pos.z;

		rotX = rot.x;
		rotY = rot.y;
		rotZ = rot.z;
		rotW = rot.w;
	}
}

/// <summary>
/// A script responsible for saving and loading the replay ghost-> used version in LeaderborardIO
/// </summary>
public class GhostSave : MonoBehaviour
{

	private static GhostSave m_Instance;               // Current Private instance
	public static GhostSave Instance                   // Current public instance
	{
		get { return m_Instance; }
	}

	/// <summary>
	/// Called on scrip loading
	/// </summary>
	private void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;
	}

	/// <summary>
	/// Called usually at the end of a run where the player saves their replay
	/// </summary>
	/// <param name="ghostData">A list of Ghost data that makes up the path of the replay ghost</param>
	public void SaveGhost(List<GhostData> ghostData)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/Replays.dat";
		FileStream stream = new FileStream(path, FileMode.Create);

		GhostData[] array = ghostData.ToArray();
		formatter.Serialize(stream, array);

		// Serializes it to file
		
		stream.Close();
	}

	/// <summary>
	/// Called when the replay ghost needs to be loaded
	/// Gets the last saved ghost path data from a file
	/// </summary>
	/// <returns>A list of Ghost data that makes upthe path of the replay ghost</returns>
	public List<GhostData> LoadGhost()
	{
		// Checks if the file exists then opens everything and returns the data as settingsdata class
		string path = Application.persistentDataPath + "/Replays.dat";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			GhostData[] data = formatter.Deserialize(stream) as GhostData[];
			stream.Close();

			List<GhostData> ghost = new List<GhostData>();

			for(int i = 0; i < data.Length; i++)
			{
				ghost.Add(data[i]);
			}

			return ghost;
		}

		// Logs an error
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}
