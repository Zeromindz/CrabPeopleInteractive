using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A modular type script that can beattached to any object to disable
/// </summary>
public class ItemPickup : MonoBehaviour
{  
    /// <summary>
    /// Called when the player collides with a pickup
    /// Disables current object
    /// </summary>
    public void OnPickup()
    {
        gameObject.SetActive(false);
    }
}
