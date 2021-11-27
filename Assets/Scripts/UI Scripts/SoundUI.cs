using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A struct pairing the a slider to it's respective inputbox and visa versa
/// </summary>
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

/// <summary>
/// A script relating to all the sound UI 
/// </summary>
public class SoundUI : MonoBehaviour
{
	#region Variables/Properties
	[Header("Current volumes")]
	public float m_MasterVolume = 1.0f;                                 // Current volume for Master
	public float m_MusicVolume = 1.0f;                                  // Current volume for the Music
	public float m_SFXVolume = 1.0f;                                    // Current volume for the SFX

	[Header("Master volume")]
	public Slider m_MasterVolumeSlider = null;                          // The slider for Master volume
	public GameObject m_MasterVolumeInput = null;                       // The inputbox for Master volume

	[Header("Music volume")]
	public Slider m_MusicVolumeSlider = null;                           // The slider for Music volume
	public GameObject m_MusicVolumeInput = null;                        // The inpubox for Music volume

	[Header("SFX volume")]
	public Slider m_SFXVolumeSlider = null;                             // The slider for SFX volume
	public GameObject m_SFXVolumeInput = null;                          // The inputbox for SFX volume

	private SliderInput m_Master;                                       // Slider and Inputboxz for Master sounds
	private SliderInput m_Music;                                        // Slider and Inputbox for Music sounds
	private SliderInput m_SFX;                                          // Slider and Inputbox for SFX sounds
	#endregion

	#region Unity Functions
	
	/// <summary>
	/// Called on first frame.
	/// Initializes everything and sets defaults
	/// </summary>
	private void Start()
	{
		// Pairs each Slider to it's InputField
		m_Master = new SliderInput(m_MasterVolumeSlider, m_MasterVolumeInput);
		m_Music = new SliderInput(m_MusicVolumeSlider, m_MusicVolumeInput);
		m_SFX = new SliderInput(m_SFXVolumeSlider, m_SFXVolumeInput);

		// Sets the Slider and InputFields to their defaults
		UpdateInputValue(m_Master, m_MasterVolume);
		UpdateSliderValue(m_Master, m_MasterVolume);

		UpdateInputValue(m_Music, m_MusicVolume);
		UpdateSliderValue(m_Music, m_MusicVolume);

		UpdateInputValue(m_SFX, m_SFXVolume);
		UpdateSliderValue(m_SFX, m_SFXVolume);
	}
	#endregion

	#region Functions
	/// <summary>
	/// Called when the Master slider value has been changed.
	/// Calls to update the inputbox value
	/// </summary>
	/// <param name="volume">The value of the slider</param>
	public void SetMasterVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}

		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		m_MasterVolume = volume;
		UpdateInputValue(m_Master, m_MasterVolume);
		SoundManager.Instance.SetVolume(m_MasterVolume, VolumeType.Master);
	}

	/// <summary>
	/// Called when the Master inputbox value has been changed.
	/// Calls to update the slider with the new value
	/// </summary>
	/// <param name="text">The text of the inputbox will be an integer</param>
	public void SetMasterVolume(string text)
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
			m_MasterVolume = (volumefloat / 100.0f);
			UpdateSliderValue(m_Master, m_MasterVolume);
			SoundManager.Instance.SetVolume(m_MasterVolume, VolumeType.Master);
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
		SoundManager.Instance.SetVolume(m_MusicVolume, VolumeType.Music);
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
			SoundManager.Instance.SetVolume(m_MusicVolume, VolumeType.Music);
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
		SoundManager.Instance.SetVolume(m_SFXVolume, VolumeType.SFX);
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
			SoundManager.Instance.SetVolume(m_SFXVolume, VolumeType.SFX);
		}
	}
	/// <summary>
	/// Called after an input box has changed value.
	/// Updates the corresponing slider
	/// </summary>
	/// <param name="sliderInput">The pair of slider and input box that needs to be updated</param>
	/// <param name="value">The value the slider needs to be updated to</param>
	private void UpdateSliderValue(SliderInput sliderInput, float value)
	{
		if(sliderInput.Sliders != null)
		{
			sliderInput.Sliders.GetComponent<Slider>().value = value;
		}
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
		if(sliderInput.Input != null)
		{
			TMP_InputField text = sliderInput.Input.GetComponent<TMP_InputField>();
			text.text = "" + num;
		}
	}
	#endregion

	public void LoadSoundSettings()
	{
		m_MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
		m_MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
		m_SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

		// Sets the Slider and InputFields to their saved values
		UpdateInputValue(m_Master, m_MasterVolume);
		UpdateSliderValue(m_Master, m_MasterVolume);

		UpdateInputValue(m_Music, m_MusicVolume);
		UpdateSliderValue(m_Music, m_MusicVolume);

		UpdateInputValue(m_SFX, m_SFXVolume);
		UpdateSliderValue(m_SFX, m_SFXVolume);
	}

	public void SaveSoundSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", m_MasterVolume);
		PlayerPrefs.SetFloat("MusicVolume", m_MusicVolume);
		PlayerPrefs.SetFloat("SFXVolume", m_SFXVolume);
		PlayerPrefs.Save();
	}
}
