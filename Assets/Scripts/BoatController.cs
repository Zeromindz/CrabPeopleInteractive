using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BoatController : MonoBehaviour
{
    private Rigidbody m_RigidBody; // Rigidbody attached to the boat

    [Header("Engine Settings")]
    [SerializeField] private float m_SteeringTorque = 5.0f;
    [SerializeField] private float m_HorsePower = 18.0f;
    [SerializeField] private Vector3 m_EnginePosition;

    private Vector3 m_LastPosition;
    private float m_CurrentSpeed;

    private float m_VerticalInput;
    private float m_HorizontalInput;

    public float GetSpeed() { return m_CurrentSpeed; }


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
        
    }

    private void CalculateSpeed()
    {
        // Calculate speed in meters per second
        m_CurrentSpeed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        //Save the position for the next update
        m_LastPosition = transform.position;

        Debug.Log("Current speed: " + (int)m_CurrentSpeed);
    }

    private void FixedUpdate()
    {
        Accelerate(m_VerticalInput);
        Turn(m_HorizontalInput);

        CalculateSpeed();
    }

    // Controls the acceleration of the boat
    public void Accelerate(float _input)
    {
        Vector3 forward = m_RigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        m_RigidBody.AddForce(m_HorsePower * _input * forward, ForceMode.Acceleration); // Add force forward based on input and horsepower
        m_RigidBody.AddRelativeTorque(-Vector3.right * _input, ForceMode.Acceleration);

        //Debug.Log(boatController.CurrentSpeed);

        //Vector3 forceToAdd = -waterJetTransform.forward * currentJetPower;

        //// Only add the force if the engine is below sea level
        //float waveYPos = WaveManager.m_Instance.GetWaveHeight(waterJetTransform.position.y);

        //if (waterJetTransform.position.y < waveYPos)
        //{
        //    boatRB.AddForceAtPosition(forceToAdd, waterJetTransform.position);
        //}
        //else
        //{
        //    boatRB.AddForceAtPosition(Vector3.zero, waterJetTransform.position);
        //}
    }

    public void Turn(float _input)
    {
        m_RigidBody.AddRelativeTorque(new Vector3(0f, m_SteeringTorque, -m_SteeringTorque * 0.5f) * _input, ForceMode.Acceleration); 
        
        
        // Add torque based on input and torque amount
        //Steer left
        //if (Input.GetKey(KeyCode.A))
        //{
        //    WaterJetRotation_Y = waterJetTransform.localEulerAngles.y + 2f;

        //if (WaterJetRotation_Y > 30f && WaterJetRotation_Y < 270f)
        //{
        //    WaterJetRotation_Y = 30f;
        //}

        //Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

        //waterJetTransform.localEulerAngles = newRotation;
        //}
        ////Steer right
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    WaterJetRotation_Y = waterJetTransform.localEulerAngles.y - 2f;

        //    if (WaterJetRotation_Y < 330f && WaterJetRotation_Y > 90f)
        //    {
        //        WaterJetRotation_Y = 330f;
        //    }

        //    Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

        //    waterJetTransform.localEulerAngles = newRotation;
        //}
    }
}
