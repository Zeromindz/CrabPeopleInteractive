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
	private bool IsGamePaused										// Displayed if the Game is currently paused
	{
		get { return m_State == MenuState.GAMEPAUSED; }
	}
	#endregion

	#region Singleton instances
	// Singleton instance
	private static MenuController m_Instance;						// The current instance of MenuController
	public static MenuController Instance							// The public current instance of MenuController
	{
		get { return m_Instance; }
	}
	#endregion

	#region Unity Functions
	/// <summary>
	/// Called when the script is being loaded.
	/// Creates an instance and sets the UI
	/// </summary>
	void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;

		m_CurrentUI = m_MenuUI;
		m_UIStack = new Stack<MenuStackItem>();
		LoadMenu();
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
			MenuStackItem menu = m_UIStack.Peek();
			m_State = menu.State;
			UpdateState();
		}

		else
		{
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
		m_State = MenuState.GAMEPAUSED;
		UpdateState();
		m_UIStack.Push(new MenuStackItem(m_GamePausedUI, MenuState.GAMEPAUSED));
		Time.timeScale = 0;
	}

	/// <summary>
	/// Un pauses the game and loads the game UI
	/// </summary>
	public void UnPauseGame()
	{
		Debug.Log("Un-Pausing Game");
		m_State = MenuState.GAME;
		UpdateState();
	}

	/// <summary>
	/// Loads the main menu UI
	/// </summary>
	public void LoadMenu()
	{
		Debug.Log("Loading Menu");
		m_State = MenuState.MAINMENU;
		UpdateState();
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_MenuUI, MenuState.MAINMENU));
		Time.timeScale = 0;
	}

	/// <summary>
	/// Loads the game UI
	/// </summary>
	public void LoadGame()
	{
		Debug.Log("Loading: Game");
		m_State = MenuState.GAME;
		UpdateState();
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_GameUI, MenuState.GAME));
	}

	/// <summary>
	/// Loads the settings UI
	/// </summary>
	public void LoadSettings()
	{
		Debug.Log("Loading: Settings");
		m_State = MenuState.SETTINGS;
		UpdateState();
		m_UIStack.Push(new MenuStackItem(m_SettingsUI, MenuState.SETTINGS));
	}


	/// <summary>
	/// Loads the leaderboard UI
	/// </summary>
	public void LoadEndScreen()
	{
		Debug.Log("Loading: EndScreen");
		m_State = MenuState.ENDSCREEN;
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_EndScreenUI, MenuState.ENDSCREEN));
		UpdateState();
	}

	/// <summary>
	/// Loads the leaderboard UI
	/// </summary>
	public void LoadLearderboard()
	{
		Debug.Log("Loading: Leaderboard");
		m_State = MenuState.LEADERBOARD;
		m_UIStack.Push(new MenuStackItem(m_LeaderboardUI, MenuState.LEADERBOARD));
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
			m_MenuUI.SetActive(true);
			m_CurrentUI = m_MenuUI;
		}

		if (m_State == MenuState.GAME)
		{
			m_GameUI.SetActive(true);
			m_CurrentUI = m_GameUI;
			m_GameUI.GetComponent<GameUI>().StartCountDown();
		}

		if (m_State == MenuState.GAMEPAUSED)
		{
			m_GamePausedUI.SetActive(true);
			m_CurrentUI = m_GamePausedUI;
		}

		if (m_State == MenuState.SETTINGS)
		{
			m_SettingsUI.SetActive(true);
			m_CurrentUI = m_SettingsUI;
		}

		if (m_State == MenuState.ENDSCREEN)
		{
			m_EndScreenUI.SetActive(true);
			m_CurrentUI = m_EndScreenUI;
		}

		if (m_State == MenuState.LEADERBOARD)
		{
			m_LeaderboardUI.SetActive(true);
			m_CurrentUI = m_LeaderboardUI;
		}
	}
	#endregion

	#region Extras
	/// <summary>
	/// Cause Jayden wanted to be apart
	/// </summary>
	public void JaydenWasHere(){ }
	#endregion
}
