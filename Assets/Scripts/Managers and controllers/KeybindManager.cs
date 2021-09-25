using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.UI;

// reference https://www.studica.com/blog/custom-input-manager-unity-tutorial

[System.Serializable]
public class KeybindData
{
	public KeyCode forward;
	public KeyCode turnLeft;
	public KeyCode turnRight;
	public KeyCode backwardOrStop;
	public KeybindData(KeyCode forwardKey, KeyCode turnLeftKey, KeyCode turnRightKey, KeyCode backwardOrStopKey)
	{
		forward = forwardKey;
		turnLeft = turnLeftKey;
		turnRight = turnRightKey;
		backwardOrStop = backwardOrStopKey;
	}
}

public static class SaveKeybind
{
	public static void SaveKeybinds(KeyCode forwardKey, KeyCode turnLeftKey, KeyCode turnRightKey, KeyCode backwardOrStopKey)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/CustomKeybinds.dat";
		FileStream stream = new FileStream(path, FileMode.Create);
		KeybindData data = new KeybindData(forwardKey, turnLeftKey, turnRightKey, backwardOrStopKey);

		// Serializes it to file
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static KeybindData LoadKeybinds(LoadType type)
	{
		// Checks if the file exists then opens everything and returns the data as settingsdata class
		string path = Application.persistentDataPath + "/DefaultKeyBinds.dat";
		if (File.Exists(path))
		{
			if(type == LoadType.Default)
			{
				path = Application.persistentDataPath + "/DefaultKeyBinds.dat";
			}
			else
			{
				path = Application.persistentDataPath + "/CustomKeybinds.dat";
			}

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			KeybindData data = formatter.Deserialize(stream) as KeybindData;
			stream.Close();
			return data;
		}

		// Logs an error
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}

public enum LoadType
{
	Default,
	Custom
}

// safe to file, the default keys and save\to file the custom keys
public class KeybindManager : MonoBehaviour
{

	[Header("Default keybinds")]

	public KeyCode Forward;
	public KeyCode TurnLeft;
	public KeyCode TurnRight;
	public KeyCode BackwardOrStop;

	[Header("Labels")]
	public GameObject[] m_Actions = new GameObject[4];
	public GameObject[] m_Keybind = new GameObject[4];

	private bool m_LookingForKey = false;
	private int m_KeybindIndex;

	private static KeybindManager m_Instance;
	public static KeybindManager Instance
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

	// Start is called before the first frame update
	void Start()
    {
		string path = Application.persistentDataPath + "/CustomKeybinds.dat";
		if (File.Exists(path))
		{
			KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Custom);
			UpdateKeybinds(data);
			Debug.Log("Using Custom Keybind");
		}
		else
		{
			KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Default);
			UpdateKeybinds(data);
			Debug.Log("No custom keybind found");
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (m_LookingForKey)
		{
			foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKey(vkey))
				{
					if (vkey != KeyCode.Return && vkey != KeyCode.Backspace && vkey != KeyCode.Escape)
					{
						m_LookingForKey = false;
						Changingkeybind(vkey);
					}
				}
			}
		}
    }

    /// <summary>
    /// Called when the "Reset to default" button is pressed
    /// Resets all the current keybinds to the default ones set
    /// </summary>
	public void ResetKeybinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Default);
		UpdateKeybinds(data);
		SetKeybinds();
	}

	// Sets the keybinds
	public void SetKeybinds()
	{
		SaveKeybind.SaveKeybinds(Forward, TurnLeft, TurnRight, BackwardOrStop);
	}

	private void UpdateKeybinds(KeybindData data)
	{
		m_Actions[0].GetComponent<TextMeshProUGUI>().text = "Forward";
		m_Actions[1].GetComponent<TextMeshProUGUI>().text = "Turn Left";
		m_Actions[2].GetComponent<TextMeshProUGUI>().text = "Turn Right";
		m_Actions[3].GetComponent<TextMeshProUGUI>().text = "Backwards / Stop";

		m_Keybind[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.forward;
		m_Keybind[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.turnLeft;
		m_Keybind[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.turnRight;
		m_Keybind[3].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.backwardOrStop;

		Forward = data.forward;
		TurnLeft = data.turnLeft;
		TurnRight = data.turnRight;
		BackwardOrStop = data.backwardOrStop;
	}

	public void ChangeKeyBind(int index)
	{
		m_LookingForKey = true;
		m_KeybindIndex = index;
	} 

	private void Changingkeybind(KeyCode key)
	{
		
		switch (m_KeybindIndex)
		{
			case 0:
				Forward = key;
				break;

			case 1:
				TurnLeft = key;
				break;

			case 2:
				TurnRight = key;
				break;

			case 3:
				BackwardOrStop = key;
				break;
		}

		UpdateText();
	}

	public void UpdateText()
	{
		m_Keybind[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + Forward;
		m_Keybind[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + TurnLeft;
		m_Keybind[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + TurnRight;
		m_Keybind[3].GetComponentInChildren<TextMeshProUGUI>().text = "" + BackwardOrStop;
	} 

	public void LoadCustomBinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Custom);
		UpdateKeybinds(data);

	}

	public void LoadDefaultBinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Default);
		UpdateKeybinds(data);
	}
}
