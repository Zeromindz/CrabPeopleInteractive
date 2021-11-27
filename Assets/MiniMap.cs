using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
	[SerializeField] private Transform m_player = null;
	[SerializeField] private Transform m_PlayerBlip = null;

	//[SerializeField] private Transform m_Ghost = null;
	[SerializeField] private Transform m_GhostBlip = null;

	[SerializeField] private RectTransform m_MiniMapUI = null;
	[SerializeField] private RectTransform m_MiniMapImageUI = null;

	private Camera m_MiniMapCam = null;

	[Header("MiniMap Controlls")]
	[SerializeField] bool m_RealtimeMapChanging = false;
	[SerializeField] bool m_RotateWithPlayer = true;
	[SerializeField] bool m_ShowGhost = false;
	[SerializeField,Range(50, 150)] int m_Zoom = 100;
	[SerializeField, Range(4, 8)] int m_BlipSize = 4;
	[SerializeField,Range(200, 400)] int m_MapSize = 200;

	private Quaternion m_DefaultIconRotation;
	private void Awake()
	{
		m_MiniMapCam = GetComponentInChildren<Camera>();

		m_MiniMapCam.orthographicSize = m_Zoom;

		m_DefaultIconRotation = m_PlayerBlip.rotation;
		Vector3 blipScale = m_PlayerBlip.localScale;

		blipScale.x = m_BlipSize;
		blipScale.y = m_BlipSize;
		m_PlayerBlip.localScale = blipScale;
		m_GhostBlip.localScale = blipScale;

		m_GhostBlip.gameObject.SetActive(m_ShowGhost);

		Vector2 rectSizeDelta = Vector2.zero;
		rectSizeDelta.x = m_MapSize;
		rectSizeDelta.y = m_MapSize;
		m_MiniMapUI.sizeDelta = rectSizeDelta;

		Vector3 newMiniMapPos = Vector3.zero;
		newMiniMapPos.x = -(0.5f * m_MapSize);
		newMiniMapPos.y = -(0.5f * m_MapSize);
		
		m_MiniMapUI.anchoredPosition = newMiniMapPos;
		m_MiniMapImageUI.anchoredPosition = newMiniMapPos; 
		
		
	}

	public void FixedUpdate()
	{
		if (m_RealtimeMapChanging)
		{
			m_MiniMapCam.orthographicSize = m_Zoom;
			Vector3 blipScale = m_PlayerBlip.localScale;
			blipScale.x = m_BlipSize;
			blipScale.y = m_BlipSize;
			m_PlayerBlip.localScale = blipScale;
			m_GhostBlip.localScale = blipScale;
			m_GhostBlip.gameObject.SetActive(m_ShowGhost);

			Vector2 rectSizeDelta = Vector2.zero;
			rectSizeDelta.x = m_MapSize;
			rectSizeDelta.y = m_MapSize;
			m_MiniMapUI.sizeDelta = rectSizeDelta;

			Vector2 imageRectSizeDelta = Vector2.zero;
			imageRectSizeDelta.x = m_MapSize * 2;
			imageRectSizeDelta.y = m_MapSize * 2;
			m_MiniMapImageUI.sizeDelta = imageRectSizeDelta;

			Vector3 newMiniMapPos = Vector3.zero;
			newMiniMapPos.x = -(0.5f * m_MapSize);
			newMiniMapPos.y = -(0.5f * m_MapSize);

			m_MiniMapUI.anchoredPosition = newMiniMapPos;
			m_MiniMapImageUI.anchoredPosition = newMiniMapPos;
		}

		// Changes Camera Position
		Vector3 newPos = m_player.position;
		newPos.y = transform.position.y;
		transform.position = newPos;

		// Changes Player Blip Position
		Vector3 newBlipPos = m_player.position;
		newBlipPos.y = m_PlayerBlip.position.y;
		m_PlayerBlip.position = newBlipPos;

		//// Changes Ghost Blip Position
		//Vector3 newGhostBlipPos = m_Ghost.position;
		//newGhostBlipPos.y = m_GhostBlip.position.y;
		//m_GhostBlip.position = newGhostBlipPos;

		m_PlayerBlip.rotation = Quaternion.Euler(m_DefaultIconRotation.eulerAngles.x, 0.0f, m_DefaultIconRotation.eulerAngles.z + m_player.eulerAngles.y);
		//m_GhostBlip.rotation = Quaternion.Euler(m_DefaultIconRotation.eulerAngles.x, 0.0f, m_DefaultIconRotation.eulerAngles.z + m_Ghost.eulerAngles.y);

		if (m_RotateWithPlayer)
		{
			transform.rotation = Quaternion.Euler(90.0f, m_player.eulerAngles.y, 0.0f);
		}
	}
}
