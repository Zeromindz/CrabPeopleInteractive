﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//==================================================
// Handles moving the player through the rigidbody
//______________________________________________/

public class PlayerMovement : MonoBehaviour
{
    private Vector2 m_Movement = Vector2.zero;
    private Vector2 m_MovementInput;
    private bool m_IsSpacePressed;
    private bool m_IsShiftPressed;

    [Header("Drive Settings")]
    public float m_CurrentSpeed;
    [Space(10)]

    [SerializeField] internal float m_AccelerationForce = 60.0f;       // Force generated by the engine
    [SerializeField] internal float m_BoostForce = 20.0f;              // Boost force
    [SerializeField] internal float m_MaxSpeed = 100.0f;               // Max speed without boost
    [SerializeField] internal float m_VelocitySlowFactor = 0.99f;      // Percentage of velocity maintained when not thrusting
    internal Vector3 m_CurrentVel;
    internal bool m_Boosting = false;

    [Space(10)]
    [Header("Steer Settings")]
    [SerializeField] internal float m_SteeringTorque = 8.0f;           // Steering force added to the rigidbody as torque
    [SerializeField] internal float m_TrickTorque = 15.0f;             // Trick rotation force
    [SerializeField] internal float m_SidewaysDriftAmount = 15.0f;     // Sideways motion (drift) while accelerating and turning (1 to 100)
    [SerializeField] private Transform m_ShipBody;                     // GFX of the boat
    public float m_ShipRollAngle = 20f;                                // The angle that the ship "banks" into a turn
    public float m_ShipRollSpeed = 5f;                                 // Banking speed

    [Space(10)]
    [Header("Hover Settings")]
    [SerializeField] internal bool m_Grounded = false;                  // Is the boat grounded
    [SerializeField] private float m_GroundCheckDist = 5f;
    [SerializeField] internal bool m_AtTrickHeight = false;             // Height player must be to allow for tricks
    [Space(10)]
    [SerializeField] internal float m_HoverForce = 20.0f;
    [SerializeField] internal float m_HoverHeight = 5.0f;
    [SerializeField] internal float m_HoverGravity = 20f;               //The gravity applied to the ship while it is on the ground
    [SerializeField] internal float m_FallGravity = 30f;               //The gravity applied to the ship while it is in the air
    [SerializeField] internal float m_LevelingForce = 0.1f;             // Force applied to hover points to keep the boat level
    [SerializeField] internal float m_TrickHeightCheck = 15.0f;
    [SerializeField] private GameObject[] m_HoverPoints;
    public LayerMask m_HoverLayers;
    private Vector3 m_CoM;
    private Vector3 m_InAirCoM;
    private Vector3 m_GroundNormal;

    public PIDController hoverPID;			                            //A PID controller to smooth the ship's hovering
    internal PlayerController m_PlayerController;                       // Player controller script
    internal Rigidbody m_RigidBody;                                      // Rigidbody attached to the boat

    private float drag;
    private bool m_IsSelfRighting = false;

    private static PlayerMovement m_Instance;
    public static PlayerMovement Instance
    {
        get { return m_Instance; }
    }

    void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;

    }

    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_CoM = gameObject.transform.Find("CoM").transform.localPosition;
        m_InAirCoM = gameObject.transform.Find("InAirCoM").transform.localPosition;
        m_RigidBody.centerOfMass = m_CoM;

        drag = m_AccelerationForce / m_MaxSpeed;
    }

    void Update()
    {
        m_MovementInput = m_Movement;
    }

    void FixedUpdate()
    {
        m_CurrentVel = m_RigidBody.velocity;
        m_CurrentSpeed = Vector3.Dot(m_CurrentVel, transform.forward);
        m_AtTrickHeight = AtTrickHeight();

        // Set the rb's CoM position based on if the player is at trick height
        if (m_AtTrickHeight)
        {
            m_RigidBody.centerOfMass = m_InAirCoM;
        }
        else
        {
            m_RigidBody.centerOfMass = m_CoM;
        }


        Accelerate();
        Steer();
        //CalculatePropulsion();
        Hover();
    }

    // Controls the acceleration of the boat
    public void Accelerate()
    {
        Vector3 forward = m_RigidBody.transform.forward;
        //forward.y = 0.0f;
        //forward.Normalize();

        forward = Vector3.ProjectOnPlane(forward, m_GroundNormal);
        if (!m_AtTrickHeight)
        {
            m_RigidBody.AddForce(forward * m_MovementInput.y * m_AccelerationForce, ForceMode.Acceleration);
        }
        else
        {
            if (m_IsSpacePressed)
            {
                AirFlip(m_MovementInput.y);
            }
        }

        // Boost
        if (m_IsShiftPressed)
        {
            Boost();
            m_Boosting = true;
        }
        else
        {
            m_Boosting = false;
        }

        // Clamp Speed
        if (m_CurrentSpeed > m_MaxSpeed)
        {
            m_RigidBody.velocity = m_RigidBody.velocity.normalized * m_MaxSpeed;
        }

        // Slow velocity when theres no forward input and not in the air
        if (m_MovementInput.y < 0.01f && m_Grounded)
        {
            m_RigidBody.velocity = m_RigidBody.velocity * m_VelocitySlowFactor;
        }
    }

    // Controls turning
    public void Steer()
    {
        if (m_IsSpacePressed)
        {
            if (m_AtTrickHeight)
            {
                AirRoll(m_MovementInput.x);
            }
            else
            {
                m_RigidBody.AddRelativeTorque(Vector3.up * m_MovementInput.x * m_SteeringTorque, ForceMode.Acceleration);
            }
        }
        else
        {
            m_RigidBody.AddRelativeTorque(Vector3.up * m_MovementInput.x * m_SteeringTorque, ForceMode.Acceleration);
        }

        if (m_Grounded)
        {
            //Calculate the angle we want the ship's body to bank into a turn.
            float angle = m_ShipRollAngle * -m_MovementInput.x;

            //Calculate the rotation needed for this new angle
            Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);

            //Finally, apply this angle to the ship's body
            m_ShipBody.rotation = Quaternion.Lerp(m_ShipBody.rotation, bodyRotation, Time.deltaTime * m_ShipRollSpeed);

        }

        // Calculate current sideways speed by using the dot product.
        float sidewaysSpeed = Vector3.Dot(m_RigidBody.velocity, transform.right);

        // Calculate the force to apply to the side of the vehicle to limit the amount of sideways drifting while turning.
        Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime) / m_SidewaysDriftAmount;

        // Apply the sideways friction
        m_RigidBody.AddForce(sideFriction, ForceMode.Acceleration);
    }

    void CalculatePropulsion()
    {
        float rotationTorque = m_MovementInput.x - m_RigidBody.angularVelocity.y;
        m_RigidBody.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);

        float sidewaysSpeed = Vector3.Dot(m_RigidBody.velocity, transform.right);

        // Calculate the force to apply to the side of the vehicle to limit the amount of sideways drifting while turning.
        Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime) / m_SidewaysDriftAmount;

        // Apply the sideways friction
        m_RigidBody.AddForce(sideFriction, ForceMode.Acceleration);

        if (m_MovementInput.x <= 0f)
        {
            m_RigidBody.velocity *= m_VelocitySlowFactor;
        }
        if (!m_Grounded)
            return;

        float propulsion = m_AccelerationForce * m_MovementInput.y - drag * Mathf.Clamp(m_CurrentSpeed, 0f, m_MaxSpeed);
        m_RigidBody.AddForce(transform.forward * propulsion, ForceMode.Acceleration);

    }

    private void Hover()
    {
        //Old hover calculation:
        //m_RigidBody.AddForceAtPosition(((m_GroundNormal * m_HoverForce)) * Mathf.Abs(1.0f - (Vector3.Distance(hit.point, hoverPoint.transform.position) / m_HoverHeight)), hoverPoint.transform.position, ForceMode.Acceleration);

        // Create a new ray
        Ray groundRay = new Ray(transform.position, -m_GroundNormal);
        // Store raycast data
        RaycastHit groundHit;
        // Check if player is grounded
        m_Grounded = Physics.Raycast(groundRay, out groundHit, m_GroundCheckDist, m_HoverLayers);

        if (m_Grounded)
        {
            // Store height from ground
            float height = groundHit.distance;
            // Get the normal direction of the ground

            //m_GroundNormal = groundHit.normal.normalized;
            
            Vector3[] hoverPointGroundNormals = new Vector3[m_HoverPoints.Length];

            for (int i = 0; i < m_HoverPoints.Length; i++)
            {
                var hoverPoint = m_HoverPoints[i];

                Ray hoverRay = new Ray(hoverPoint.transform.position, -transform.up);
                RaycastHit hit;
                
                Physics.Raycast(hoverRay, out hit, m_GroundCheckDist, m_HoverLayers);

                hoverPointGroundNormals[i] = hit.normal.normalized;
            }

            m_GroundNormal = (hoverPointGroundNormals[0] + hoverPointGroundNormals[1] + hoverPointGroundNormals[2] + hoverPointGroundNormals[3]) / m_HoverPoints.Length;

            // Use PID controller to calculate a percentage 
            float forcePercent = hoverPID.Seek(m_HoverHeight, height);

            // Calculate a force to apply based on the ground normal, multiplied by the force percentage
            Vector3 force = m_GroundNormal * m_HoverForce * forcePercent;
            // Calculate a gravity force based on the height
            Vector3 gravity = -m_GroundNormal * m_HoverGravity * height;

            // Add each force to the rigid body
            m_RigidBody.AddForce(force, ForceMode.Acceleration);
            m_RigidBody.AddForce(gravity, ForceMode.Acceleration);

            // Rotate the rigidbody based on the ground normal
            Vector3 projection = Vector3.ProjectOnPlane(transform.forward, m_GroundNormal);
            Quaternion rotation = Quaternion.LookRotation(projection, m_GroundNormal);

            m_RigidBody.MoveRotation(Quaternion.Lerp(m_RigidBody.rotation, rotation, Time.fixedDeltaTime * 10f));
        }

        // If not grounded
        else
        {
            // Set the ground normal to be directly up
            m_GroundNormal = Vector3.up;
            // Set gravity
            Vector3 gravity = -m_GroundNormal * m_FallGravity;
            // And apply the force to the RB
            m_RigidBody.AddForce(gravity, ForceMode.Acceleration);

            // If below trick height
            if (!m_AtTrickHeight)
            {
                for (int i = 0; i < m_HoverPoints.Length; i++)
                {
                    var hoverPoint = m_HoverPoints[i];
                    // level out hoverpoints
                    if (m_CoM.y > hoverPoint.transform.position.y)
                    {
                        m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
                    }
                    else
                    {
                        m_RigidBody.AddForceAtPosition(hoverPoint.transform.up * -m_LevelingForce, hoverPoint.transform.position, ForceMode.Acceleration);
                    }
                }

            }
        }
    }


    public bool AtTrickHeight()
    {
        return !Physics.Raycast(transform.position, -Vector3.up, m_TrickHeightCheck, m_HoverLayers);
    }

    private void AirFlip(float _currentPitch)
    {
        m_RigidBody.AddRelativeTorque(Vector3.right * _currentPitch * m_TrickTorque, ForceMode.Acceleration);
    }

    private void AirRoll(float _currentRoll)
    {
        m_RigidBody.AddRelativeTorque(Vector3.back * _currentRoll * m_TrickTorque, ForceMode.Acceleration);
    }

    private void Boost()
    {
        Vector3 forward = m_RigidBody.transform.forward;

        m_RigidBody.AddForce(forward * m_BoostForce, ForceMode.Acceleration);
    }

    public void ShiftPressed(float value)
    {

        if (value > 0)
        {
            m_IsShiftPressed = true;
        }

        else
        {
            m_IsShiftPressed = false;
        }
    }

    public void SpacePressed(float value)
    {

        if (value > 0)
        {
            m_IsSpacePressed = true;
        }

        else
        {
            m_IsSpacePressed = false;
        }
    }

    public void Movement(Vector2 movement)
    {
        m_Movement = movement;
    }

    void OnDrawGizmos()
    {
        // Hoverpoint Drawing
        // Copying racast conditions from hover method to draw gizmos
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];

            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_HoverHeight, m_HoverLayers))
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
                Gizmos.DrawLine(hoverPoint.transform.position, hoverPoint.transform.position - transform.up * m_HoverHeight);
            }
            // Hoverpoints
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(hoverPoint.transform.position, 0.3f);
        }

        // CoM
        if (m_RigidBody)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + m_CoM, 0.5f);

        }
        // Trick height
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, (transform.position + (-Vector3.up * m_TrickHeightCheck)));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + m_CurrentVel);

    }

    public Vector2 GetSpeeds()
    {
        Vector2 speeds = Vector2.zero;
        speeds.x = m_CurrentSpeed;
        speeds.y = m_MaxSpeed;
        return speeds;
    }
}
