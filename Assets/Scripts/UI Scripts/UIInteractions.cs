using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIInteractions : MonoBehaviour
{
	[SerializeField] private Color m_DefaultColour;
	[SerializeField] private Color m_SwitchToColour;
	[SerializeField] private int m_DefaultSize = 30;
	[SerializeField] private int m_SelectedSize = 50;

	public void UIHoverEnter(BaseEventData data)
	{
		if(data != null)
		{
			GameObject obj = data.selectedObject.gameObject;
			Debug.Log(obj.name);
			TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
			text.color = m_SwitchToColour;
			text.fontSize = m_SelectedSize;
		}
		//GameObject obj = data.selectedObject;
		//TMP_Text text = data.selectedObject.GetComponentInChildren<TMP_Text>();
		//text.color = m_SwitchToColour;
	}
	public void UIHoverExit(BaseEventData data)
	{
		if (data != null)
		{
			GameObject obj = data.selectedObject.gameObject;
			Debug.Log(obj.name);
			TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
			text.color = m_DefaultColour;
			text.fontSize = m_DefaultSize;
		}
	}
}
