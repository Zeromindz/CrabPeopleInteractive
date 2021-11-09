using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Worked on by:
//	Declan Doller
//
//



/// <summary>
/// The manager class used for all sound related things
/// </summary>
public class SoundManager : MonoBehaviour
{
	#region Variables/Properties
	[Header("Current volumes")]
	public float m_MainVolume = 1.0f;                                   // Current volume for Main
	public float m_MusicVolume = 1.0f;                                  // Current volume for the Music
	public float m_SFXVolume = 1.0f;                                    // Current volume for the SFX

	[Header("Sounds")]
	public AudioClip m_GameMusic = null;                                // The game music
	public AudioClip m_MenuMusic = null;                                // The menu music
	public AudioClip[] m_CollisionClips = new AudioClip[10];            // The collision sounds
	public AudioClip[] m_GhostPickupClips = new AudioClip[10];          // The sounds for ghost pickup
	public AudioClip[] m_BMGClips = new AudioClip[10];
	public AudioClip[] m_MenuSelectClips = new AudioClip[10];			// The menu selection sounds

	[Header("Sources")]
	public GameObject m_SoundSourceObject = null;
	private AudioSource m_CollisionSource = null;
	private AudioSource m_GhostPickupSource = null;
	private AudioSource m_BGM = null;

	#endregion

	#region Singleton
	// Singleton instance
	private static SoundManager m_Instance;
	public static SoundManager Instance
	{
		get { return m_Instance; }
	}
	#endregion

	#region Unity Functions
	/// <summary>
	/// Called when the scrip is being loaded.
	/// Initializes instance
	/// </summary>
	void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;

		AudioSource[] SourceArray = m_SoundSourceObject.GetComponentsInChildren<AudioSource>(false);
		m_CollisionSource = SourceArray[0];
		m_GhostPickupSource = SourceArray[1];
		
	}
	#endregion

	#region Functions

	/// <summary>
	/// Called at the start of the game.
	/// Plays the music for the game
	/// </summary>
	public void PlayGameMusic()
	{
	//	m_GameMusic.Play();
	}

	/// <summary>
	/// Called in the menu.
	/// Plays the menu sound/music
	/// </summary>
	public void PlayMenuMusic()
	{
		//m_MenuMusic.volume = m_MusicVolume;
	//	m_MenuMusic.Play();
	}

	/// <summary>
	/// Called when something in the menu has been selected.
	/// Plays a Menu Selec sound
	/// </summary>
	/// <param name="index">The index of the sound that will be played</param>
	public void PlayMenuSelectSound(int index)
	{
		
	}

    #region Collisions
    /// <summary>
    /// Called when the player collides with an object.
    /// Plays a collision sound
    /// </summary>
    /// <param name="index">The index of the collision sound that will be played</param>
    public void PlayCollisionSound(int index)
	{
		m_CollisionSource.clip = m_CollisionClips[index];
		m_CollisionSource.Play();
		Debug.Log("SoundManager play collision: " + index);
	}

	public void PlayRandomCollisionSound()
	{
		int rand = Random.Range(0, m_CollisionClips.Length);
		m_CollisionSource.clip = m_CollisionClips[rand];
		m_CollisionSource.Play();
		Debug.Log("SoundManager play collision: Random " + rand);
	}
    #endregion

    #region GhostPickup
    public void PlayGhostPickupSound(int index)
	{
		m_GhostPickupSource.clip = m_GhostPickupClips[index];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: " + index);
	}

	public void PlayRandomGhostPickupSound()
	{
		int rand = Random.Range(0, m_GhostPickupClips.Length);
		m_GhostPickupSource.clip = m_GhostPickupClips[rand];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: Random " + rand);
	}
	#endregion

	#region Background Music
	public void PlayBGM(int index)
	{
		m_BGM.clip = m_GhostPickupClips[index];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: " + index);
	}

	public void PlayRandomGhostPickupSound()
	{
		int rand = Random.Range(0, m_GhostPickupClips.Length);
		m_GhostPickupSource.clip = m_GhostPickupClips[rand];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: Random " + rand);
	}

	#endregion
	public void SetSFXVolume(float volume)
	{
		m_SFXVolume = volume;
	}

	#endregion
}

