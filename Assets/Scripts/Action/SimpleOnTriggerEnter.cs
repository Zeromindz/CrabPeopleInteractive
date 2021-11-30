using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;

    private void OnTriggerEnter(Collider other)
    {
        _event.Invoke();
    }
}
