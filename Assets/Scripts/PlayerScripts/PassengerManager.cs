using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles spawning and attaching ghosts to the boat

public class PassengerManager : MonoBehaviour
{
    private PoolManager m_PoolManager;
    public List<GameObject> m_AttachPoints;
    public List<GameObject> m_PassengerObjects;
    public List<GameObject> m_OldAttachPoints;

    private void Awake()
    {
        m_OldAttachPoints = m_AttachPoints;
    }

    private void Start()
    {
        m_PoolManager = PoolManager.m_Instance;


    }

    public void SpawnPassenger()
    {
        // Choose a random index to select attach point from list
        int attachPointIndex = Random.Range(0, m_AttachPoints.Count);
        var selectedAttachPoint = m_AttachPoints[attachPointIndex];
        // Spawn passenger at the attach point with a random rotation
        GameObject objectToSpawn = m_PoolManager.SpawnFromPool("Passenger", selectedAttachPoint.transform.position, Quaternion.Euler(new Vector3(Random.insideUnitSphere.x, Random.insideUnitSphere.y, Random.insideUnitSphere.z)));
        objectToSpawn.transform.SetParent(selectedAttachPoint.transform);

        // Set the passenger's connected rigidbody to be the selected attach point
        //objectToSpawn.GetComponentInChildren<SpringJoint>().connectedBody = selectedAttachPoint.GetComponent<Rigidbody>();
        //GameObject hand = objectToSpawn.transform.Find("GFX/Ghost_With_Rig 1/Root/Arm_L/Arm_L_end").gameObject;
        //hand.transform.position = selectedAttachPoint.transform.position;
        GameObject grabPoint = objectToSpawn.transform.Find("Grab Point").gameObject;

        m_AttachPoints.Add(grabPoint);
        m_PassengerObjects.Add(objectToSpawn);

        // Remove the attach point from the list to avoid double attaches
        //m_AttachPoints.RemoveAt(attachPointIndex);
        // Cache the grab point on the passenger game object
        // Cache the grab point on the passenger game object



        // Unused
        //hand.transform.SetParent(selectedAttachPoint.transform);


        // Add object to passenger list (for resetting)
    }

    public void ResetPassengers()
    {
        for (int i = m_PassengerObjects.Count - 1; i >= 0; i--)
        {
            m_PassengerObjects[i].SetActive(false);

        }

        m_PassengerObjects.Clear();
        m_AttachPoints = m_OldAttachPoints;
        //m_AttachPoints.Clear();
        //m_AttachPoints.Add(gameObject.transform.Find("Grab Point").gameObject);
        Debug.Log("");
    }
}
