using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillBoardLookAt : MonoBehaviour
{
    [SerializeField] Camera m_Camera = null;
    [SerializeField] RectTransform m_nameTag = null;

    Vector2 originalSize = Vector2.zero;
	private void Awake()
	{
        originalSize = m_nameTag.sizeDelta;	
	}

	// Update is called once per frame
	void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);

        Vector2 camPos = m_Camera.transform.position;
        Vector2 nameTagPos = this.transform.position;

        float distance = Vector2.Distance(camPos, nameTagPos);
        if (distance < 0)
		{
            distance = distance * -1;
		}

        if(distance > 1000)
		{
            m_nameTag.gameObject.SetActive(false);
		}

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
