using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles spawning and attaching ghosts to the boat

public class PassengerManager : MonoBehaviour
{
    private PoolManager m_PoolManager;
    public List<GameObject> m_AttachPoints;

    private void Start()
    {
        m_PoolManager = PoolManager.m_Instance;

    }

    public void SpawnPassenger()
    {
        GameObject objectToSpawn = m_PoolManager.SpawnFromPool("Passenger", m_AttachPoints[0].transform.position, Quaternion.identity);
        objectToSpawn.GetComponent<SpringJoint>().connectedBody = m_AttachPoints[0].GetComponent<Rigidbody>();
    }
}
