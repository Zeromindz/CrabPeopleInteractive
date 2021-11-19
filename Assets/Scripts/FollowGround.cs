using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script component attached to the moving ground
/// </summary>
public class FollowGround : MonoBehaviour
{
    private GameObject m_Player;    // The player object 
    private Vector3 m_CurrentPos;   // The first position of the player

    /// <summary>
    /// Called on first frame
    /// Caches needed information
    /// </summary>
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentPos = transform.position;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        // Updates the floating grounds position based on the players position
        transform.position = new Vector3(m_Player.transform.position.x, m_CurrentPos.y, m_Player.transform.position.z);
    }
}
