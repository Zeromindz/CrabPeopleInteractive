using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public GameObject m_GeneralSettings = null;                     // The Canvas holding the volume and resolution settings
    public GameObject m_KeybindSettings = null;                     // The Canvas holding the keybind settings

	[Header("General Settings")]
	public GameObject m_ResultionDropDown = null;                   // The resolution drop down box
	public GameObject m_GeneralButton = null;                       // The button that brings up the general settings

	[Header("Keybind Settings")]
	public GameObject m_KeybindButton = null;                       // The button that brings up the keybind settings

	[Header("Screen Resolutions")]
	public bool m_FullScreen = false;                                // If the game is fullscreen or not
	public List<Vector2> m_ScreenSizes = new List<Vector2>();       // The posible screen resolutions
	public int m_DropDownValue;                                     // The current index shown by the drop down box


	private void Start()
	{
		TMP_Dropdown drop = m_ResultionDropDown.GetComponent<TMP_Dropdown>();

		List<string> options = new List<string>();

		for (int i = 0; i < m_ScreenSizes.Count; i++)
		{
			options.Add("" + m_ScreenSizes[i].x + " X " + m_ScreenSizes[i].y);
		}

		m_DropDownValue  = 0;
		drop.AddOptions(options);
		SetGeneralUI();
	}
	/// <summary>
	/// Called when a tickbox is pressed.
	/// Togles fullscreen
	/// </summary>
	public void ToggleFullScreen()
	{
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
	/// Sets the index of the drop down box
	/// </summary>
	/// <param name="value"> The index of the chosen drop down box  </param>
	public void SetDropDownValue(int value)
	{
		m_DropDownValue = value;
	}

	/// <summary>
	/// Sets the Size of the screen
	/// </summary>
	public void SetScreenSize()
	{
		Vector2 size = m_ScreenSizes[m_DropDownValue];
		Screen.SetResolution((int)size.x, (int)size.y, m_FullScreen);
		Debug.Log("Screen set: " + size.x + " X " + size.y + "  Fullscreen: " + m_FullScreen);
	}

	/// <summary>
	/// Displays the General UI within the Settings UI
	/// </summary>
	public void SetGeneralUI()
	{
		m_GeneralSettings.SetActive(true);
		m_KeybindSettings.SetActive(false);
	}

	/// <summary>
	/// Displays the keybind UI within the Settings UI
	/// </summary>
	public void SetKeyBindUI()
	{
		m_KeybindSettings.SetActive(true);
		m_GeneralSettings.SetActive(false);
	}
}
