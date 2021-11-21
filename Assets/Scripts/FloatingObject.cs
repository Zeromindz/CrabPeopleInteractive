using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Script attached to all floating objects, like the pickups
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    private Rigidbody m_RigidBody;

    [Header("Floater Settings")]
    [SerializeField] internal float m_Gravity = -9.81f;             // The value that gravity will be set at
    [SerializeField] internal float m_GroundHoverForce = 9.0f;      // The force applied going against gravity
    [SerializeField] internal float m_GroundHoverHeight = 54.0f;    // The height that is set to determin the amount of force that needs to be applied
    //private Vector3 m_HitPos;

    [SerializeField] private bool floating = false;                 // If the object is floating
    

    private int m_LayerMask;                                        // The layer mask for objects to hover over

    /// <summary>
    /// Called on the first frame
    /// Caches some needed information
    /// </summary>
    private void Start()
    {
        m_RigidBody = gameObject.GetComponent<Rigidbody>();
        m_LayerMask = 1 << LayerMask.NameToLayer("Pickup");
        m_LayerMask = ~m_LayerMask;
    }

    /// <summary>
    /// Called every fixed frame
    /// Adds force to the rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        RaycastHit hit;
        
        // Applies a force to push the rigidbody upwards
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, m_GroundHoverHeight, m_LayerMask))
        {
            m_RigidBody.AddForceAtPosition(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), transform.position, ForceMode.Acceleration);
        }

        // Apply gravity that pulls the rigid body downwards
        else
        {
            m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);
        }

        Debug.DrawRay(transform.position, -transform.up, Color.green);
    }

    /// <summary>
    /// Called when drawing gizmos
    /// Displays some of the debugging information for the ea
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up);
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(m_HitPos, 0.2f); // Draw sphere at the wave hight at the players position
    }
}
