using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for containing all the references to other UI related scripts
/// </summary>
public class UIController : MonoBehaviour
{
	public GameUI GameUI = null;					// Reference to the GameUI script
	public SettingsUI SettingsUI = null;			// Reference to SettingsUI script
	public LeaderboardUI LeaderboardUI = null;		// Reference to LeaderboardUI script
	public EndScreenUI EndScreenUI = null;			// Reference to EndScreenUI script
	public SoundUI SoundUI = null;					// Reference to SoundUI script
	public MenuController MenuController = null;	// Reference to MenuController script
	public UISounds UISounds = null;				// Reference to UISounds script

	private static UIController m_Instance;         // The current instance 
	public static UIController Instance             // The public current instance
	{
		get { return m_Instance; }
	}
	
	/// <summary>
	/// Called as script is loaded
	/// </summary>
	void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;
	}
}
