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
	public GameObject m_SettingsUI = null;							// The canvas holding The settings  UI

	[Header("Settings UI")]
	public GameObject m_GeneralSettings = null;						// The Canvas holding the volume and resolution settings
	public GameObject m_KeybindSettings = null;						// The Canvas holding the keybind settings

	[Header("General Settings")]
	public GameObject m_ResultionDropDown = null;					// The resolution drop down box
	public GameObject m_GeneralButton = null;						// The button that brings up the general settings

	[Header("Keybind Settings")]
	public GameObject m_KeybindButton = null;						// The button that brings up the keybind settings

	[Header("Screen Resolutions")]
	public bool m_FullScreen = true;								// If the game is fullscreen or not
	public List<Vector2> m_ScreenSizes = new List<Vector2>();		// The posible screen resolutions
	public int m_DropDownValue;										// The current index shown by the drop down box


	// -----Private-----
	Vector2 m_ScreenSize;
	private GameObject m_CurrentUI = null;                          // The current Ui that is being displayed
	private Stack<MenuStackItem> m_UIStack;							// The stack holding information when travelling between UIs
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

	/// <summary>
	/// Runs on first frame.
	/// Initializes everything 
	/// </summary>
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

	/// <summary>
	/// Runs every frame.
	/// Checks for key presses 
	/// </summary>
	private void Update()
	{
		// Pauses the game
		if (Input.GetKeyDown(KeyCode.Escape) && m_State == MenuState.GAME)
		{
			PauseGame();
		}

		// Returns to the previous UI
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			ReturnToPreviousUI();
		}
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
		SetGeneralUI();
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

	// -----SettingsUI-----

	/// <summary>
	/// Called when a tickbox is pressed.
	/// Togles fullscreen
	/// </summary>
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

	/// <summary>
	/// Sets the index of the drop down box
	/// </summary>
	/// <param name="value"> The index of the chosen drop down box  </param>
	public void SetDropDownValue(int value)
	{
		m_DropDownValue = value;
	}

	/// <summary>
	/// Sets the Size of the screen
	/// </summary>
	public void SetScreenSize()
	{
		Vector2 size = m_ScreenSizes[m_DropDownValue];
		Screen.SetResolution((int)size.x, (int)size.y, m_FullScreen);
		Debug.Log("Screen set: " + size.x + " X " + size.y + "  Fullscreen: " + m_FullScreen);
	}
		
	/// <summary>
	/// Displays the General UI within the Settings UI
	/// </summary>
	public void SetGeneralUI()
	{
		EventSystem.current.SetSelectedGameObject(m_GeneralButton);
		m_GeneralSettings.SetActive(true);
		m_KeybindSettings.SetActive(false);
	}

	/// <summary>
	/// Displays the keybind UI within the Settings UI
	/// </summary>
	public void SetKeyBindUI()
	{
		EventSystem.current.SetSelectedGameObject(m_KeybindButton);
		m_KeybindSettings.SetActive(true);
		m_GeneralSettings.SetActive(false);
	}
	#endregion

	#region Extras
	/// <summary>
	/// Cause Jayden wanted to be apart
	/// </summary>
	public void JaydenWasHere(){ }
	#endregion
}
