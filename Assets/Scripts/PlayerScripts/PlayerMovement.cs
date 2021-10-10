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
    

    [Header("Boat Settings")]
    [SerializeField] internal bool m_Airbourne = false;
    [SerializeField] internal float m_HorsePower = 30.0f;
    [SerializeField] internal float m_MaxSpeed = 50.0f;
    [SerializeField] internal float m_SteeringTorque = 8.0f;
    [SerializeField] internal float m_LevelingForce = 2.0f;
    [SerializeField] internal float m_VelocitySlowFactor = 0.95f;
    [SerializeField] internal float m_Gravity = -9.81f;
    [SerializeField] private Transform m_CenterOfMass;
    private int m_LayerMask;
    private float m_CurrentThrust = 0.0f;
    private float m_CurrentSteer = 0.0f;
    private float m_CurrentSpeed = 0f;
    public float GetSpeed() { return m_CurrentSpeed; }
    private Vector3 m_LastPosition;

    // Floating behaviour works in two ways, one which occurs when solid gound is hit below the player, and one when submerged in water
    // Each hoverpoint calculates an upwards force based on either a raycast hit dist or a rigidbody submerged percentage
    [Space(10)]
    [SerializeField] private GameObject[] m_HoverPoints;
    [SerializeField] private Transform m_EngineTransform;

    [Space(10)]
    [Header("Grounded Behaviour Settings")]
    [SerializeField] internal float m_GroundHoverForce = 9.0f;
    [SerializeField] internal float m_GroundHoverHeight = 4.0f;

    [Space(10)]
    [Header("Floating Behaviour Settings")]
    public float m_DepthBeforeSubmerged = 1.0f; // Broken
    public float m_DisplacementAmount = 3.0f;
    public float m_WaterDrag = 0.99f;
    public float m_WaterAngularDrag = 0.5f;


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

       // Debug.Log("Current thrust = " + m_CurrentThrust);
    }

    void FixedUpdate()
    {
        m_RigidBody.centerOfMass = m_CenterOfMass.localPosition;

        // Apply gravity
        m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);
     
        // Clamp Speed
        if(m_RigidBody.velocity.magnitude > m_MaxSpeed)
        {
            m_RigidBody.velocity = m_RigidBody.velocity.normalized * m_MaxSpeed;
        }

        if (m_PlayerController.playerInput.GetVertical() < 0.01f)
        {
            m_RigidBody.velocity = m_RigidBody.velocity * m_VelocitySlowFactor;
        }
        
        ApplyForcetoPoints();
       
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
            m_RigidBody.AddForce(forward * m_CurrentThrust * m_HorsePower, ForceMode.Acceleration);
            if(m_Airbourne)
            {
                // Remove thrust when airbourne
                m_CurrentThrust = 0f;
            }
        }
        //Debug.Log(m_CurrentThrust);
    }

    // Controls turning
    public void Steer()
    {
        m_RigidBody.AddRelativeTorque(Vector3.up * m_CurrentSteer * m_SteeringTorque);
        //Debug.Log(m_CurrentSteer);
    }

    // Only works with wave manager active
    private void ApplyForcetoPoints()
    {
        // Raycast down from each floater
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            bool pointSubmerged = hoverPoint.GetComponent<Floater>().GetSubmerged();
            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
            {
                //======================================
                // Hovering
                //____________________________________/
                m_RigidBody.AddForceAtPosition(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), hoverPoint.transform.position, ForceMode.Acceleration);

                // level out hoverpoints

                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
                }
                else
                {
                    m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
                }

                m_Airbourne = false;
              //  Debug.Log("Hovering");
            }
            else
            {
                m_Airbourne = !pointSubmerged;
            }
            

            //if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
            //{
            //    //======================================
            //    // Hovering
            //    //____________________________________/
            //    m_RigidBody.AddForceAtPosition(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), hoverPoint.transform.position, ForceMode.Acceleration);

            //     // level out hoverpoints

            //    if (transform.position.y > hoverPoint.transform.position.y)
            //    {
            //        m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
            //    }
            //    else
            //    {
            //        m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
            //    }

            //    m_Airbourne = false;
            //    Debug.Log("Hovering");
            //}
            //else
            //{
            //    //======================================
            //    // Floating
            //    //____________________________________/
            //    float waveHeight = WaveManager.m_Instance.GetWaveHeight(transform.position);
            //    // Check if the floaters y position is below the waveheight, in which case we consider it underwater
            //    if (hoverPoint.transform.position.y < waveHeight)
            //    {
            //        // How much is the rigidbody submerged
            //        float displacementMultiplier = Mathf.Clamp01(-hoverPoint.transform.position.y / m_DepthBeforeSubmerged) * m_DisplacementAmount; // clamped between 0-1 because bouyancy force remains the same regardless of how submerged the object is
            //        // Add a force equal to gravity multiplied by the displacement multiplier, using the acceleration forcemode.
            //        m_RigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(m_Gravity) * displacementMultiplier, 0f), hoverPoint.transform.position, ForceMode.Acceleration);
            //        m_RigidBody.AddForce(displacementMultiplier * -m_RigidBody.velocity * m_WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //        m_RigidBody.AddTorque(displacementMultiplier * -m_RigidBody.angularVelocity * m_WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

            //        m_Airbourne = false;
            //        Debug.Log("Floating");
            //    }
            //    else
            //    {
            //        m_Airbourne = true;
            //    }
            //}

        }
    }


    private void CalculateSpeed()
    {

        m_CurrentSpeed = m_RigidBody.velocity.magnitude;

        //// Calculate speed in meters per second
        //Vector3 yRemoved = transform.position - m_LastPosition;
        //yRemoved.y = 0;
        //float speed = yRemoved.magnitude / Time.deltaTime;
        //m_CurrentSpeed = speed;
        ////Save the position for the next update
        //m_LastPosition = transform.position;
    }

    void OnDrawGizmos()
    {
        // Copying racast conditions from hover method to draw gizmos
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
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
                Gizmos.DrawLine(hoverPoint.transform.position, hoverPoint.transform.position - Vector3.up * m_GroundHoverHeight);
            }
        }

        Gizmos.color = Color.cyan;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            Gizmos.DrawWireSphere(hoverPoint.transform.position, 0.3f);
           
        }

        if(m_CenterOfMass)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_CenterOfMass.position, 0.5f);

        }
 
    }
}
