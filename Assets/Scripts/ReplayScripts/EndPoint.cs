using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to add behaviour to the end point
/// </summary>
public class EndPoint : MonoBehaviour
{
	/// <summary>
	/// Called when the trigger is entered
	/// </summary>
	/// <param name="other"> The collider of the item hit</param>
	private void OnTriggerEnter(Collider other)
	{
        Debug.Log(gameObject.tag + " Collided with: " + other.tag);

        if(other.tag.Equals("Player"))
		{
            Debug.Log("Player reached the end");
            TempGameManager.Instance.RestartGame();
		}
	}
}
