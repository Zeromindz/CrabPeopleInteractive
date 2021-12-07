using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// A script relating to the resolution and navigating settings UI
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject m_GeneralSettings = null;		// The Canvas holding the volume and resolution settings
    [SerializeField] private GameObject m_KeybindSettings = null;		// The Canvas holding the keybind settings

	[Header("General Settings")]
	[SerializeField] private GameObject m_ResultionDropDown = null;		// The resolution drop down box
	[SerializeField] private GameObject m_GeneralButton = null;			// The button that brings up the general settings

	[Header("Keybind Settings")]
	[SerializeField] private GameObject m_KeybindButton = null;         // The button that brings up the keybind settings

	[Header("Screen Resolutions")]
	private bool m_FullScreen = false;									// If the game is fullscreen or not
	private int m_DropDownValue;										// The current index shown by the drop down box
	private Resolution[] m_Resolutions = null;                          // Array of resoluutions
	private bool IsSaved = true;

	/// <summary>
	/// Called before first frame
	/// Caches needed information
	/// </summary>
	private void Start()
	{
		TMP_Dropdown drop = m_ResultionDropDown.GetComponent<TMP_Dropdown>();

		List<string> options = new List<string>();

		m_Resolutions = Screen.resolutions;

		for (int i = 0; i < m_Resolutions.Length; i++)
		{
			options.Add("" + m_Resolutions[i].width + " X " + m_Resolutions[i].height + " : " + m_Resolutions[i].refreshRate);
		}

		drop.AddOptions(options);
		Resolution size = m_Resolutions[m_Resolutions.Length - 1];
		m_DropDownValue  = m_Resolutions.Length - 1;
		m_ResultionDropDown.GetComponent<TMP_Dropdown>().value = m_Resolutions.Length - 1;
		Screen.SetResolution(size.width, size.height, m_FullScreen, size.refreshRate);
		SetGeneralUI();
		//LoadSettings();
	}
	/// <summary>
	/// Called when the fullscreen tickbox is pressed.
	/// Togles fullscreen
	/// </summary>
	public void ToggleFullScreen()
	{
		IsSaved = false;
		if (m_FullScreen)
		{
			m_FullScreen = false;
		}

		else
		{
			m_FullScreen = true;
		}
	}

	/// <summary>
	/// Called when a new item from the dropdown box is selected
	/// Sets the index of the drop down box
	/// </summary>
	/// <param name="value"> The index of the chosen drop down box  </param>
	public void SetDropDownValue(int value)
	{
		IsSaved = false;
		m_DropDownValue = value;
	}

	/// <summary>
	/// Called when the Set resolution button is pressed
	/// Sets the Size of the screen
	/// </summary>
	public void SetScreenSize()
	{
		Resolution size = m_Resolutions[m_DropDownValue];
		Screen.SetResolution(size.width, size.height, m_FullScreen, size.refreshRate);
		Debug.Log("Screen set: " + size.width + " X " + size.height + ", Fullscreen: " + m_FullScreen + ", Refresh rate " + size.refreshRate);
		IsSaved = true;
	}

	/// <summary>
	/// Called when the general button is pressed
	/// Displays the General UI within the Settings UI
	/// </summary>
	public void SetGeneralUI()
	{
		m_GeneralSettings.SetActive(true);
		m_KeybindSettings.SetActive(false);
	}

	/// <summary>
	/// Called when the keybind button is pressed
	/// Displays the keybind UI within the Settings UI
	/// </summary>
	public void SetKeyBindUI()
	{
		m_KeybindSettings.SetActive(true);
		m_GeneralSettings.SetActive(false);
	}

	//public void LoadSettings()
	//{
	//	m_DropDownValue = PlayerPrefs.GetInt("DropDownIndex");
	//	m_ResultionDropDown.GetComponent<TMP_Dropdown>().value = m_DropDownValue;
	//	int value = PlayerPrefs.GetInt("Fullscreen");
	//	if (value == 1)
	//	{
	//		m_FullScreen = true;
	//	}

	//	else
	//	{
	//		m_FullScreen = false;
	//	}
	//	SetScreenSize();
	//	UIController.Instance.SoundUI.LoadSoundSettings();
	//}

	//public void SaveSettings()
	//{
	//	if (IsSaved)
	//	{
	//		PlayerPrefs.SetInt("DropDownIndex", m_DropDownValue);
	//		int value;
	//		if (m_FullScreen)
	//		{
	//			value = 1;
	//		}
	//		else
	//		{
	//			value = 0;
	//		}
	//		PlayerPrefs.SetInt("Fullscreen", value);
	//		PlayerPrefs.Save();
	//	}

	//	UIController.Instance.SoundUI.SaveSoundSettings();
	//}
}
