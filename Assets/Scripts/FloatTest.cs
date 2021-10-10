using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class FloatTest : MonoBehaviour
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
        Debug.Log("helloWorld");
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask);
        Debug.DrawRay(transform.position, -transform.up, Color.green);
        /*
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
        m_RigidBody.AddForce(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), ForceMode.Acceleration);
        Debug.DrawRay(transform.position, -Vector3.up, Color.green);
        if (hit.distance >  m_GroundHoverHeight)
        {
            // Hovering
            m_RigidBody.AddForce(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), ForceMode.Acceleration);
            floating = true;

        }
        else
        {
            // Apply gravity
            m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);

            floating = false;
        }
        */
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up);
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(m_HitPos, 0.2f); // Draw sphere at the wave hight at the players position

    }
}
