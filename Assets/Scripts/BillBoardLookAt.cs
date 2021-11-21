using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillBoardLookAt : MonoBehaviour
{
    [SerializeField] Camera m_Camera = null;            // The camer which the billboard will rotate to face (Player Camera)
    [SerializeField] RectTransform m_nameTag = null;    // The rect transform of the nametag label

    Vector2 originalSize = Vector2.zero;                // The cached original size for the nametag
    
    /// <summary>
    /// Called when the script is created
    /// </summary>
	private void Awake()
	{
        originalSize = m_nameTag.sizeDelta;	
	}

	/// <summary>
    /// Called each frame
    /// </summary>
	void Update()
    {      
        // Making the billboard rotate to look at the camera
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);

        // storing position variables
        Vector2 camPos = m_Camera.transform.position;
        Vector2 nameTagPos = this.transform.position;

        // getting the distance between the two points
        float distance = Vector2.Distance(camPos, nameTagPos);

        // Deactivating the label if the player gets too far away
        if (distance < 0)
		{
            // Makes it always positive
            distance = distance * -1;
		}

        if(distance > 1000)
		{
            m_nameTag.gameObject.SetActive(false);
		}

        // Changes the size of the label depending on the distance
        else if(distance <= 1000 &&  distance > 3)
		{
            m_nameTag.gameObject.SetActive(true);
            Vector2 newSize = m_nameTag.sizeDelta;
            newSize.x = originalSize.x + (3 * distance);
            newSize.y = originalSize.y + (3 * distance);
            m_nameTag.sizeDelta = newSize;
        }
    }
}
