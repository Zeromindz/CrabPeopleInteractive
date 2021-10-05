using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BoatController : MonoBehaviour
{
    private Rigidbody m_RigidBody; // Rigidbody attached to the boat
    [SerializeField] private Camera m_Cam;
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    [Header("Player")]
    private int m_Passengers = 0;

    [Space(10)]
    [Header("Engine")]
    [SerializeField] private bool m_InWater = false;
    [SerializeField] private float m_SteeringTorque = 5.0f;
    [SerializeField] private float m_HorsePower = 18.0f;
    [SerializeField] private Transform m_EngineTransform;
    private Vector3 m_LastPosition;
    private float m_CurrentSpeed;
    
    private float m_VerticalInput;
    private float m_HorizontalInput;
    private float m_CamFovVel;

    // Return methods
    public float GetSpeed() { return m_CurrentSpeed; }
    public float GetPassengers() { return m_Passengers; }

    //[SerializeField] private LayerMask m_LayerMask;

    // Other movement
    private float m_Throttle;
    private float m_SteerFactor;

    private Vector3 m_EngineDirection;
    private float m_TurnVel;
    private float m_CurrentAngle;

    public float m_MovementThreshold = 10.0f;
    private float m_MovementFactor;


    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        m_VerticalInput = Input.GetAxis("Vertical");
        m_HorizontalInput = Input.GetAxis("Horizontal");

        m_InWater = EngineSubmerged();
    }

    private void FixedUpdate()
    {
        Accelerate(m_VerticalInput);
        Turn(m_HorizontalInput);

        CalculateSpeed();
    }

    private void LateUpdate()
    {
        if (m_Cam)
        {
            float fov = Mathf.SmoothStep(m_FovMin, m_FovMax, m_RigidBody.velocity.magnitude * 0.005f);
            m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, fov, ref m_CamFovVel, m_FovSmoothTime);
        }
    }

    // Controls the acceleration of the boat
    public void Accelerate(float _input)
    {
        Vector3 forward = m_RigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        // Only add the force if the engine is submerged
        if (m_InWater)
        {
            m_RigidBody.AddForce(m_HorsePower * _input * forward, ForceMode.Acceleration); // Add force forward based on input and horsepower
        }

    }

    public void Turn(float _input)
    {
        m_RigidBody.AddRelativeTorque(new Vector3(0f, m_SteeringTorque, 0) * _input, ForceMode.Acceleration);
    }

    private void CalculateSpeed()
    {
        // Calculate speed in meters per second
        m_CurrentSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        //Save the position for the next update
        m_LastPosition = transform.position;

        Debug.Log("Current speed: " + (int)m_CurrentSpeed);
    }

    private bool EngineSubmerged()
    {
        float waveYPos = WaveManager.m_Instance.GetWaveHeight(m_EngineTransform.position);
        return (m_EngineTransform.position.y < waveYPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Pickup")
        {
            m_Passengers++;
            other.gameObject.GetComponent<ItemPickup>().OnPickup();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_EngineTransform.position, 0.3f);
       
    }
}
