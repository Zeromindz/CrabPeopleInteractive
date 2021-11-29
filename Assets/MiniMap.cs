using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
	[SerializeField] private Transform m_player = null;

	[SerializeField] private RectTransform m_MiniMapUI = null;
	[SerializeField] private RectTransform m_MiniMapImageUI = null;
	[SerializeField] private GameObject m_PlayerBlip = null;

	private List<Transform> m_ReplayGhostPoss;
	private List<GameObject> m_ReplayGhostBlips;
	[SerializeField] private GameObject m_ReplayGhostPrefab = null;

	private Camera m_MiniMapCam = null;

	[Header("MiniMap Controlls")]
	[SerializeField] bool m_RealtimeMapChanging = false;
	[SerializeField] bool m_RotateWithPlayer = true;
	[SerializeField] bool m_ShowGhost = false;
	[SerializeField,Range(50, 500)] int m_Zoom = 100;
	[SerializeField, Range(4, 25)] int m_BlipSize = 4;
	[SerializeField,Range(200, 400)] int m_MapSize = 200;

	private Quaternion m_DefaultIconRotation;

	private static MiniMap m_Instance;                       // The current instance of MenuController
	public static MiniMap Instance                           // The public current instance of MenuController
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

		m_ReplayGhostPoss = new List<Transform>();
		m_ReplayGhostBlips = new List<GameObject>();

		m_MiniMapCam = GetComponentInChildren<Camera>();

		m_MiniMapCam.orthographicSize = m_Zoom;

		m_DefaultIconRotation = m_PlayerBlip.transform.rotation;
		Vector3 blipScale = m_PlayerBlip.transform.localScale;

		blipScale.x = m_BlipSize;
		blipScale.y = m_BlipSize;
		m_PlayerBlip.transform.localScale = blipScale;

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
			Vector3 blipScale = m_PlayerBlip.transform.localScale;
			blipScale.x = m_BlipSize;
			blipScale.y = m_BlipSize;
			m_PlayerBlip.transform.localScale = blipScale;

			for (int i = 0; i < m_ReplayGhostPoss.Count; i++)
			{
				m_ReplayGhostBlips[i].transform.localScale = blipScale;
				m_ReplayGhostBlips[i].gameObject.SetActive(m_ShowGhost);
			}


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
		newPos.y = m_player.transform.position.y + 300;
		transform.position = newPos;

		//Changes Player Blip Position
		Vector3 newBlipPos = m_player.position;
		newBlipPos.y = m_PlayerBlip.transform.position.y;
		m_PlayerBlip.transform.position = newBlipPos;

		// Changes Ghost Blip Position
		
		for(int i = 0; i < m_ReplayGhostPoss.Count; i++)
		{
			Vector3 newGhostBlipPos = m_ReplayGhostPoss[i].position;
			newGhostBlipPos.y = m_ReplayGhostPoss[i].position.y;
			m_ReplayGhostBlips[i].transform.position = newGhostBlipPos;
			m_ReplayGhostBlips[i].transform.rotation = Quaternion.Euler(m_DefaultIconRotation.eulerAngles.x, 0.0f, m_DefaultIconRotation.eulerAngles.z + m_ReplayGhostBlips[i].transform.eulerAngles.y);
		}

		m_PlayerBlip.transform.rotation = Quaternion.Euler(m_DefaultIconRotation.eulerAngles.x, 0.0f, m_DefaultIconRotation.eulerAngles.z + m_player.eulerAngles.y);


		if (m_RotateWithPlayer)
		{
			transform.rotation = Quaternion.Euler(90.0f, m_player.eulerAngles.y, 0.0f);
		}
	}

	// Adds a ghost
	public void AddGhost(Transform ghostPos)
	{
		m_ReplayGhostPoss.Add(ghostPos);
		GameObject obj = Instantiate(m_ReplayGhostPrefab);
		m_ReplayGhostBlips.Add(obj);
	}

	public void RemoveGhosts()
	{
		m_ReplayGhostPoss.Clear();
		m_ReplayGhostBlips.Clear();
	} 
}
