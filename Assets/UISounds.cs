using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{ 
	public void PlayUISelect()
	{
		SoundManager.Instance.PlayUISelectSound();
	}

	public void PlayUIHover()
	{
		SoundManager.Instance.PlayUIHoverSound();
	} 
}
