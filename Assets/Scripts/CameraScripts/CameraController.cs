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
    [SerializeField] private float m_CamDist = 10.0f;
    [SerializeField] private float m_CamAngle = 5f;
    [SerializeField] private float m_FollowSpeed = 2.0f; // Movement smoothing Time
    [SerializeField] private float m_RotationSpeed = 2.0f; // Look smoothing Time

    [Space(10)]
    [Header("FOV Settings")]
    [SerializeField] private float m_FovMin = 60.0f;
    [SerializeField] private float m_FovMax = 75.0f;
    [SerializeField] private float m_FovSmoothTime = 0.5f;
    public float m_TargetSpeed;
    private float m_CamFovVel;
    private Vector3 m_TargetLastPosition;

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
        m_Player = m_Target.gameObject.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // Store the targets speed in m/s, ignoring the y component of the velocity
        m_TargetSpeed = Vector3.Dot(m_Player.playerMovement.GetCurrentVel(), m_Target.forward);

        // Store the target's direction of movement
        Vector3 vel = m_Player.playerMovement.GetCurrentVel();

        if (!m_Player.playerMovement.m_AtTrickHeight || vel.magnitude < 0.1f)
        {
            // If the player is below the trick height, use their forward vector as the cam's distance offset direction
            desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (m_Target.forward * m_CamDist);
        }
        else
        {
            // Otherwise, use the player's velocity normalised as the distance direction
            Vector3 dir = vel.normalized;
            desiredPosition = m_Target.position + (Vector3.up * m_CamHeight) - (dir * m_CamDist);
        }

        // Lerp between the cams current position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, m_FollowSpeed * Time.deltaTime);

        // Lerp camera rotation to the direction of the target
        Vector3 direction = (m_Target.position + (Vector3.up * m_CamAngle)) - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, m_RotationSpeed * Time.deltaTime);

        // Calculate a smoothed fov value based on the target's speed
        float fov = Mathf.SmoothStep(m_FovMin, m_FovMax, m_TargetSpeed * 0.005f);
        // Lerp the cam's fov
        m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, fov, ref m_CamFovVel, m_FovSmoothTime);

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