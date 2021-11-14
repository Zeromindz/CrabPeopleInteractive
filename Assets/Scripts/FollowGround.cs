using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGround : MonoBehaviour
{
    private GameObject m_Player;
    private Vector3 m_CurrentPos;
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentPos = transform.position;
    }


    void Update()
    {
        
        transform.position = new Vector3(m_Player.transform.position.x, m_CurrentPos.y, m_Player.transform.position.z);
    }
}
