using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public GameObject m_SettingsUI = null;
	public GameObject m_EndScreenUI = null;
	private GameObject m_CurrentUI = null;
	Stack<MenuStackItem> m_UIStack = new Stack<MenuStackItem>();

	public bool IsGamePaused
	{
		get { return m_State == MenuState.GAMEPAUSED; }
	}

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

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && m_State == MenuState.GAME)
		{
			PauseGame();
		}

		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			ReturnToPreviousUI();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
		
		}
	}

	public void ReturnToPreviousUI()
	{
		if(m_UIStack.Count >= 2)
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

		if(m_State == MenuState.MAINMENU)
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
	public void JaydenWasHere()
	{}

	// Toggles pause
	public void TogglePause()
	{
		SetState(IsGamePaused ? MenuState.GAMEPAUSED : MenuState.GAME);
	}
}
