using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BoatController : MonoBehaviour
{
    private Rigidbody m_RigidBody; // Rigidbody attached to the boat
    private int m_LayerMask;

    [Space(10)]
    [Header("Boat")]
    [SerializeField] private bool m_InWater = false;
    [SerializeField] private float m_HorsePower = 18.0f;
    [SerializeField] private float m_SteeringTorque = 5.0f;
    [SerializeField] private float m_LevelingForce = 2.0f;
    [SerializeField] private float m_Gravity = -9.81f;
    [SerializeField] private float m_HoverForce = 9.0f;
    [SerializeField] private float m_HoverHeight = 2.0f;

    [SerializeField] private Transform m_EngineTransform;

    public GameObject[] m_HoverPoints;
    private float m_CurrentThrust = 0.0f;
    private float m_CurrentTurn = 0.0f;
    private int m_Passengers = 0;
    private Vector3 m_LastPosition;
    private float m_CurrentSpeed;

    [Space(10)]
    [Header("Camera")]
    [SerializeField] private Camera m_Cam;
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    private float m_CamFOVVel;

    public float GetSpeed() { return m_CurrentSpeed; }
    public float GetPassengers() { return m_Passengers; }

    private float m_DeadZone = 0.1f;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        m_LayerMask = 1 << LayerMask.NameToLayer("Character");
        m_LayerMask = ~m_LayerMask;
    }

    private void Update()
    {
        GetInput();

        CalculateSpeed();

        //m_InWater = EngineSubmerged();
    }

    void FixedUpdate()
    {
        Hover();

        Accelerate();
        Steer();
    }

   private void LateUpdate()
   {
       if (m_Cam)
       {
           float fov = Mathf.SmoothStep(m_FovMin, m_FovMax, m_RigidBody.velocity.magnitude * 0.005f);
           m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, fov, ref m_CamFOVVel, m_FovSmoothTime);
       }
   }

    void GetInput()
    {
        // Main Thrust
        m_CurrentThrust = 0.0f;
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > m_DeadZone)
        {
            m_CurrentThrust = verticalInput;
        }

        // Turning
        m_CurrentTurn = 0.0f;
        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > m_DeadZone)
        {
            m_CurrentTurn = horizontalInput;
        }

    }

    public void Hover()
    {
        // Apply gravity
        m_RigidBody.AddForceAtPosition((Vector3.up * m_Gravity), transform.position, ForceMode.Acceleration);
    
        RaycastHit hit;
        for (int i = 0; i < m_HoverPoints.Length; i++)
        {
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
    }

    // Controls turning
    public void Steer()
    {
        m_RigidBody.AddRelativeTorque(Vector3.up * m_CurrentTurn * m_SteeringTorque);
    }

    private void CalculateSpeed()
    {
        // Calculate speed in meters per second
        m_CurrentSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        //Save the position for the next update
        m_LastPosition = transform.position;
    }

    // Only works with wave manager active
    private bool EngineSubmerged()
    {
        float waveYPos = WaveManager.m_Instance.GetWaveHeight(m_EngineTransform.position);
        return (m_EngineTransform.position.y < waveYPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Picking up object
        if(other.tag == "Pickup")
        {
            m_Passengers++;
            other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }
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
    }
}
