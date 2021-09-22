using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reference https://www.studica.com/blog/custom-input-manager-unity-tutorial

// safe to file, the default keys and save to file the custom keys
public class KeybindManager : MonoBehaviour
{

	[Header("Default keybinds")]
	public KeyCode Forward;
	public KeyCode TurnLeft;
	public KeyCode TurnRight;
	public KeyCode BackwardOrStop;

	private static KeybindManager m_Instance;
	public static KeybindManager Instance
	{
		get { return m_Instance; }
	}

	private void Awake()
	{
		// Initialize Singleton
		if (m_Instance != null && m_Instance != this)
			Destroy(this.gameObject);
		else
			m_Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		Forward = KeyCode.W;
		TurnLeft = KeyCode.A;
		TurnRight = KeyCode.S;
		BackwardOrStop = KeyCode.D;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Called when the "Reset to default" button is pressed
    /// Resets all the current keybinds to the default ones set
    /// </summary>
	public void ResetKeybinds()
	{
		
	}
}
