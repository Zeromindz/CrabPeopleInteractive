using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// Worked on by:
//	Declan Doller
//
//

/// <summary>
/// The states of UI 
/// </summary>
public enum MenuState
{
	MAINMENU,
	GAME,
	GAMEPAUSED,
	SETTINGS,
	ENDSCREEN,
	LEADERBOARD,
}

/// <summary>
/// Pairs the canvas with a state, used in the stack
/// </summary>
public struct MenuStackItem
{
	public GameObject UI;
	public MenuState State;

	public MenuStackItem(GameObject Canvas, MenuState state)
	{
		UI = Canvas;
		State = state;
	}
}

/// <summary>
/// Controlling all the menus in the game
/// </summary>
public class MenuController : MonoBehaviour
{
	#region Variables/Properties
	// --- Public ---
	public MenuState m_State;										// Displays the current state of the menu

	[Header("UIs")]
	public GameObject m_MenuUI = null;								// The canvas holding the Main Menu UI
	public GameObject m_GameUI = null;								// The canvas holding the Game Ui
	public GameObject m_GamePausedUI = null;						// The canvas holding the Game Paused UI
	public GameObject m_EndScreenUI = null;							// The canvas holding the endscreen UI
	public GameObject m_SettingsUI = null;                          // The canvas holding The settings  UI
	public GameObject m_LeaderboardUI = null;						// The canvas holding the leaderboard UI
	public bool IsInGame 
	{ 
		get { 	return m_State == MenuState.GAME; } 
	}

	// -----Private-----
	Vector2 m_ScreenSize;
	private GameObject m_CurrentUI = null;                          // The current Ui that is being displayed
	private Stack<MenuStackItem> m_UIStack;                         // The stack holding information when travelling between UIs
	public bool IsGamePaused										// Displayed if the Game is currently paused
	{
		get { return m_State == MenuState.GAMEPAUSED; }
	}
	#endregion

	#region Unity Functions
	/// <summary>
	/// Called when the script is being loaded.
	/// Creates an instance and sets the UI
	/// </summary>
	void Awake()
	{
		m_CurrentUI = m_MenuUI;
		m_UIStack = new Stack<MenuStackItem>();
		
	}
	#endregion

	#region Functions
	/// <summary>
	/// Using the stack returns to the previous UI
	/// </summary>
	public void ReturnToPreviousUI()
	{
		if (m_UIStack.Count >= 2)
		{
			m_UIStack.Pop();
			OnExitPreviousState();
			MenuStackItem menu = m_UIStack.Peek();
			m_State = menu.State;
			UpdateState();
		}
	}

	/// <summary>
	///  Pauses the game and loads the Pause Menu UI
	/// </summary>
	public void PauseGame()
	{
		Debug.Log("Pausing Game");
		OnExitPreviousState();
		m_State = MenuState.GAMEPAUSED;
		m_UIStack.Push(new MenuStackItem(m_GamePausedUI, MenuState.GAMEPAUSED));
		Time.timeScale = 0;
		UpdateState();
	}

	/// <summary>
	/// Un pauses the game and loads the game UI
	/// </summary>
	public void UnPauseGame()
	{
		Debug.Log("Un-Pausing Game");
		OnExitPreviousState();
		m_State = MenuState.GAME;
		LoadGame();
	}

	/// <summary>
	/// Loads the main menu UI
	/// </summary>
	public void LoadMenu()
	{
		Debug.Log("Loading Menu");
		OnExitPreviousState();
		m_State = MenuState.MAINMENU;
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_MenuUI, MenuState.MAINMENU));
		PortalManager.m_Instance.SetState(PortalManager.PortalStates.VOID);
		SoundManager.Instance.PlayMusic(1);
		SoundManager.Instance.StartTerrainSounds();
		UIController.Instance.GameUI.SetActive(false);
		UIController.Instance.GameUI.ResetGameUI();
		GameManager.Instance.State = GameState.NotInRun;
		UpdateState();
	}

	/// <summary>
	/// Loads the game UI
	/// </summary>
	public void LoadGame()
	{
		Debug.Log("Loading: Game");
		PlayerMovement.Instance.m_MoveForward = true;
		PortalManager.m_Instance.SpawnPortalFromMenu();
		OnExitPreviousState();
		m_State = MenuState.GAME;
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_GameUI, MenuState.GAME));
		SoundManager.Instance.PlayMusic(0);
		UpdateState();
	}

	public void LoadGameFromMenu()
	{
		Debug.Log("Re-Loading: Game");
		m_State = MenuState.GAME;
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_GameUI, MenuState.GAME));
		UpdateState();
		ResetGame();
		UIController.Instance.GameUI.SetActive(false);
	}

	/// <summary>
	/// Loads the settings UI
	/// </summary>
	public void LoadSettings()
	{
		Debug.Log("Loading: Settings");
		OnExitPreviousState();
		m_State = MenuState.SETTINGS;
		m_UIStack.Push(new MenuStackItem(m_SettingsUI, MenuState.SETTINGS));
		//UIController.Instance.SettingsUI.LoadSettings();
		UpdateState();
	}


	/// <summary>
	/// Loads the leaderboard UI
	/// </summary>
	public void LoadEndScreen()
	{
		Debug.Log("Loading: EndScreen");
		OnExitPreviousState();
		m_State = MenuState.ENDSCREEN;
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_EndScreenUI, MenuState.ENDSCREEN));
		Time.timeScale = 0;
		UIController.Instance.EndScreenUI.Reset();
		UIController.Instance.EndScreenUI.SetScore();
		SoundManager.Instance.StopTerrainSounds();
		SoundManager.Instance.PlayMusic(1);
		GameManager.Instance.m_ChosenGhostIndices.Clear();
		UpdateState();
	}

	/// <summary>
	/// Loads the leaderboard UI
	/// </summary>
	public void LoadLearderboard()
	{
		Debug.Log("Loading: Leaderboard");
		OnExitPreviousState();
		UIController.Instance.LeaderboardUI.m_ChosenIndices.Clear();
		m_State = MenuState.LEADERBOARD;
		m_UIStack.Push(new MenuStackItem(m_LeaderboardUI, MenuState.LEADERBOARD));
		UIController.Instance.LeaderboardUI.Load();
		UpdateState();
	}


	/// <summary>
	/// Called when Exit to windows button is pressed
	/// Exis the application
	/// </summary>
	public void ExitGame()
	{
		Debug.Log("Closing App");
		Application.Quit();
	}

	/// <summary>
	///  Called whenever a state is changed.
	///  Updates the states 
	/// </summary>
	private void UpdateState()
	{
		m_CurrentUI.SetActive(false);

		if (m_State == MenuState.MAINMENU)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Time.timeScale = 1;
			m_MenuUI.SetActive(true);
			m_CurrentUI = m_MenuUI;
			GhostRecorder.Instance.ResetData();
			GameManager.Instance.SetMenu();
			GhostRecorder.Instance.ResetAndStopRecording();
			UIController.Instance.GameUI.SetActive(false);
		}

		if (m_State == MenuState.GAME)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			m_GameUI.SetActive(true);
			m_CurrentUI = m_GameUI;
			Time.timeScale = 1;
			if (GameManager.Instance.GetIsPlaying())
			{
				GameManager.Instance.PlayReplays();
				GhostRecorder.Instance.StartRecording();
			}

			
			//UIController.Instance.GameUI.ResetGameUI();
		}

		if (m_State == MenuState.GAMEPAUSED)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Time.timeScale = 0;
			m_GamePausedUI.SetActive(true);
			m_CurrentUI = m_GamePausedUI;
			if (GameManager.Instance.GetIsPlaying())
			{
				GameManager.Instance.StopReplays();
			}
			GhostRecorder.Instance.StopRecording();
		}

		if (m_State == MenuState.SETTINGS)
		{
			m_SettingsUI.SetActive(true);
			m_CurrentUI = m_SettingsUI;
		}

		if (m_State == MenuState.ENDSCREEN)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			m_EndScreenUI.SetActive(true);
			m_CurrentUI = m_EndScreenUI;
		}

		if (m_State == MenuState.LEADERBOARD)
		{
			m_LeaderboardUI.SetActive(true);
			m_CurrentUI = m_LeaderboardUI;
		}
	}

	private void OnExitPreviousState()
	{
		if(m_State == MenuState.SETTINGS)
		{
			//UIController.Instance.SettingsUI.SaveSettings();
		}

		else if(m_State == MenuState.GAMEPAUSED)
		{
			if (GhostRecorder.Instance.recording)
			{
				GhostRecorder.Instance.StartRecording();
			}
			GameManager.Instance.PlayReplays();
		}

		else if(m_State == MenuState.LEADERBOARD)
		{
			GameManager.Instance.m_ChosenGhostIndices = UIController.Instance.LeaderboardUI.m_ChosenIndices;
		}
	}
	#endregion

	public void ResetGame()
	{
		GhostRecorder.Instance.ResetAndStopRecording();
		UIController.Instance.GameUI.ResetGameUI();
		GameManager.Instance.ResetGame();
	}
	#region Extras
	/// <summary>
	/// Cause Jayden wanted to be apart
	/// </summary>
	public void JaydenWasHere(){ }
	#endregion
}
