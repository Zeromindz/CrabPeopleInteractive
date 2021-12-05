using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
	internal PlayerController playerController;
    private SoundManager m_SoundManager;

    Vector3 m_ReflectionDirection;
    private LayerMask m_WallLayer;

    public float m_WallBounceForce = 50f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        m_SoundManager = SoundManager.Instance;
        m_WallLayer = LayerMask.NameToLayer("Wall");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == m_WallLayer)
        {
            //Vector3 playerVelocity = playerController.playerMovement.m_CurrentVel;
            //
            //Vector3 surfaceNormal = other.GetContact(0).normal;
            //Vector3 myDirection = playerVelocity.normalized;
            //
            //Vector3 temp = Vector3.Cross(surfaceNormal, myDirection);
            //myDirection = Vector3.Cross(temp, surfaceNormal);
            //
            //Vector3 newDirection = myDirection + surfaceNormal;
            //
            //playerController.playerMovement.m_RigidBody.AddForce(myDirection.normalized * playerVelocity.magnitude, ForceMode.Impulse);
           

        }

        if (other.collider.tag == "Wall")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            if (m_SoundManager)
                SoundManager.Instance.PlayCollisionSound(1);
        }

        if (other.collider.tag == "Obstacle")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            if (m_SoundManager)
                SoundManager.Instance.PlayCollisionSound(0);
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
                SoundManager.Instance.PlayRandomGhostPickupSound();
        }
        
        // Hit starting checkpoint
        if(other.tag == "Start")
		{
            if (GameManager.Instance.State == GameState.NotInRun)
            {
                GameManager.Instance.StartGame();
                UIController.Instance.GameUI.ShowTrickTip();
                UIController.Instance.GameUI.SetActive(true);
            }
		}

        // Hit end checkpoint
        if (other.tag == "End")
        {
            if(GameManager.Instance)
            {
                GameManager.Instance.EndGame();
            }
        }

        // Hit intermediary checkpoint
        if(other.tag == "CheckPoint")
		{
            UIController.Instance.GameUI.TakeTime();
          //  other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(transform.position, m_ReflectionDirection * 10f);
    }
}
