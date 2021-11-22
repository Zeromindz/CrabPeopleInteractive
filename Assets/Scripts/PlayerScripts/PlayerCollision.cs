using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
	internal PlayerController playerController;
    private SoundManager m_SoundManager;

    Vector3 m_ReflectionDirection;
    private LayerMask m_WallLayer;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        m_SoundManager = SoundManager.Instance;
        m_WallLayer = LayerMask.NameToLayer("Wall");
    }

    /// <summary>
    ///  Handle collisions while the player collider continues to intersect with other collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == m_WallLayer)
        {
            Vector3 impactDirection = collision.GetContact(0).point - transform.position;

            //Vector3 reflectionForce = -impactDirection * collision.impulse.magnitude;

            //Vector3 upwardsForce = Vector3.Dot(collision.impulse, transform.up) * transform.up;
            //playerController.playerMovement.m_RigidBody.AddForce(reflectionForce, ForceMode.Impulse);
            //m_ReflectionDirection = Vector3.Reflect(impactDirection, collision.GetContact(0).normal);

            Vector3 surfaceNormal = collision.GetContact(0).normal;
            Vector3 myDirection = playerController.playerMovement.m_CurrentVel.normalized;

            Vector3 temp = Vector3.Cross(surfaceNormal, myDirection);
            myDirection = Vector3.Cross(temp, surfaceNormal);

            playerController.playerMovement.m_RigidBody.AddForce(myDirection.normalized * 100f, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Handle collisions with colliders depending on the objects tag
    /// </summary>
    /// <param name="other"> Collider being detected in the collision </param>
    private void OnTriggerEnter(Collider other)
    {
        // Hit pick-up object
        if (other.tag == "Pickup")
        {
            Debug.Log("Pickup hit!");
            other.GetComponentInParent<ItemPickup>().OnPickup();
            playerController.AddPassenger();
            if(m_SoundManager)
                SoundManager.Instance.PlayGhostPickupSound(0);
        }
        
        // Hit starting checkpoint
        if(other.tag == "Start")
		{
            GameManager.Instance.StartGame();
		}

        // Hit end checkpoint
        if (other.tag == "End")
        {
            GameManager.Instance.EndGame();
        }

        // Hit intermediary checkpoint
        if(other.tag == "CheckPoint")
		{
            UIController.Instance.GameUI.TakeTime();
          //  other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "Wall")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            if(m_SoundManager)
                SoundManager.Instance.PlayCollisionSound(1);
        }


        if (other.collider.tag == "Obstacle")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            if(m_SoundManager)
                SoundManager.Instance.PlayCollisionSound(0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(transform.position, m_ReflectionDirection * 10f);
    }
}
