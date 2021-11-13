using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public GameUI GameUI = null;
	public SettingsUI SettingsUI = null;
	public LeaderboardUI LeaderboardUI = null;
	public EndScreenUI EndScreenUI = null;
	public SoundUI SoundUI = null;
	public MenuController MenuController = null;
	public UISounds UISounds = null;

	private static UIController m_Instance;                       // The current instance 
	public static UIController Instance                           // The public current instance
	{
		get { return m_Instance; }
	}
	void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;
	}
}
