using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Worked on by:
//	Declan Doller
//
//

public class SoundManager : MonoBehaviour
{
	private static SoundManager m_Instance;
	private float m_MainVolume = 1.0f;
	private float m_MusicVolume = 1.0f;
	private float m_OtherVolume = 1.0f;

	public AudioSource m_GameMusic = null;
	public AudioSource m_MenuMusic = null;
	public AudioSource[] m_CollisionSounds = new AudioSource[10];
	public AudioSource[] m_MenuSelectSounds = new AudioSource[10];

	// Singleton instance
	public static SoundManager Instance
	{
		get { return m_Instance; }
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

	// Sets the main volume
	public void SetMainVolume(float volume)
	{
		if (volume > 1)
		{
			volume = 1.0f;
		}

		else if (volume < 0)
		{
			volume = 0;
		}
		m_MainVolume = volume;
	}

	// Sets the music volume
	public void SetMusicVolume(float volume)
	{
		if (volume > 1)
		{
			volume = 1.0f;
		}

		else if (volume < 0)
		{
			volume = 0;
		}

		m_MusicVolume = volume;
	}

	// Sets the other volume
	public void SetOtherVolume(float volume)
	{
		if (volume > 1)
		{
			volume = 1.0f;
		}

		else if (volume < 0)
		{
			volume = 0;
		}

		m_OtherVolume = volume;
	}

	// Plays the Game Music
	public void PlayGameMusic()
	{
		m_GameMusic.Play();
	}

	// Plays the menu Music
	public void PlayMenuMusic()
	{
		m_MenuMusic.Play();
	}

	// Plays a collsion sound
	public void PlayCollisionSound(int index)
	{
		m_CollisionSounds[index].Play();
	}

	// Plays a menu select sound
	public void PlayMenuSelectSound(int index)
	{
		m_MenuSelectSounds[index].Play();
	}
}

