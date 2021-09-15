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
		ENDSCREEN,
	}

public class MenuController : MonoBehaviour
{
	private static MenuController m_Instance;
	private MenuState m_State;

	// States for the game

	// Singleton instance
	public static MenuController Instance
	{
		get { return m_Instance; }
	}

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
	}

	// Sets the game state
	public void SetState(MenuState state)
	{
		m_State = state;
	}

	// Toggles pause
	public void TogglePause()
	{
		SetState(IsGamePaused ? MenuState.GAMEPAUSED : MenuState.GAME);
	}
}
