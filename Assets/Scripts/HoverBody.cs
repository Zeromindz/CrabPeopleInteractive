using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class HoverBody : MonoBehaviour
{

    [Header("Hover Settings")]
    [SerializeField] private int m_LayerMask;
    public Rigidbody m_RigidBody;
    public GameObject[] m_HoverPoints;

    [SerializeField] private float m_HoverHeight = 3.0f;
    [SerializeField] private float m_HoverForce = 5.0f;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();

        m_LayerMask = 1 << LayerMask.NameToLayer("Character");
        m_LayerMask = ~m_LayerMask;
    }


    void Update()
    {

    }

    //private void FixedUpdate()
    //{


    //    RaycastHit hit;
    //    for (int i = 0; i < m_HoverPoints.Length; i++)
    //    {
    //        var hoverPoint = m_HoverPoints[i];

    //        if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_HoverHeight, m_LayerMask))
    //        {
    //            m_RigidBody.AddForceAtPosition(Vector3.up * m_HoverForce * (1.0f - (hit.distance / m_HoverHeight)), hoverPoint.transform.position);
    //        }
    //        else
    //        {
    //            if (transform.position.y > hoverPoint.transform.position.y)
    //            {
    //                m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_HoverForce, hoverPoint.transform.position);
    //            }
    //            else
    //            {
    //                m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_HoverForce, hoverPoint.transform.position);
    //            }
    //        }
    //    }



    //}

    void FixedUpdate()
    {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                                -Vector3.up, out hit,
                                m_HoverHeight,
                                m_LayerMask))
                m_RigidBody.AddForceAtPosition(Vector3.up
                  * m_HoverForce
                  * (1.0f - (hit.distance / m_HoverHeight)),
                                          hoverPoint.transform.position);
            else
            {
                if (transform.position.y > hoverPoint.transform.position.y)
                    m_RigidBody.AddForceAtPosition(
                      hoverPoint.transform.up * m_HoverForce,
                      hoverPoint.transform.position);
                else
                    m_RigidBody.AddForceAtPosition(
                      hoverPoint.transform.up * -m_HoverForce,
                      hoverPoint.transform.position);
            }
        }

        void OnDrawGizmos()
        {

            //  Hover Force
            RaycastHit debugHit;
            for (int i = 0; i < m_HoverPoints.Length; i++)
            {
                var hoverPoint = m_HoverPoints[i];
                if (Physics.Raycast(hoverPoint.transform.position,
                                    -Vector3.up, out debugHit,
                                    m_HoverHeight,
                                    m_LayerMask))
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(hoverPoint.transform.position, debugHit.point);
                    Gizmos.DrawSphere(hit.point, 0.5f);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hoverPoint.transform.position,
                                   hoverPoint.transform.position - Vector3.up * m_HoverHeight);
                }
            }
        }
    }
}

