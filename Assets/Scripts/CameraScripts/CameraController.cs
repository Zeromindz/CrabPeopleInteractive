using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    private Camera m_Cam;                               // Reference to the camera component to give access to the fieldOfView property 
    [SerializeField] private Transform m_Target;        // Target object transform
    private PlayerController m_Player;                  // Player controller so we can more easily get the current velocity 

    bool m_PositiveForwardVel = false;

    [SerializeField] private float m_CamHeight = 5.0f;
    [SerializeField] private float m_CamDist = 15.0f;
    [SerializeField] private float m_CamAngle = 5f;
    [SerializeField] private float m_FollowSpeed = 30.0f; // Movement smoothing Time
    [SerializeField] private float m_RotationSpeed = 10.0f; // Look smoothing Time
    [SerializeField] private float m_MinCameraDist = 3.0f;
    private float rotationVector;
    private Vector3 m_DesiredPosition;

    [Space(10)]
    [Header("FOV Settings")]
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    private float m_TargetSpeed;
    private float m_CamFovVel;
    private Vector3 m_TargetLastPosition;

    public LayerMask m_CollideWithCamera;

    public float GetCamFOV() { return m_Cam.fieldOfView; }

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
        m_Player = m_Target.gameObject.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // Store the targets speed in m/s, ignoring the y component of the velocity
        m_TargetSpeed = Vector3.Dot(m_Player.playerMovement.m_CurrentVel, m_Target.forward);

        // Store the target's direction of movement
        Vector3 playerVelocity = m_Player.playerMovement.m_CurrentVel;

        //if(vel.z < -0.5f)
        //{
        //    rotationVector = m_Target.eulerAngles.y + 100f;
        //}
        //else
        //{
        //    rotationVector = m_Target.eulerAngles.y;
        //}

        

        // Store the target's y rotation angle
        rotationVector = m_Target.eulerAngles.y;

        float desiredAngle = rotationVector;
        float desiredHeight = m_Target.position.y + m_CamHeight;
        float currentAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentAngle = Mathf.LerpAngle(currentAngle, desiredAngle, m_RotationSpeed * Time.deltaTime);
        currentHeight = Mathf.LerpAngle(currentHeight, desiredHeight, m_RotationSpeed * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        transform.position = m_Target.position;
        transform.position -= currentRotation * Vector3.forward * m_CamDist;

        Vector3 m_DesiredPosition = transform.position;
        m_DesiredPosition.y = currentHeight;


        //if(!m_Player.playerMovement.m_Grounded)
        //{
        //    m_DesiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (playerVelocity.normalized * m_CamDist);
        //}
        
        // Find the player's local velocity so we can tell if we're moving forwards or backwards
        //Vector3 localPlayerVel = m_Player.transform.InverseTransformDirection(playerVelocity);
        //
        //// Check if the z component is positive or negative
        //if(localPlayerVel.z > 0.0f)
        //{
        //    m_PositiveForwardVel = true;
        //}
        //else
        //{
        //    m_PositiveForwardVel = false;
        //}
        //
        //// Variable storing the normalized direction vector of the player
        //Vector3 normalizedVelocity;
        //
        //// If player's velocity is greater than 0.1
        //if(playerVelocity.magnitude > 0.1f)
        //{
        //    // Set vector to the player's velocity normalized
        //    normalizedVelocity = playerVelocity.normalized;
        //}
        //else
        //{
        //    // Otherwise, use the player's forward direction
        //    normalizedVelocity = m_Player.transform.forward;
        //}
        //   
        //// Float used to offset the camera's position on the y axis
        //float camHeightOffset;
        //
        //// Check if the player is currently ascending or decending
        //if (normalizedVelocity.y > 0f)
        //{
        //    // Moving upwards
        //    camHeightOffset = -3;
        //}
        //else
        //{
        //    // Moving downwards
        //    camHeightOffset = 3;
        //}

        //if (m_PositiveForwardVel && normalizedVelocity.y > 0.1f)
        //{
        //    // Moving forwards up incline
        //    camHeightOffset = -5;
        //}
        //else if(m_PositiveForwardVel && normalizedVelocity.y < -0.1f)
        //{
        //    // Moving forwards down incline
        //    camHeightOffset = 5;
        //}
        //else if(!m_PositiveForwardVel && normalizedVelocity.y > 0.1f)
        //{
        //    // Moving backwards up incline
        //    camHeightOffset = -5;
        //}
        //else
        //{
        //    // Moving backwards down incline
        //    camHeightOffset = 5;
        //}



        // Set the y component of the desired position to the calculated height + the y component of the player's velocity multiplied by the offset
        //m_DesiredPosition.y = currentHeight + (normalizedVelocity.y * camHeightOffset);

        // Set the camera's position
        transform.position = m_DesiredPosition;
        
        //transform.position = Vector3.Lerp(transform.position, m_DesiredPosition, m_FollowSpeed *  Time.deltaTime);

        //Look at camera
        transform.LookAt(m_Target.position + (Vector3.up * m_CamAngle));



        // Calculate a smoothed fov value based on the target's speed
        float fov = Mathf.SmoothStep(m_FovMin, m_FovMax, m_TargetSpeed * 0.005f);
        // Lerp the cam's fov
        m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, fov, ref m_CamFovVel, m_FovSmoothTime);

        CheckForWall();
    }

    void CheckForWall()
    {
        // Camera collision

        RaycastHit hitInfo;

        Vector3 direction = transform.position - m_Target.transform.position;
        float distance = direction.magnitude;
        Debug.DrawRay(m_Target.transform.position, direction, Color.green);
        if (Physics.Raycast(m_Target.position, direction, out hitInfo, distance, m_CollideWithCamera))
        {
            //Debug.Log("Camera raycast hit");
            
            float hitDist = hitInfo.distance;
            Vector3 sphereCenter = m_Target.transform.position + (direction.normalized * hitDist);
            transform.position = sphereCenter;
        }
    }

    public void WatchGhost()
    {
        m_Target = GameObject.FindGameObjectWithTag("Ghost").transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + m_DesiredPosition, 1f);
        

    }
}