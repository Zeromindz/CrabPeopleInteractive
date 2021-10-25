using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class FloatingObject : MonoBehaviour
{
    private Rigidbody m_RigidBody;

    [Header("Floater Settings")]
    [SerializeField] internal float m_Gravity = -9.81f;
    [SerializeField] internal float m_GroundHoverForce = 9.0f;
    [SerializeField] internal float m_GroundHoverHeight = 54.0f;
    private Vector3 m_HitPos;
    

    public bool floating = false;

    private int m_LayerMask;

    private void Start()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        m_LayerMask = 1 << LayerMask.NameToLayer("Pickup");
        m_LayerMask = ~m_LayerMask;

    }

    private void FixedUpdate()
    {

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, m_GroundHoverHeight, m_LayerMask))
        {
            m_RigidBody.AddForceAtPosition(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), transform.position, ForceMode.Acceleration);
        }
        else
        {
            // Apply gravity
            m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);
        }
        Debug.DrawRay(transform.position, -transform.up, Color.green);
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up);
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(m_HitPos, 0.2f); // Draw sphere at the wave hight at the players position

    }
}
