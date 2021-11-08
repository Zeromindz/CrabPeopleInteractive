using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    private Camera m_Cam;                               // Reference to the camera component to give access to the fieldOfView property 
    [SerializeField] private Transform m_Target;        // Target object transform
    private PlayerController m_Player;                  // Player controller so we can more easily get the current velocity 
    private Vector3 desiredPosition;                    // Desired position of the cam to lerp to


    [SerializeField] private float m_CamHeight = 5.0f;
    [SerializeField] private float m_CamDist = 15.0f;
    [SerializeField] private float m_CamAngle = 5f;
    [SerializeField] private float m_FollowSpeed = 10.0f; // Movement smoothing Time
    [SerializeField] private float m_RotationSpeed = 10.0f; // Look smoothing Time

    [Space(10)]
    [Header("FOV Settings")]
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    private float m_TargetSpeed;
    private float m_CamFovVel;
    private Vector3 m_TargetLastPosition;

    public LayerMask m_CollideWithCamera;


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
        Vector3 vel = m_Player.playerMovement.m_CurrentVel;

        if (!m_Player.playerMovement.m_AtTrickHeight || m_TargetSpeed < 0.1f)
        {
            // Use player's forward vector as the cam's distance offset direction
            desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (m_Target.forward * m_CamDist);
        }
        else
        {

            // If the player's speed is low and they aren't falling
            if (m_TargetSpeed < 0.1f && !m_Player.playerMovement.m_Grounded)
            {
                desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (m_Target.forward * m_CamDist);
            }
            else
            {
                // Use player's velocity normalised as the distance direction
                Vector3 dir = vel.normalized;
                desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (dir * m_CamDist);

            }
        }

        

        // Lerp between the cams current position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, m_FollowSpeed * Time.fixedDeltaTime);
        

        // Lerp camera rotation to the direction of the target
        Vector3 direction = (m_Target.position + (Vector3.up * m_CamAngle)) - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, m_RotationSpeed * Time.fixedDeltaTime);

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
        Gizmos.DrawWireSphere(desiredPosition, 1f);

    }
}