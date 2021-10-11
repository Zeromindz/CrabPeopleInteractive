﻿using System.Collections;
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
    [SerializeField] internal bool m_Grounded = false;
    [Space(10)]
    [SerializeField] internal float m_HorsePower = 30.0f;
    [SerializeField] internal float m_MaxSpeed = 50.0f;
    [SerializeField] internal float m_BoostSpeed = 100.0f;
    [SerializeField] internal float m_MaxBoostSpeed = 50.0f;
    [SerializeField] internal float m_SteeringTorque = 8.0f;
    private bool m_Boosting = false;
    [Space(10)]
    [SerializeField] internal float m_Gravity = -9.81f;
    [SerializeField] internal float m_LevelingForce = 2.0f;
    [SerializeField] internal float m_VelocitySlowFactor = 0.95f;
    internal Vector3 m_CoM;
    internal float m_PlayerHeight;

    [Header("Physics")]
    [SerializeField] internal bool m_AtTrickHeight;
    [SerializeField] internal float m_GroundHoverForce = 9.0f;
    [SerializeField] internal float m_GroundHoverHeight = 4.0f;
    [SerializeField] internal float m_InAirTorque = 20.0f; // May need to split to vert and hori
    [SerializeField] internal float m_TrickHeightCheck = 10.0f;
    //private int m_LayerMask;
    public LayerMask m_LayerMask;
    private float m_CurrentThrust = 0.0f;
    private float m_CurrentSteer = 0.0f;
    private float m_CurrentPitch = 0.0f;
    private float m_CurrentRoll = 0.0f;
    private float m_CurrentSpeed = 0f;
    public float GetSpeed() { return m_RigidBody.velocity.magnitude; }

    [Space(10)]
    [SerializeField] private GameObject[] m_HoverPoints;

    [Header("GFX")]
    [SerializeField] private ParticleSystem m_RocketTrail;
    [SerializeField] private ParticleSystem m_GroundedTrail;

    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_RigidBody = GetComponent<Rigidbody>();

        m_CoM = gameObject.transform.Find("CoM").transform.localPosition;
        m_RigidBody.centerOfMass = m_CoM;

        //m_LayerMask = 1 << LayerMask.NameToLayer("Character");
        //m_LayerMask = ~m_LayerMask;

    }

    void Update()
    {

        //Check the input manager for current input
        m_CurrentThrust = m_PlayerController.playerInput.GetWASDVertical();
        m_CurrentSteer = m_PlayerController.playerInput.GetWASDHorizontal();
        m_CurrentPitch = m_PlayerController.playerInput.GetArrowsVertical();
        m_CurrentRoll = m_PlayerController.playerInput.GetArrowsHorizontal();
        m_CurrentSpeed = GetSpeed();
        // Set vfx emissions
        int rocketEmissionRate = 0;
        if(m_Boosting)
        {
            rocketEmissionRate = 10;

        }
        var rocketEmission = m_RocketTrail.emission;
        rocketEmission.rateOverTime = new ParticleSystem.MinMaxCurve(rocketEmissionRate);

        int groundEmissionRate = 0;
        if(m_Grounded)
        {
            groundEmissionRate = 10;

        }
        var groundEmission = m_GroundedTrail.emission;
        groundEmission.rateOverTime = new ParticleSystem.MinMaxCurve(groundEmissionRate);

    }

    void FixedUpdate()
    {

        //debug
        m_AtTrickHeight = AtTrickHeight();

        // Apply gravity
        m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);

        if(m_Boosting)
        {
            if(GetSpeed() > m_MaxBoostSpeed)
            {
                m_RigidBody.velocity = m_RigidBody.velocity.normalized * m_MaxBoostSpeed;
            }
        }
        else
        {
            // Clamp Speed
            if (GetSpeed() > m_MaxSpeed)
            {
                m_RigidBody.velocity -= m_RigidBody.velocity.normalized;
                
            }
        }
            
       

        if (m_CurrentThrust < 0.01f)
        {
            //Vector3 newVel = m_RigidBody.velocity * m_VelocitySlowFactor;
            //newVel.y = m_Gravity;
            m_RigidBody.velocity = m_RigidBody.velocity * m_VelocitySlowFactor;
        }

        if (m_PlayerController.playerInput.ShiftPressed())
        {
            Boost();
            m_Boosting = true;
        }
        else
        {
            m_Boosting = false;
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
        m_RigidBody.AddForce(forward * m_CurrentThrust * m_HorsePower, ForceMode.Acceleration);
        if (m_AtTrickHeight)
        {
            AirFlip(m_CurrentPitch);

        }
        
    }

    // Controls turning
    public void Steer()
    {

        if (m_AtTrickHeight)
        {
            AirRoll(m_CurrentRoll);

        }
        else
        {

            m_RigidBody.AddRelativeTorque(Vector3.up * m_CurrentSteer * m_SteeringTorque, ForceMode.Acceleration);
        }

    }
  

    public bool AtTrickHeight()
    {
        return !Physics.Raycast(transform.position, -transform.up, m_TrickHeightCheck, m_LayerMask);
    }

    public void AirFlip(float _currentThrust)
    {
        m_RigidBody.AddRelativeTorque(Vector3.right * _currentThrust * m_InAirTorque, ForceMode.Acceleration);
    }

    public void AirRoll(float _currentSteer)
    {
        m_RigidBody.AddRelativeTorque(Vector3.back * _currentSteer * m_InAirTorque, ForceMode.Acceleration);
    }

    public void Boost()
    {
        Vector3 forward = m_RigidBody.transform.forward;

        m_RigidBody.AddForce(forward * m_BoostSpeed, ForceMode.Acceleration);
    }
    
    // Only works with wave manager active
    private void ApplyForcetoPoints()
    {
        // Raycast down from each floater
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
            {
                //======================================
                // Hovering
                //____________________________________/
                m_RigidBody.AddForceAtPosition(Vector3.up * m_GroundHoverForce * (1.0f - (hit.distance / m_GroundHoverHeight)), hoverPoint.transform.position, ForceMode.Acceleration);

                m_Grounded = true;
            }
            else
            {
                m_Grounded = false;

                // If below trick height
                if(!m_AtTrickHeight)
                {
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

    void OnDrawGizmos()
    {
        // Hoverpoint Drawing
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
            // Hoverpoints
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(hoverPoint.transform.position, 0.3f);
        }

        // CoM
        if(m_RigidBody)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_CoM, 0.5f);
        }

        // Trick height
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, (transform.position  + (-Vector3.up * m_TrickHeightCheck)));

    }
}