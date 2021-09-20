using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Worked on by:
//	Declan Doller
//
//

public struct SliderInput
{
	public Slider Sliders;
	public GameObject Input;
	
	public SliderInput(Slider slider, GameObject input)
	{
		Sliders = slider;
		Input = input;
	}
}

public class SoundManager : MonoBehaviour
{
	private static SoundManager m_Instance;
	public float m_MainVolume = 1.0f;
	public float m_MusicVolume = 1.0f;
	public float m_SFXVolume = 1.0f;

	public AudioSource m_GameMusic = null;
	public AudioSource m_MenuMusic = null;
	public AudioSource[] m_CollisionSounds = new AudioSource[10];
	public AudioSource[] m_MenuSelectSounds = new AudioSource[10];

	[Header ("Sliders")]
	public Slider m_MainVolumeSlider = null;
	public Slider m_MusicVolumeSlider = null;
	public Slider m_SFXVolumeSlider = null;

	[Header("InputFields")]
	public GameObject m_MainVolumeInput = null;
	public GameObject m_MusicVolumeInput = null;
	public GameObject m_SFXVolumeInput = null;

	private SliderInput m_Main;
	private SliderInput m_Music;
	private SliderInput m_SFX;

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

	// On start 
	private void Start()
	{
		// Pairs each Slider to it's InputField
		m_Main = new SliderInput(m_MainVolumeSlider, m_MainVolumeInput);
		m_Music = new SliderInput(m_MusicVolumeSlider, m_MusicVolumeInput);
		m_SFX = new SliderInput(m_SFXVolumeSlider, m_SFXVolumeInput);

		// Sets the Slider and InputFields to their defaults
		UpdateInputValue(m_Main, m_MainVolume);
		UpdateSliderValue(m_Main, m_MainVolume);

		UpdateInputValue(m_Music, m_MusicVolume);
		UpdateSliderValue(m_Music, m_MusicVolume); 

		UpdateInputValue(m_SFX, m_SFXVolume);
		UpdateSliderValue(m_SFX, m_SFXVolume);
	}

	// Updates every frame
	private void Update()
	{
	}

	// Sets the main volume
	public void SetMainVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}

		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		m_MainVolume = volume;
		UpdateInputValue(m_Main, m_MainVolume);
	}

	public void SetMainVolume(string text)
	{
		int volume;
		float volumefloat;
		if (int.TryParse(text, out volume))
		{
			if(volume > 100)
			{
				volume = 100;
			}

			else if (volume < 0)
			{
				volume = 0;
			}
			volumefloat = (float)volume; 
			m_MainVolume = (volumefloat / 100.0f);
			UpdateSliderValue(m_Main, m_MainVolume);
		}
	}

	// Sets the music volume
	public void SetMusicVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}

		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		m_MusicVolume = volume;
		UpdateInputValue(m_Music, m_MusicVolume);
	}
	public void SetMusicVolume(string text)
	{
		int volume;
		float volumefloat;
		if (int.TryParse(text, out volume))
		{
			if (volume > 100)
			{
				volume = 100;
			}

			else if (volume < 0)
			{
				volume = 0;
			}
			volumefloat = (float)volume;
			m_MusicVolume = (volumefloat / 100.0f);
			UpdateSliderValue(m_Music, m_MusicVolume);
		}
	}

	// Sets the SFX volume
	public void SetSFXVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}

		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		m_SFXVolume = volume;
		UpdateInputValue(m_SFX, m_SFXVolume);
	}
	public void SetSFXVolume(string text)
	{
		int volume;
		float volumefloat;
		if (int.TryParse(text, out volume))
		{
			if (volume > 100)
			{
				volume = 100;
			}

			else if (volume < 0)
			{
				volume = 0;
			}
			volumefloat = (float)volume;
			m_SFXVolume = (volumefloat / 100.0f);
			UpdateSliderValue(m_SFX, m_SFXVolume);
		}
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

	// Updates the slider value from Input fields
	private void UpdateSliderValue(SliderInput sliderInput, float value)
	{
		sliderInput.Sliders.GetComponent<Slider>().value = value;
	}

	// Updates the Input value based on the slider
	private void UpdateInputValue(SliderInput sliderInput, float value)
	{
		float num = value * 100;
		num = (int)num;
		
		
		TMP_InputField text = sliderInput.Input.GetComponent<TMP_InputField>();
		text.text = "" + num;
	}
}

