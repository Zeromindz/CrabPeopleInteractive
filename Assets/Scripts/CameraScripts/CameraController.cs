using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform m_Target;
    
    [Space(10)]
    [SerializeField] private Vector3 m_PositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    [SerializeField] private Vector3 m_AngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private float m_SmoothTime = 2.0f; // Movement Smoothing Time
    [Space(10)]
    [SerializeField] private Vector3 rotationMask;

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


    // Late update since we're using physics and our calculations should be after everything else
    private void FixedUpdate()
    {
        // --- old ---  
        // Set the target position above the player of the camera
        Vector3 desiredPosition = m_Target.position + (Vector3.up * m_PositionOffset.y) - (m_Target.forward * m_PositionOffset.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_SmoothTime);
        transform.LookAt(m_Target.position + (Vector3.up * m_AngleOffset.y), Vector3.up);

        //CameraFollow();
    }

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

        // hange the position with a Lerp.
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, m_SmoothTime * Time.deltaTime);
        transform.position = position;
    }

    public void WatchGhost()
	{
        m_Target = GameObject.FindGameObjectWithTag("Ghost").transform;
	}

    private void OnDrawGizmos()
    {
        
    }
}


