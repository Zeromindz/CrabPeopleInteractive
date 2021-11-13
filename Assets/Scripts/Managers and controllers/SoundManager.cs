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
	[SerializeField] private AudioClip m_GameMusic = null;                                // The game music
	[SerializeField] private AudioClip m_MenuMusic = null;                                // The menu music
	[SerializeField] private AudioClip[] m_CollisionClips = new AudioClip[10];            // The collision sounds
	[SerializeField] private AudioClip[] m_GhostPickupClips = new AudioClip[10];          // The sounds for ghost pickup
	[SerializeField] private AudioClip[] m_BGMClips = new AudioClip[10];
	[SerializeField] private AudioClip[] m_UIClips = new AudioClip[10];         // The menu selection sounds
	[SerializeField] private AudioClip[] m_BoostClips = new AudioClip[10];
	[SerializeField,Range(0,1)] private float BoostFadeInOutTime;

	[Header("Sources")]
	public GameObject m_SoundSourceObject = null;
	private AudioSource m_CollisionSource = null;
	private AudioSource m_GhostPickupSource = null;
	private AudioSource m_BGMSource = null;
	private AudioSource m_BoostSource = null;
	private AudioSource m_UISource = null;

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
		m_BGMSource = SourceArray[2];
		m_BoostSource = SourceArray[3];
		m_UISource = SourceArray[4];

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
		m_BGMSource.clip = m_BGMClips[index];
		m_BGMSource.Play();
		Debug.Log("SoundManager play BGM: " + index);
	}

	public void PlayRandomBGM()
	{ 
		int rand = Random.Range(0, m_BGMClips.Length);
		m_BGMSource.clip = m_BGMClips[rand];
		m_BGMSource.Play();
		Debug.Log("SoundManager play BGM: Random " + rand);
	}

	public void StopBGM()
	{
		m_BGMSource.Stop();
	}
	#endregion

	#region Boosting Sounds
	public void PlayBoost(int index)
	{
		m_BoostSource.clip = m_BoostClips[index];
		m_BoostSource.loop = true;
		m_BoostSource.volume = 0.0f;
		m_BoostSource.Play();
		Debug.Log("SoundManager Starting boost: " + index);

	}

	public void StopBoost()
	{
		Debug.Log("SoundManager Ending boost ");
		m_BoostSource.Stop();
	}
	public void BoostFadeIn()
	{
		m_BoostSource.volume += Time.deltaTime / BoostFadeInOutTime;
		Debug.Log("Fade in");
	}

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
	public void PlayUIHoverSound()
	{
		m_UISource.clip = m_UIClips[0];
		float range = Random.Range(1, 301);
		range = range / 100;
		m_UISource.pitch = range;
		m_UISource.Play();
		//Debug.Log("SoundManager play UI Hover");
	}

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
	public void SetSFXVolume(float volume)
	{
		m_SFXVolume = volume;
	}

	#endregion
}

