using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// An enum for reffering to certain volume types
/// </summary>
public enum VolumeType
{
	Master,
	Music,
	SFX
}

/// <summary>
/// The manager class used for all sound related things
/// </summary>
public class SoundManager : MonoBehaviour
{
	#region Variables/Properties
	[Header("Current volumes")]
	public float m_MasterVolume = 1.0f;													// Current volume for Master
	public float m_MusicVolume = 1.0f;												    // Current volume for the Music
	public float m_SFXVolume = 1.0f;													// Current volume for the SFX

	public float m_FinalMusicVolume = 1.0f;												// The calculated music volume
	public float m_FinalSFXVolume = 1.0f;												// The calculated SFX volume

	[Header("Sounds")]
	[SerializeField] private AudioClip m_GameMusic = null;                              // The game music
	[SerializeField] private AudioClip m_MenuMusic = null;                              // The menu music
	[SerializeField] private AudioClip[] m_CollisionClips = new AudioClip[10];          // The collision sounds
	[SerializeField] private AudioClip[] m_GhostPickupClips = new AudioClip[10];        // The sounds for ghost pickup
	[SerializeField] private AudioClip[] m_BGMClips = new AudioClip[10];				// The back ground music clips
	[SerializeField] private AudioClip[] m_UIClips = new AudioClip[10];					// The menu selection sounds
	[SerializeField] private AudioClip[] m_BoostClips = new AudioClip[10];				// The boost sound clips

	[SerializeField,Range(0,1)] private float BoostFadeInOutTime;						// How fast the boost sound fades in and out

	[Header("Sources")]
	[SerializeField] private GameObject m_SoundSourceObject = null;						// The gameobject holding alltheaudio sources								
	private AudioSource m_CollisionSource = null;										// The collision audio source component
	private AudioSource m_GhostPickupSource = null;										// The Ghost pickup audio source component
	private AudioSource m_BGMSource = null;												// The back ground music audio source component
	private AudioSource m_BoostSource = null;											// The boost audio source component
	private AudioSource m_UISource = null;												// The UI sounds audio source component

	private List<AudioSource> m_MusicVolumeSources = null;								// A list of aud
	private List<AudioSource> m_SFXVolumeSources = null;

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

		m_MusicVolumeSources = new List<AudioSource>();
		m_SFXVolumeSources = new List<AudioSource>();

		AudioSource[] SourceArray = m_SoundSourceObject.GetComponentsInChildren<AudioSource>(false);
		m_CollisionSource = SourceArray[0];
		m_GhostPickupSource = SourceArray[1];
		m_BGMSource = SourceArray[2];
		m_BoostSource = SourceArray[3];
		m_UISource = SourceArray[4];

		m_SFXVolumeSources.Add(m_UISource);
		m_SFXVolumeSources.Add(m_GhostPickupSource);
		m_SFXVolumeSources.Add(m_BoostSource);
		m_SFXVolumeSources.Add(m_CollisionSource);

		m_MusicVolumeSources.Add(m_BGMSource);

		//m_BoostSource.volume = 0.0f;
		PlayBoost(0);
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

	/// <summary>
	/// Called when a player collides with an object
	/// Plays a random collision sound
	/// </summary>
	public void PlayRandomCollisionSound()
	{
		int rand = Random.Range(0, m_CollisionClips.Length);
		m_CollisionSource.clip = m_CollisionClips[rand];
		m_CollisionSource.Play();
		Debug.Log("SoundManager play collision: Random " + rand);
	}
    #endregion

    #region GhostPickup
	/// <summary>
	/// Called when the player collides with a ghost pickup
	/// PLays a ghost pick up sound using its index from an array
	/// </summary>
	/// <param name="index"></param>
    public void PlayGhostPickupSound(int index)
	{
		m_GhostPickupSource.clip = m_GhostPickupClips[index];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: " + index);
	}

	/// <summary>
	/// Called when the player collides with a ghost pickup
	/// Plays a random ghost pickup sound
	/// </summary>
	public void PlayRandomGhostPickupSound()
	{
		int rand = Random.Range(0, m_GhostPickupClips.Length);
		m_GhostPickupSource.clip = m_GhostPickupClips[rand];
		m_GhostPickupSource.Play();
		Debug.Log("SoundManager play Ghost: Random " + rand);
	}
	#endregion

	#region Background Music
	/// <summary>
	/// Called when the game starts
	/// Plays the backround music at the index
	/// </summary>
	/// <param name="index">An index referring to a background music sound from a array</param>
	public void PlayBGM(int index)
	{
		m_BGMSource.clip = m_BGMClips[index];
		m_BGMSource.Play();
		Debug.Log("SoundManager play BGM: " + index);
	}

	/// <summary>
	/// Called when the game starts
	/// Plays a random background music
	/// </summary>
	public void PlayRandomBGM()
	{ 
		int rand = Random.Range(0, m_BGMClips.Length);
		m_BGMSource.clip = m_BGMClips[rand];
		m_BGMSource.Play();
		Debug.Log("SoundManager play BGM: Random " + rand);
	}

	/// <summary>
	/// Called when the game ends,
	/// stops the backround music from playing
	/// </summary>
	public void StopBGM()
	{
		m_BGMSource.Stop();
	}
	#endregion

	#region Boosting Sounds
	/// <summary>
	/// Called when the player boosts
	/// Plays a boosting sound at an index on an array
	/// </summary>
	/// <param name="index"></param>
	public void PlayBoost(int index)
	{
		m_BoostSource.clip = m_BoostClips[index];
		m_BoostSource.loop = true;
		m_BoostSource.volume = 0.0f;
		m_BoostSource.Play();
		Debug.Log("SoundManager Starting boost: " + index);
	}

	/// <summary>
	/// Called when the game ends
	/// Stops the boost sound from playing
	/// </summary>
	public void StopBoost()
	{
		Debug.Log("SoundManager Ending boost ");
		m_BoostSource.Stop();
	}

	/// <summary>
	/// Called when the player starts boosting,
	/// Fades in the boost sound
	/// </summary>
	public void BoostFadeIn()
	{
		m_BoostSource.volume += Time.deltaTime / BoostFadeInOutTime;
		Debug.Log("Fade in");
	}

	/// <summary>
	/// Called when the player stops boosting
	/// Fades the boost sound out
	/// </summary>
	public void BoostFadeOut()
	{
		m_BoostSource.volume -= Time.deltaTime / BoostFadeInOutTime;
	}

	public void BoostFadeToStop()
	{
		while(m_BoostSource.volume != 0)
		{
			m_BoostSource.volume -= Time.deltaTime / BoostFadeInOutTime;
		}
	}
	#endregion

	#region UI Sounds

	/// <summary>
	/// Called when the mouse is hovering over a button
	/// Plays a UI hover sound at a random pitch
	/// </summary>
	public void PlayUIHoverSound()
	{
		m_UISource.clip = m_UIClips[0];
		float range = Random.Range(1, 301);
		range = range / 100;
		m_UISource.pitch = range;
		m_UISource.Play();
		//Debug.Log("SoundManager play UI Hover");
	}

	/// <summary>
	/// Called when the mouse has selected a button
	/// Plays a UI select sound at a random pitch
	/// </summary>
	public void PlayUISelectSound()
	{
		m_UISource.clip = m_UIClips[1];
		float range = Random.Range(1, 301);
		range = range / 100;
		m_UISource.pitch = range; 
		m_UISource.Play();
		//Debug.Log("SoundManager play UI Select");
	}

	#endregion
	
	/// <summary>
	/// Called whenever the user edits a volume value
	/// Sets the vollume for the Audio sources
	/// </summary>
	/// <param name="value">The volume of the audio source</param>
	/// <param name="type">The type of sound volume that will be changing</param>
	public void SetVolume(float value, VolumeType type)
	{
		if(type == VolumeType.Master)
		{
			m_MasterVolume = value;
		}

		else if(type == VolumeType.Music)
		{
			m_MusicVolume = value;
		}

		else if(type == VolumeType.SFX)
		{
			m_SFXVolume = value;
		}

		m_FinalMusicVolume = m_MusicVolume * m_MasterVolume;
		m_FinalSFXVolume = m_SFXVolume * m_MasterVolume;

		ApplyVolume();
	}

	/// <summary>
	/// Called after the volumes have changed
	/// Appplies the volume to theaudio sources
	/// </summary>
	private void ApplyVolume()
	{
		for (int i = 0; i < m_MusicVolumeSources.Count; i++)
		{
			m_MusicVolumeSources[i].volume = m_FinalMusicVolume;
		}

		for (int i = 0; i < m_SFXVolumeSources.Count; i++)
		{
			m_SFXVolumeSources[i].volume = m_FinalSFXVolume;
		}

		//Debug.Log("Volume, Master: " + m_MasterVolume + " Music: " + m_MusicVolume + " Final: " +m_FinalMusicVolume + " SFX: " + m_SFXVolume + " Final: " + m_FinalSFXVolume);
	}
	#endregion
}

