using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    private Camera m_Cam;
    [SerializeField] private Transform m_Target;
    [SerializeField] private Transform m_LookAtTransform;
    [SerializeField] private float m_CamHeight = 5.0f;
    [SerializeField] private float m_CamDist = 10.0f;
    [SerializeField] private float m_CamAngle = 5f;

    [Space(10)]
    [Header("FOV Settings")]
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    private float m_TargetSpeed;
    private float m_CamFovVel;
    private Vector3 m_TargetLastPosition;

    [Space(10)]
    //[SerializeField] private Vector3 m_PositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    //[SerializeField] private Vector3 m_AngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    //[SerializeField] private float m_SmoothTime = 2.0f; // Movement Smoothing Time
    //[Space(10)]
    //[SerializeField] private Vector3 rotationMask;

    private static CameraController m_Instance;               // Current Private instance
    public static CameraController Instance                   // Current public instance
    {
        get { return m_Instance; }
    }

    private void Awake()
    {
        // Initialize Singleton
        if (m_Instance != null && m_Instance != this)
            Destroy(this.gameObject);
        else
            m_Instance = this;
    }

    private void Start()
    {
        m_Cam = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        m_TargetSpeed = m_Target.GetComponent<PlayerController>().playerMovement.GetSpeed();    
    }

    private void FixedUpdate()
    {
        // --- old ---  
        // Set the target position above the player of the camera
        Vector3 desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (m_Target.forward * m_CamDist);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_SmoothTime);
        transform.LookAt(m_Target.position + (Vector3.up * m_CamAngle), Vector3.up);


        //CalculateTargetSpeed();

        float fov = Mathf.SmoothStep(m_FovMin, m_FovMax, m_TargetSpeed * 0.005f);
        m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, fov, ref m_CamFovVel, m_FovSmoothTime);


        #region alternate logic
        /*
        // Early out if we don't have a target
        if (!m_Target) return;

        // Calculate the current rotation angles
        float wantedRotationAngle = m_Target.eulerAngles.y;
        float wantedHeight = m_Target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = m_Target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        float targetRotX = m_Target.eulerAngles.x;
        float targetRotY = m_Target.eulerAngles.y;
        float targetRotZ = m_Target.eulerAngles.z;

        // Set rotation to the inverse of the x and z components
        m_LookAtTransform.rotation = Quaternion.Euler(targetRotX - targetRotX, targetRotY, targetRotZ - targetRotZ);
        m_LookAtTransform.position = m_Target.position + (Vector3.up * m_LookAtTargetHeight);
        transform.LookAt(m_LookAtTransform.position + (m_LookAtTransform.forward * m_LookAtTargetDist), Vector3.up);

        */

        // Always look at the target
        //transform.LookAt(m_Target.position + (Vector3.up * m_AngleOffset.y) + (m_Target.forward * 10f), Vector3.up);

        //CameraFollow();
        #endregion
    }


    /*
    void CameraFollow()
    {
        // Apply the initial rotation to the camera.
        Quaternion initialRotation = Quaternion.Euler(m_AngleOffset);

        // Calculate the rotation to be applied to the camera
        Quaternion rot = Quaternion.Lerp(transform.rotation, m_Target.rotation * initialRotation, m_SmoothTime * Time.deltaTime);
        // Mask out rotation axies
        rot = Quaternion.Euler(Vector3.Scale(rot.eulerAngles, rotationMask));
        transform.rotation = rot;

        // Calculate the camera transformed axes.
        Vector3 forward = transform.rotation * Vector3.forward;
        Vector3 right = transform.rotation * Vector3.right;
        Vector3 up = transform.rotation * Vector3.up;

        // Calculate the offset in the camera's coordinate frame.
        Vector3 targetPos = m_Target.position;
        Vector3 desiredPosition = targetPos
            + forward * m_PositionOffset.z
            + right * m_PositionOffset.x
            + up * m_PositionOffset.y;

        // change the position with a Lerp.
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, m_SmoothTime * Time.deltaTime);
        transform.position = position;
    }
    */ //Camera Follow


    private void CalculateTargetSpeed()
    {
        // Calculate speed in meters per second
        m_TargetSpeed = (m_Target.transform.position - m_TargetLastPosition).magnitude / Time.deltaTime;
        //Save the position for the next update
        m_TargetLastPosition = transform.position;
    }

    public void WatchGhost()
    {
        m_Target = GameObject.FindGameObjectWithTag("Ghost").transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_LookAtTransform.position + (m_LookAtTransform.forward * 10f), 1f);


    }


}