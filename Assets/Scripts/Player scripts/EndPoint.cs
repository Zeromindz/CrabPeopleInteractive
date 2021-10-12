using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
