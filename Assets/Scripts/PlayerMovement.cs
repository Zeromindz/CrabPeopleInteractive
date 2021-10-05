using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]

//==================================================
// Handles moving the player through the rigidbody
//______________________________________________/

public class PlayerMovement : MonoBehaviour
{
    internal PlayerController m_PlayerController;
    
    private Rigidbody m_RigidBody; // Rigidbody attached to the boat

    [SerializeField] internal bool m_InWater = false;
    [SerializeField] internal float m_HorsePower = 30.0f;
    [SerializeField] internal float m_SteeringTorque = 8.0f;
    [SerializeField] internal float m_LevelingForce = 2.0f;
    [SerializeField] internal float m_Gravity = -9.81f;

    [Space(10)]
    [Header("Hover Settings")]
    [SerializeField] internal float m_HoverForce = 9.0f;
    [SerializeField] internal float m_HoverHeight = 4.0f;

    [Space(10)]
    [Header("Float Settings")]
    public float m_DepthBeforeSubmerged = 1.0f;
    public float m_DisplacementAmount = 3.0f;
    public int m_FloaterCount = 1;
    public float m_WaterDrag = 0.99f;
    public float m_WaterAngularDrag = 0.5f;

    [SerializeField] private GameObject[] m_HoverPoints;
    [SerializeField] private Transform m_EngineTransform;
    private int m_LayerMask;

    private float m_CurrentThrust = 0.0f;
    private float m_CurrentSteer = 0.0f;

    private Vector3 m_LastPosition;
    private float m_CurrentSpeed;

    public float GetSpeed() { return m_CurrentSpeed; }

    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_RigidBody = GetComponent<Rigidbody>();

        m_LayerMask = 1 << LayerMask.NameToLayer("Character");
        m_LayerMask = ~m_LayerMask;

    }

    void Update()
    {
        //Check the input manager for current input
        m_CurrentThrust = m_PlayerController.playerInput.GetVertical();
        m_CurrentSteer = m_PlayerController.playerInput.GetHorizontal();

        CalculateSpeed();

        Debug.Log("Current thrust = " + m_CurrentThrust);
    }

    void FixedUpdate()
    {
        // Apply gravity
        m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);

        CheckIfSubmerged();

        Accelerate();
        Steer();
    }

    // Controls the acceleration of the boat
    public void Accelerate()
    {
        Vector3 forward = m_RigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        if (Mathf.Abs(m_CurrentThrust) > 0)
        {
            m_RigidBody.AddForce(forward * m_CurrentThrust * m_HorsePower);
        }
        else
        {
            m_RigidBody.AddForce(forward * m_CurrentThrust * m_HorsePower);
        }
        Debug.Log(m_CurrentThrust);
    }

    // Controls turning
    public void Steer()
    {
        m_RigidBody.AddRelativeTorque(Vector3.up * m_CurrentSteer * m_SteeringTorque);
        Debug.Log(m_CurrentSteer);
    }

    // Only works with wave manager active
    private void CheckIfSubmerged()
    {
        // Raycast down from each floater
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_HoverHeight, m_LayerMask))
            {
                //======================================
                // Hovering
                //____________________________________/
                m_RigidBody.AddForceAtPosition(Vector3.up * m_HoverForce * (1.0f - (hit.distance / m_HoverHeight)), hoverPoint.transform.position);

                 // level out hoverpoints
        
                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_LevelingForce, hoverPoint.transform.position);
                }
                else
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_LevelingForce, hoverPoint.transform.position);
                }
                

                Debug.Log("Hovering");
            }
            else
            {
                //======================================
                // Floating
                //____________________________________/
                float waveHeight = WaveManager.m_Instance.GetWaveHeight(transform.position);
                // Check if the floaters y position is below the waveheight
                if (hoverPoint.transform.position.y < waveHeight)
                {
                    // How much is the rigidbody submerged
                    float displacementMultiplier = Mathf.Clamp01((waveHeight - hoverPoint.transform.position.y) / m_DepthBeforeSubmerged) * m_DisplacementAmount;
                    // Add a force equal to gravity multiplied by the displacement multiplier, using the acceleration forcemode.
                    m_RigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(m_Gravity) * displacementMultiplier, 0f), hoverPoint.transform.position, ForceMode.Acceleration);
                    m_RigidBody.AddForce(displacementMultiplier * -m_RigidBody.velocity * m_WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                    m_RigidBody.AddTorque(displacementMultiplier * -m_RigidBody.angularVelocity * m_WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                    
                    Debug.Log("Floating");
                }
            }

        }
    }

    public void Float()
    {
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            float waveHeight = WaveManager.m_Instance.GetWaveHeight(transform.position);
            // Check if the floaters y position is below the waveheight
            if (m_HoverPoints[i].transform.position.y < waveHeight)
            {
                // How much is the rigidbody submerged
                float displacementMultiplier = Mathf.Clamp01((waveHeight - m_HoverPoints[i].transform.position.y) / m_DepthBeforeSubmerged) * m_DisplacementAmount;
                // Add a force equal to gravity multiplied by the displacement multiplier, using the acceleration forcemode.
                m_RigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), m_HoverPoints[i].transform.position, ForceMode.Acceleration);
                m_RigidBody.AddForce(displacementMultiplier * -m_RigidBody.velocity * m_WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                m_RigidBody.AddTorque(displacementMultiplier * -m_RigidBody.angularVelocity * m_WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }

    public void Hover()
    {
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            Debug.Log(m_HoverPoints[i].name + " hovering");
            // If a raycast down hits, apply an upqwards hoverforce based on the distance to the hit
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_HoverHeight, m_LayerMask))
            {
                m_RigidBody.AddForceAtPosition(Vector3.up * m_HoverForce * (1.0f - (hit.distance / m_HoverHeight)), hoverPoint.transform.position);

            }
            // Otherwise, level out hoverpoints
            else
            {
                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_LevelingForce, hoverPoint.transform.position);
                }
                else
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_LevelingForce, hoverPoint.transform.position);
                }
            }
        }
    }

    private void CalculateSpeed()
    {
        // Calculate speed in meters per second
        m_CurrentSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        //Save the position for the next update
        m_LastPosition = transform.position;
    }

    void OnDrawGizmos()
    {
        // Copying racast conditions from hover method to draw gizmos
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_HoverHeight, m_LayerMask))
            {
                // Line from hoverpoint to hit and draws a sphere
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
            else
            {
                // Line from hoverpoint to max distance down from RB
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position, hoverPoint.transform.position - Vector3.up * m_HoverHeight);
            }
        }

        float waveYPos = WaveManager.m_Instance.GetWaveHeight(m_EngineTransform.position);
        if (m_EngineTransform.position.y < waveYPos)
        {
            
                
        }


    }
}
