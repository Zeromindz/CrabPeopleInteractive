using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public Animator m_Animator;
    private bool _opened = false;

    // Start is called before the first frame update
    void Start()
    {
        //m_Animator = GetComponentInChildren<Animator>();
        m_Animator.speed = 0;
        //m_Animator.SetBool("Open", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("The fooking player triggered me!");
        if (other.gameObject.CompareTag("Player") && !_opened)
        {
            //m_Animator.SetBool("Open", true);
            m_Animator.Play("Open");

            _opened = true;
        }
    }

    public void Open()
    {
        m_Animator.Play("Open");
        m_Animator.speed = 1;
    }
}
