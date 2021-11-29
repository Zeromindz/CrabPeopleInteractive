using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator.SetBool("Open", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //m_Animator.SetBool("Open", true);
            m_Animator.Play("Open");
        }
    }
}
