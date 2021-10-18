using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform m_Target;
    [SerializeField] private Transform m_LookAtTransform;

    [Space(10)]
    //[SerializeField] private Vector3 m_PositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    //[SerializeField] private Vector3 m_AngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private float m_LookAtTargetHeight = 5.0f;
    [SerializeField] private float m_LookAtTargetDist = 10.0f;
    [SerializeField] private float m_Angle = 5f;
    [SerializeField] private float m_SmoothTime = 2.0f; // Movement Smoothing Time
    [Space(10)]
    [SerializeField] private Vector3 rotationMask;

    public bool shouldRotate = true;

    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;


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

    private void FixedUpdate()
    {
        // --- old ---  
        // Set the target position above the player of the camera
        Vector3 desiredPosition = m_Target.position + (Vector3.up * m_LookAtTargetHeight) - (m_Target.forward * m_LookAtTargetDist);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_SmoothTime);
        transform.LookAt(m_Target.position + (Vector3.up * m_Angle), Vector3.up);


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