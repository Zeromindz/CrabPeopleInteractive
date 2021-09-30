using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class PlayerCollision : MonoBehaviour
{
    internal PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Picking up object
        if (other.tag == "Pickup")
        {
            playerController.AddPassenger();
            other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }
    }
}
