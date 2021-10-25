using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public void OnPickup()
    {
        gameObject.SetActive(false);
    }
}
