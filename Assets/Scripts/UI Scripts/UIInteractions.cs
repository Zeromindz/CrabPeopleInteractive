using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Color m_DefaultColour;
	[SerializeField] private Color m_SwitchToColour;
	[SerializeField] private int m_DefaultSize = 30;
	[SerializeField] private int m_SelectedSize = 50;

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameObject obj = this.gameObject;
		Debug.Log(obj.name);
		TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
		text.color = m_SwitchToColour;
		text.fontSize = m_SelectedSize;
		throw new System.NotImplementedException();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameObject obj = this.gameObject;
		Debug.Log(obj.name);
		TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
		text.color = m_DefaultColour;
		text.fontSize = m_DefaultSize;
		throw new System.NotImplementedException();
	}
}
