using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollision : MonoBehaviour
{
    internal PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector3 upwardsForce = Vector3.Dot(collision.impulse, transform.up) * transform.up;
            playerController.playerMovement.m_RigidBody.AddForce(-upwardsForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Picking up object
        if (other.tag == "Pickup")
        {
            playerController.AddPassenger();
            other.GetComponentInParent<ItemPickup>().OnPickup();
        }

        if(other.tag == "Start")
		{
            GameManager.Instance.StartGame();
           // other.gameObject.GetComponent<ItemPickup>().OnPickup();
		}

        if (other.tag == "End")
        {
            GameManager.Instance.EndGame();
            Debug.Log("End Hit");
           // other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }

        if(other.tag == "CheckPoint")
		{
            GameManager.Instance.CheckPointHit();
          //  other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }

        if (other.tag == "Wall")
        {
        //    Vector3 collision = other.transform.forward.normalized;
     //       PlayerMovement.Instance.BounceOffWall(collision); 
        }
    }
}
