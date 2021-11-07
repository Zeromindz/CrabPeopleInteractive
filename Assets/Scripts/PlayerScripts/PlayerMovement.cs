using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==================================================
// Handles moving the player through the rigidbody
//______________________________________________/

public class PlayerMovement : MonoBehaviour
{
    internal PlayerController m_PlayerController;                       // Player controller script
    public PIDController hoverPID;			                            //A PID controller to smooth the ship's hovering
    [SerializeField] private Rigidbody m_RigidBody;                     // Rigidbody attached to the boat
    [SerializeField] private Transform m_ShipBody;                      // GFX of the boat

    public bool m_UseGravity = true;

    [Header("Boat Settings")]
    [SerializeField] internal bool m_Grounded = false;                  // Is the boat grounded (in water)
    [SerializeField] internal float m_MaxGroundedDist = 5f;
    [SerializeField] internal bool m_Boosting = false;
    [Space(10)]
    [Tooltip("Acceleration force added to the rigidbody")]
    [Range(0f, 100.0f)]
    [SerializeField] internal float m_HorsePower = 60.0f;               // Acceleration force
    [Tooltip("Max speed (m/s) without boost")]
    [SerializeField] internal float m_MaxSpeed = 60.0f;                 // Max speed without boost
    [Tooltip("Boost force added to the rigidbody")]
    [SerializeField] internal float m_BoostSpeed = 20.0f;              // Boost force
    [Tooltip("Max speed (m/s) with boost")]
    [SerializeField] internal float m_MaxBoostSpeed = 100.0f;            // Max speed with boost
    [Tooltip("Steering force added to the rigidbody as torque")]
    [SerializeField] internal float m_SteeringTorque = 8.0f;            // Steering speed
    [Tooltip("Sideways motion (drift) while accelerating and turning")]
    [Range(1.0f, 100.0f)]
    [SerializeField] internal float m_SidewaysDriftAmount = 15.0f;       // Sideways motion (drift) while accelerating and turning (1 to 100)
    public float GetSpeed() { return m_RigidBody.velocity.magnitude; }
    public Vector3 GetCurrentVel() { return m_RigidBody.velocity; }
    private Vector2 m_MovementInput;

    [Space(10)]
    [Header("Physics")]
    [SerializeField] internal bool m_AtTrickHeight;                     // Is the boat at trick height
    [Tooltip("Height player must be to allow for tricks")]
    [Range(1.0f, 50.0f)]
    [SerializeField] internal float m_TrickHeightCheck = 15.0f;
    [Space(10)]
    public float m_HoverGravity = 20f;        //The gravity applied to the ship while it is on the ground
    public float m_FallGravity = 80f;			//The gravity applied to the ship while it is falling
    //[Range(-1.0f, -100.0f)]
    //[SerializeField] internal float m_Gravity = -50.0f;
    [Tooltip("Trick rotation force")]
    [SerializeField] internal float m_InAirTorque = 15.0f;              // Trick rotation force
    [Tooltip("Force applied to hover points to keep the boat level")]
    [SerializeField] internal float m_LevelingForce = 0.1f;             // Force applied to hover points to keep the boat level
    [Tooltip("How qucikly the boat deccelerates without input while grounded")]
    [SerializeField] internal float m_VelocitySlowFactor = 0.99f;       // Velocity slow factor
    [SerializeField] internal float m_GroundHoverForce = 20.0f;
    [SerializeField] internal float m_GroundHoverHeight = 5.0f;
    [SerializeField] private GameObject[] m_HoverPoints;
    [Space(10)]
    public LayerMask m_LayerMask;
    public Vector3 m_CoM;
    public Vector3 m_InAirCoM;

    [Space(10)]
    [Header("GFX")]
    public float m_ShipRollAngle = 20f;         // The angle that the ship "banks" into a turn
    public float m_ShipRollSpeed = 5f;          // Banking speed
    private Vector2 m_Movement = Vector2.zero;
    bool isShiftPressed;
    bool isSpacePressed;
    [SerializeField, Range(0, 2)] internal float m_BounceForce;

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
        m_CoM = gameObject.transform.Find("CoM").transform.localPosition;
        m_InAirCoM = gameObject.transform.Find("InAirCoM").transform.localPosition;
        m_RigidBody.centerOfMass = m_CoM;
    }

    void Update()
    {
        m_MovementInput = m_Movement;
        CheckWallRaycast();
    }

    void FixedUpdate()
    {
        // Check if at trick height
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

        // Apply gravity
        if (m_UseGravity && !m_Grounded)
        {
            m_RigidBody.AddForce((-Vector3.up * m_FallGravity), ForceMode.Acceleration);
        }

        // Boost
        if (isShiftPressed)
        {
            Boost();
            m_Boosting = true;
        }
        else
        {
            m_Boosting = false;
        }

        // Clamp Speed
        if (m_Boosting)
        {
            if (GetSpeed() > m_MaxBoostSpeed)
            {
                m_RigidBody.velocity = m_RigidBody.velocity.normalized * m_MaxBoostSpeed;
            }
        }
        else
        {
            if (GetSpeed() > m_MaxSpeed)
            {
                m_RigidBody.velocity -= m_RigidBody.velocity.normalized;
            }
        }

        // Slow velocity when theres no forward input and not in the air
        if (m_MovementInput.y < 0.01f && m_Grounded)
        {
            m_RigidBody.velocity = m_RigidBody.velocity * m_VelocitySlowFactor;
        }

        // Calculate current sideways speed by using the dot product.
        float sidewaysSpeed = Vector3.Dot(m_RigidBody.velocity, transform.right);

        // Calculate the force to apply to the side of the vehicle to limit the amount of sideways drifting while turning.
        Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime) / m_SidewaysDriftAmount;

        // Apply the sideways friction
        m_RigidBody.AddForce(sideFriction, ForceMode.Acceleration);

        //ApplyForceToPoints();
        CalculateHover();

        Accelerate();
        Steer();
    }

    // Controls the acceleration of the boat
    public void Accelerate()
    {
        Vector3 forward = m_RigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        if (!m_AtTrickHeight)
        {
            m_RigidBody.AddForce(forward * m_MovementInput.y * m_HorsePower, ForceMode.Acceleration);
        }
        else
        {
            if (isSpacePressed)
            {
                AirFlip(m_MovementInput.y);
            }
        }
    }

    // Controls turning
    public void Steer()
    {
        if (isSpacePressed && m_AtTrickHeight)
        {
            AirRoll(m_MovementInput.x);
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
    }

    public bool AtTrickHeight()
    {
        return !Physics.Raycast(transform.position, -Vector3.up, m_TrickHeightCheck, m_LayerMask);
    }

    public void AirFlip(float _currentPitch)
    {
        m_RigidBody.AddRelativeTorque(Vector3.right * _currentPitch * m_InAirTorque, ForceMode.Acceleration);
    }

    public void AirRoll(float _currentRoll)
    {
        m_RigidBody.AddRelativeTorque(Vector3.back * _currentRoll * m_InAirTorque, ForceMode.Acceleration);
    }

    public void Boost()
    {
        Vector3 forward = m_RigidBody.transform.forward;

        m_RigidBody.AddForce(forward * m_BoostSpeed, ForceMode.Acceleration);
    }

    // Old hover Calculation
    //m_RigidBody.AddForceAtPosition((Vector3.up * m_GroundHoverForce) * Mathf.Abs(1.0f - (Vector3.Distance(hit.point, hoverPoint.transform.position) / m_GroundHoverHeight)), hoverPoint.transform.position);
    private void ApplyForceToPoints()
    {
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
            // Raycast down from each floater
            RaycastHit hit;

            var hoverPoint = m_HoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
            {
                //======================================
                // Hovering
                //____________________________________

                // Store the normal of the ground
                Vector3 groundNormal;

                // Determine how high off the ground it is
                float height = hit.distance;
                // Save the normal of the ground
                groundNormal = hit.normal.normalized;
                // Use the PID controller to determine the amount of hover force needed
                float forcePercent = hoverPID.Seek(m_GroundHoverHeight, height);

                // Calulcate the total amount of hover force based on normal (or "up") of the ground
                Vector3 force = groundNormal * m_GroundHoverForce * forcePercent;
                // Calculate the force and direction of gravity to adhere the ship to the 
                // track (which is not always straight down in the world)
                Vector3 gravity = -groundNormal * m_HoverGravity * height;

                m_RigidBody.AddForceAtPosition(force, hoverPoint.transform.position, ForceMode.Acceleration);

                // Apply the hover and gravity forces
                m_RigidBody.AddForce(gravity, ForceMode.Acceleration);

                //Calculate the amount of pitch and roll the ship needs to match its orientation
                //with the ground.
                Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
                Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

                // Move the ship over time to match the desired rotation to match the ground.
                m_RigidBody.MoveRotation(Quaternion.Lerp(m_RigidBody.rotation, rotation, Time.fixedDeltaTime * 10f));


                m_Grounded = true;
            }
            else
            {
                m_Grounded = false;

                // If below trick height
                if (!m_AtTrickHeight)
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


    void CalculateHover()
    {
        // Raycast down from each floater
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, m_GroundHoverHeight, m_LayerMask))
        {
            //======================================
            // Hovering
            //____________________________________

            // Store the normal of the ground
            Vector3 groundNormal;

            // Determine how high off the ground it is
            float height = hit.distance;
            // Save the normal of the ground
            groundNormal = hit.normal.normalized;
            // Use the PID controller to determine the amount of hover force needed
            float forcePercent = hoverPID.Seek(m_GroundHoverHeight, height);

            // Calulcate the total amount of hover force based on normal (or "up") of the ground
            Vector3 force = groundNormal * m_GroundHoverForce * forcePercent;
            // Calculate the force and direction of gravity to adhere the ship to the 
            // track (which is not always straight down in the world)
            Vector3 gravity = -groundNormal * m_HoverGravity * height;

            m_RigidBody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);

            // Apply the hover and gravity forces
            m_RigidBody.AddForce(gravity, ForceMode.Acceleration);

            //Calculate the amount of pitch and roll the ship needs to match its orientation
            //with the ground.
            Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
            Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

            // Move the ship over time to match the desired rotation to match the ground.
            m_RigidBody.MoveRotation(Quaternion.Lerp(m_RigidBody.rotation, rotation, Time.fixedDeltaTime * 10f));

            // Old hover Calculation
            //m_RigidBody.AddForceAtPosition((Vector3.up * m_GroundHoverForce) * Mathf.Abs(1.0f - (Vector3.Distance(hit.point, hoverPoint.transform.position) / m_GroundHoverHeight)), hoverPoint.transform.position);

            m_Grounded = true;
        }
        else
        {
            m_Grounded = false;

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

    public void BounceOffWall(Vector3 angle)
    {
        m_RigidBody.AddForce(-m_RigidBody.velocity * m_BounceForce, ForceMode.VelocityChange);

     
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
        if (m_RigidBody)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + m_CoM, 0.5f);
            
        }
        // Trick height
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, (transform.position + (-Vector3.up * m_TrickHeightCheck)));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GetCurrentVel());

        // Draws feelers
        Quaternion diagonalFeelerRotation = Quaternion.Euler(0, 30, 0);
        Vector3 leftPos = (diagonalFeelerRotation * transform.forward).normalized;
        diagonalFeelerRotation.y = diagonalFeelerRotation.y * -1;
        Vector3 rightPos = (diagonalFeelerRotation * transform.forward).normalized;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + leftPos * 10);
        Gizmos.DrawLine(transform.position, transform.position + rightPos * 10);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

    }

    private void CheckWallRaycast() 
    {
        Quaternion diagonalFeelerRotation = Quaternion.Euler(0, 30, 0);
        Vector3 leftDir = (diagonalFeelerRotation * transform.forward).normalized;
        diagonalFeelerRotation.y = diagonalFeelerRotation.y * -1;
        Vector3 rightDir = (diagonalFeelerRotation * transform.forward).normalized;
        Vector3 upOffset = Vector3.up.normalized * 1;
       
        RaycastHit hitLeft;
        RaycastHit hitRight;

        if (Physics.Raycast(transform.position, leftDir, out hitLeft, ~m_LayerMask))
        {
            Debug.Log("Left Feeler Hit!");

        }

        if (Physics.Raycast(transform.position, rightDir, out hitRight, ~m_LayerMask))
        {
            Debug.Log("Right Feeler Hit!");      
        }
    }

    public void ShiftPressed(float value)
    {

        if (value > 0)
        {
            isShiftPressed = true;
        }

        else
        {
            isShiftPressed = false;
        }
    }

    public void SpacePressed(float value)
    {

        if (value > 0)
        {
            isSpacePressed = true;
        }

        else
        {
            isSpacePressed = false;
        }
    }

    public void Movement(Vector2 movement)
	{
        m_Movement = movement;
	}

}
