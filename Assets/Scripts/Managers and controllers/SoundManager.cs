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
	[Header("Current volumes")]
	public float m_MainVolume = 1.0f;									// Current volume for Main
	public float m_MusicVolume = 1.0f;									// Current volume for the Music
	public float m_SFXVolume = 1.0f;									// Current volume for the SFX

	[Header("Sounds")]
	public AudioSource m_GameMusic = null;								// The game music
	public AudioSource m_MenuMusic = null;								// The menu music
	public AudioSource[] m_CollisionSounds = new AudioSource[10];		// The collision sounds
	public AudioSource[] m_MenuSelectSounds = new AudioSource[10];		// The menu selection sounds

	[Header ("Main volume")]
	public Slider m_MainVolumeSlider = null;							// The slider for Main volume
	public GameObject m_MainVolumeInput = null;							// The inputbox for Main volume

	[Header("Music volume")]
	public Slider m_MusicVolumeSlider = null;							// The slider for Music volume
	public GameObject m_MusicVolumeInput = null;						// The inpubox for Music volume

	[Header("SFX volume")]
	public Slider m_SFXVolumeSlider = null;								// The slider for SFX volume
	public GameObject m_SFXVolumeInput = null;							// The inputbox for SFX volume

	private SliderInput m_Main;											// Slider and Inputboxz for Main sounds
	private SliderInput m_Music;										// Slider and Inputbox
	private SliderInput m_SFX;

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

	}

	/// <summary>
	/// Called on first frame.
	/// Initializes everything and sets defaults
	/// </summary>
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
	#endregion

	/// <summary>
	/// Called when the Main slider value has been changed.
	/// Calls to update the inputbox value
	/// </summary>
	/// <param name="volume">The value of the slider</param>
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

	/// <summary>
	/// Called when the Main inputbox value has been changed.
	/// Calls to update the slider with the new value
	/// </summary>
	/// <param name="text">The text of the inputbox will be an integer</param>
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

	/// <summary>
	/// Called when the Music slider value has been changed.
	/// Calls to update the value of the input box
	/// </summary>
	/// <param name="volume">The value of the slider</param>
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

	/// <summary>
	/// Called when the Music inputbox value has been changed.
	/// Calls to update the slider with the new value
	/// </summary>
	/// <param name="text">The text of the inputbox will be an integer</param>
	public void SetMusicVolume(string text)
	{
		// converting and clamping the value to be a float from 0.0f - 1.0f
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

	/// <summary>
	/// Called when the SFX slider value has been changed.
	/// Calls to update the Inputbox with the new value
	/// </summary>
	/// <param name="volume">The value of the slider</param>
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

	/// <summary>
	/// Called when the SFX inputbox value has been changed.
	/// calls to update the slider with the new value
	/// </summary>
	/// <param name="text">The text of the input box will be an integer</param>
	public void SetSFXVolume(string text)
	{
		// converting and clamping the value to be a float from 0.0f - 1.0f
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

	/// <summary>
	/// Called at the start of the game.
	/// Plays the music for the game
	/// </summary>
	public void PlayGameMusic()
	{
		m_GameMusic.Play();
	}

	/// <summary>
	/// Called in the menu.
	/// Plays the menu sound/music
	/// </summary>
	public void PlayMenuMusic()
	{
		m_MenuMusic.Play();
	}

	/// <summary>
	/// Called when the player collides with an object.
	/// Plays a collision sound
	/// </summary>
	/// <param name="index">The index of the collision sound that will be played</param>
	public void PlayCollisionSound(int index)
	{
		m_CollisionSounds[index].Play();
	}

	/// <summary>
	/// Called when something in the menu has been selected.
	/// Plays a Menu Selec sound
	/// </summary>
	/// <param name="index">The index of the sound that will be played</param>
	public void PlayMenuSelectSound(int index)
	{
		m_MenuSelectSounds[index].Play();
	}

	/// <summary>
	/// Called after an input box has changed value.
	/// Updates the corresponing slider
	/// </summary>
	/// <param name="sliderInput">The pair of slider and input box that needs to be updated</param>
	/// <param name="value">The value the slider needs to be updated to</param>
	private void UpdateSliderValue(SliderInput sliderInput, float value)
	{
		sliderInput.Sliders.GetComponent<Slider>().value = value;
	}

	/// <summary>
	/// Called after a slider value has changed.
	/// Updates the corresponding Input Box value
	/// </summary>
	/// <param name="sliderInput">The pair of slider and input box that needs to be updated</param>
	/// <param name="value">The value the input box needs to be updated to</param>
	private void UpdateInputValue(SliderInput sliderInput, float value)
	{
		float num = value * 100;
		num = (int)num;
		
		
		TMP_InputField text = sliderInput.Input.GetComponent<TMP_InputField>();
		text.text = "" + num;
	}
}

