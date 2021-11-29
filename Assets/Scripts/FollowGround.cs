using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script component attached to the moving ground
/// </summary>
public class FollowGround : MonoBehaviour
{
    private PortalManager m_PortalManager;

    private GameObject m_Player;    // The player object 
    private Vector3 m_CurrentPos;   // The first position of the player

    private bool m_PlatformHidden = false;

    /// <summary>
    /// Called on first frame
    /// Caches needed information
    /// </summary>
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentPos = transform.position;

        m_PortalManager = PortalManager.m_Instance;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        // Updates the floating grounds position based on the players position
        transform.position = new Vector3(m_Player.transform.position.x, m_CurrentPos.y, m_Player.transform.position.z);
        transform.rotation = Quaternion.Euler(0, m_Player.transform.eulerAngles.y + -45, 0);

        if (m_PortalManager.GetState() != 2)
        {
            if (m_PlatformHidden)
            {
                GetComponent<MeshRenderer>().enabled = true;
                m_PlatformHidden = false;
            } 
        }
        else
        {
            if (!m_PlatformHidden)
            {
                GetComponent<MeshRenderer>().enabled = false;
                m_PlatformHidden = true;
            }
        }

        

    }

    public void DisablePlatform()
    {
        gameObject.SetActive(false);
        m_PlatformHidden = true;
    }
    public void EnablePlatform()
    {
        gameObject.SetActive(true);
        m_PlatformHidden = false;
    }
}
