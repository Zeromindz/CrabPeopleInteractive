﻿using System.Collections;
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
            //Vector3 impactDirection = collision.transform.position - transform.position;
            //
            //Vector3 reflectionForce = -impactDirection * collision.impulse.magnitude;
            //
            ////Vector3 upwardsForce = Vector3.Dot(collision.impulse, transform.up) * transform.up;
            //playerController.playerMovement.m_RigidBody.AddForce(reflectionForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Picking up object
        if (other.tag == "Pickup")
        {
            playerController.AddPassenger();
            other.GetComponentInParent<ItemPickup>().OnPickup();
            SoundManager.Instance.PlayRandomGhostPickupSound();
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
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "Wall")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            SoundManager.Instance.PlayCollisionSound(1);
        }


        if (other.collider.tag == "Obstacle")
        {
            Debug.Log("Collided with: " + other.collider.tag);
            SoundManager.Instance.PlayCollisionSound(0);
        }
    }
}
