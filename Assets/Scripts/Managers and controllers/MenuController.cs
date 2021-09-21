using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Worked on by:
//	Declan Doller
//
//

public enum MenuState
{
	MAINMENU,
	GAME,
	GAMEPAUSED,
	SETTINGS,
	ENDSCREEN,
}

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

public class MenuController : MonoBehaviour
{
	private static MenuController m_Instance;
	public MenuState m_State;

	// States for the game

	// Singleton instance
	public static MenuController Instance
	{
		get { return m_Instance; }
	}
	public GameObject m_MenuUI = null;
	public GameObject m_GameUI = null;
	public GameObject m_GamePausedUI = null;
	public GameObject m_EndScreenUI = null;
	private GameObject m_CurrentUI = null;
	Stack<MenuStackItem> m_UIStack = new Stack<MenuStackItem>();
	public bool IsGamePaused
	{
		get { return m_State == MenuState.GAMEPAUSED; }
	}

	[Header("Settings UI")]
	// -----Public----- 
	public GameObject m_SettingsUI = null;
	public GameObject m_ResultionDropDown = null;

	[Header("Screen Resolutions")]
	public bool m_FullScreen = true;
	public List<Vector2> m_ScreenSizes = new List<Vector2>();
	public int m_DropDownValue;
	// -----Private-----
	Vector2 m_ScreenSize;


	// Called when script is being loaded
	void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;

		m_CurrentUI = m_MenuUI;
		LoadMenu();
		//m_State = MenuState.MAINMENU;
	}

	private void Start()
	{
		TMP_Dropdown drop = m_ResultionDropDown.GetComponent<TMP_Dropdown>();

		List<string> options = new List<string>();

		for (int i = 0; i < m_ScreenSizes.Count; i++)
		{
			options.Add("" + m_ScreenSizes[i].x + " X " + m_ScreenSizes[i].y);
		}

		drop.AddOptions(options);
	}

	// Runs everyframe
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && m_State == MenuState.GAME)
		{
			PauseGame();
		}

		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			ReturnToPreviousUI();
		}
	}

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

	// Pauses the game
	public void PauseGame()
	{
		Debug.Log("Pausing Game");
		m_State = MenuState.GAMEPAUSED;
		UpdateState();
		m_UIStack.Push(new MenuStackItem(m_GamePausedUI, MenuState.GAMEPAUSED));
	}

	// Unpauses the game
	public void UnPauseGame()
	{
		Debug.Log("Un-Pausing Game");
		m_State = MenuState.GAME;
		UpdateState();
	}

	// Loads the main menu
	public void LoadMenu()
	{
		Debug.Log("Loading Menu");
		m_State = MenuState.MAINMENU;
		UpdateState();
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_MenuUI, MenuState.MAINMENU));
	}

	// Starts the game
	public void LoadGame()
	{
		Debug.Log("Loading: Game");
		m_State = MenuState.GAME;
		UpdateState();
		m_UIStack.Clear();
		m_UIStack.Push(new MenuStackItem(m_GameUI, MenuState.GAME));
	}

	// Loads the settings UI
	public void LoadSettings()
	{
		Debug.Log("Loading: Settings");
		m_State = MenuState.SETTINGS;
		UpdateState();
		m_UIStack.Push(new MenuStackItem(m_SettingsUI, MenuState.SETTINGS));
	}

	// Exits the application
	public void ExitGame()
	{
		Debug.Log("Closing App");
		Application.Quit();
	}

	// Sets the game state
	public void SetState(MenuState state)
	{
		m_State = state;
		UpdateState();
	}

	// Updates the states by activating the state
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
	}

	// Cause Jayden wanted to be apart
	public void JaydenWasHere(){ }

	// Toggles pause
	public void TogglePause()
	{
		SetState(IsGamePaused ? MenuState.GAMEPAUSED : MenuState.GAME);
	}

	// -----SettingsUI-----

	// Toggles fullscreen
	public void ToggleFullScreen()
	{
		if(m_FullScreen)
		{
			m_FullScreen = false;
		}

		else
		{
			m_FullScreen = true;
		}
	}

	// Sets the index of the dropdown
	public void SetDropDownValue(int value)
	{
		m_DropDownValue = value;
	}

	// Sets the screen resolution
	public void SetScreenSize()
	{
		Vector2 size = m_ScreenSizes[m_DropDownValue];
		Debug.Log("Screen set: " + size.x + " X " + size.y + "  Fullscreen: " + m_FullScreen);
		Screen.SetResolution((int)size.x, (int)size.y, m_FullScreen);
	}
}
