using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles spawning and attaching ghosts to the boat

public class PassengerManager : MonoBehaviour
{
    private PoolManager m_PoolManager;
    public List<GameObject> m_AttachPoints;
    public List<GameObject> m_PassengerObjects;

    private void Start()
    {
        m_PoolManager = PoolManager.m_Instance;

    }

    public void SpawnPassenger()
    {
        int attachPointIndex = Random.Range(0, m_AttachPoints.Count);

        GameObject objectToSpawn = m_PoolManager.SpawnFromPool("Passenger", m_AttachPoints[attachPointIndex].transform.position, Quaternion.identity);
        m_PassengerObjects.Add(objectToSpawn);
        objectToSpawn.GetComponentInChildren<SpringJoint>().connectedBody = m_AttachPoints[attachPointIndex].GetComponent<Rigidbody>();

        //GameObject grabPoint = objectToSpawn.transform.GetChild(1).gameObject;
        GameObject grabPoint = objectToSpawn.transform.Find("Grab Point").gameObject;
        m_AttachPoints.Add(grabPoint);

        GameObject hand = objectToSpawn.transform.Find("GFX/Root/Arm_L/Hand_L").gameObject;
        hand.transform.position = m_AttachPoints[attachPointIndex].transform.position;
        hand.transform.SetParent(m_AttachPoints[attachPointIndex].transform);
        
        m_AttachPoints.RemoveAt(attachPointIndex);

        if(m_PassengerObjects.Count == 10)
        {
            ResetPassengers();
        }
    }

    public void ResetPassengers()
    {
        for(int i = m_PassengerObjects.Count - 1; i >= 0; i--)
        {
            m_PassengerObjects[i].SetActive(false);
            
        }

        m_PassengerObjects.Clear();
        m_AttachPoints.Clear();
        m_AttachPoints.Add(gameObject.transform.Find("Grab Point").gameObject);
    }
}
