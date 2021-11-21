using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the skybox camera
/// </summary>
public class SkyboxCam : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerCamera = null;   // The player Cameras transform
	[SerializeField] private float m_SkyBoxScale = 16.0f;		// The scale compared to the ral world, 16 = 1/6th scale

	/// <summary>
	/// Called every frame
	/// Updates the cameras rotation and position to mimic the players rotation
	/// </summary>
	void Update()
    {
		transform.rotation = m_PlayerCamera.rotation;
		transform.localPosition = (m_PlayerCamera.position / m_SkyBoxScale);
    }
}
