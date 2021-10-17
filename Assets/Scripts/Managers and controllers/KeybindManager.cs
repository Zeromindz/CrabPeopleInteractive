using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// A class that holds all the data needed for keybinds
/// </summary>
[System.Serializable]
public class KeybindData
{	
	public KeyCode forward;			// The keycode for going forward
	public KeyCode turnLeft;		// The keycode for turning left
	public KeyCode turnRight;		// The keycode for turning right
	public KeyCode backwardOrStop;	// The keycode for going backwards or stopping

	/// <summary>
	/// A constructor holding all the keybinds
	/// </summary>
	/// <param name="forwardKey">The keycode for going forward</param>
	/// <param name="turnLeftKey">The keycode for turning left</param>
	/// <param name="turnRightKey">The keycode for turning right</param>
	/// <param name="backwardOrStopKey">The keycode for going backwards or stopping</param>
	public KeybindData(KeyCode forwardKey, KeyCode turnLeftKey, KeyCode turnRightKey, KeyCode backwardOrStopKey)
	{
		forward = forwardKey;
		turnLeft = turnLeftKey;
		turnRight = turnRightKey;
		backwardOrStop = backwardOrStopKey;
	}
}

/// <summary>
/// A class used for saving the keybinds to a file
/// </summary>
public static class SaveKeybind
{
	/// <summary>
	/// Saves the new keybinds to file
	/// </summary>
	/// <param name="forwardKey">The keycode for going forward</param>
	/// <param name="turnLeftKey">The keycode for turning left</param>
	/// <param name="turnRightKey">The keycode for turning right</param>
	/// <param name="backwardOrStopKey">The keycode for going backwards or stopping</param>
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

	/// <summary>
	/// Loads the keybinds depending on the load type
	/// </summary>
	/// <param name="type">The type of keybinds wanted to be loaded</param>
	/// <returns></returns>
	public static KeybindData LoadKeybinds(LoadType type)
	{
		// Checks if the file exists then opens everything and returns the data as settingsdata class
		string path = Application.persistentDataPath + "/DefaultKeyBinds.dat";
		if (File.Exists(path))
		{
			//	Changes the file to be loaded from depending on what type is specified
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

/// <summary>
/// A Enum for all the load types
/// </summary>
public enum LoadType
{
	Default,
	Custom
}

// safe to file, the default keys and save\to file the custom keys
public class KeybindManager : MonoBehaviour
{
	#region Variables/Properties
	// --- Public ---
	[Header("Default keybinds")]
	public KeyCode Forward;									// Keycode for forward direction
	public KeyCode TurnLeft;								// Keycode for turning left
	public KeyCode TurnRight;								// keycode for turning right
	public KeyCode BackwardOrStop;							// Keycoded for going backwards or stopping

	[Header("Labels")]
	public GameObject[] m_Actions = new GameObject[4];		// The text for the current actions possible
	public GameObject[] m_Keybind = new GameObject[4];		// The button holding the text for the current keycodes

	// --- Private ---
	private bool m_LookingForKey = false;					// If the game should be looking for key presses
	private int m_KeybindIndex;                             // The index of the action the new keybind is effecting
	#endregion

	#region Singleton
	// Singleton instance
	private static KeybindManager m_Instance;				// Current Private instance
	public static KeybindManager Instance					// Current public instance
	{
		get { return m_Instance; }
	}
	#endregion

	#region Unity Functions
	/// <summary>
	/// Called when scriptis loaded
	/// Initializes instance
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
	/// Called at the start of the first frame.
	/// Loads the custom keybinds if they exist
	/// </summary>
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

		m_Actions[0].GetComponent<TextMeshProUGUI>().text = "Forward";
		m_Actions[1].GetComponent<TextMeshProUGUI>().text = "Turn Left";
		m_Actions[2].GetComponent<TextMeshProUGUI>().text = "Turn Right";
		m_Actions[3].GetComponent<TextMeshProUGUI>().text = "Backwards / Stop";

	}

	/// <summary>
	/// Called once per frame.
	/// Used to pick up key presses to change keybinds
	/// </summary>
    void Update()
    {
		//if (m_LookingForKey)
		//{ 
		//	foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
		//	{
		//		if (Input.GetKey(vkey))
		//		{
		//			if (vkey != KeyCode.Return && vkey != KeyCode.Backspace && vkey != KeyCode.Escape)
		//			{
		//				m_LookingForKey = false;
		//				Changingkeybind(vkey);
		//			}
		//		}
		//	}
		//}
    }
	#endregion

	#region Functions
	/// <summary>
	/// Called when the "Reset to default" button is pressed.
	/// Resets all the current keybinds to the default ones set
	/// </summary>
	public void ResetKeybinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Default);
		UpdateKeybinds(data);
		SetKeybinds();
	}

	/// <summary>
	/// Called when "set keybind" button is pressed.
	/// Saves the current keybinds to file
	/// </summary>
	public void SetKeybinds()
	{
		SaveKeybind.SaveKeybinds(Forward, TurnLeft, TurnRight, BackwardOrStop);
	}

	/// <summary>
	/// Called when keybinds are being set.
	/// Updates the keybind text to loaded keybinds
	/// </summary>
	/// <param name="data">The saved keybind data</param>
	private void UpdateKeybinds(KeybindData data)
	{
		m_Keybind[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.forward;
		m_Keybind[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.turnLeft;
		m_Keybind[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.turnRight;
		m_Keybind[3].GetComponentInChildren<TextMeshProUGUI>().text = "" + data.backwardOrStop;

		Forward = data.forward;
		TurnLeft = data.turnLeft;
		TurnRight = data.turnRight;
		BackwardOrStop = data.backwardOrStop;
	}

	/// <summary>
	/// Called when button is pressed to change keybind.
	/// Starts looking for the next key press
	/// </summary>
	/// <param name="index">The index of the action that the keybind will be set to</param>
	public void ChangeKeyBind(int index)
	{
		m_LookingForKey = true;
		m_KeybindIndex = index;
	} 

	/// <summary>
	/// Called after a new keybind is pressed.
	/// Changes the keybind
	/// </summary>
	/// <param name="key">The new keycode that will be used </param>
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

	/// <summary>
	/// Called when a keybind is changed.
	/// Updates the text that shows the current keybind
	/// </summary>
	public void UpdateText()
	{
		m_Keybind[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + Forward;
		m_Keybind[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + TurnLeft;
		m_Keybind[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + TurnRight;
		m_Keybind[3].GetComponentInChildren<TextMeshProUGUI>().text = "" + BackwardOrStop;
	} 

	/// <summary>
	/// Called when the game starts.
	/// Loads the custom keybinds 
	/// </summary>
	public void LoadCustomBinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Custom);
		UpdateKeybinds(data);

	}

	/// <summary>
	/// Called when "reset to default" button is pressed.
	/// Loads the default keybinds
	/// </summary>
	public void LoadDefaultBinds()
	{
		KeybindData data = SaveKeybind.LoadKeybinds(LoadType.Default);
		UpdateKeybinds(data);
	}
	#endregion
}
