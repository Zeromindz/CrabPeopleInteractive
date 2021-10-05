using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    internal PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        // Picking up object
        if (other.tag == "Pickup")
        {
            other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }
    }
}
