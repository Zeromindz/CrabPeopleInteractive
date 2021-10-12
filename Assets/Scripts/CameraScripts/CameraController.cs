using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector2 m_Offset = new Vector2(10, 5); // Cam Offset
    [SerializeField] private float m_Angle = 2.0f; // Cam Angle
    [SerializeField] private float m_SmoothTime = 2.0f; // Movement Smoothing Time
    private Transform m_Player;
    private Vector3 m_TargetPosition;

    public bool m_InMenu;

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
        m_InMenu = true;
        m_Player = GameObject.FindWithTag("Player").transform;
        
    }

    // Late update since we're using physics and our calculations should be after everything else
    private void FixedUpdate()
    {
        //Vector3 forward = m_Player.gameObject.GetComponent<PlayerController>().playerMovement.GetCurrentVel().normalized;

        //forward.Scale(new Vector3(1f, 0f, 1f));

        Vector3 forward = m_Player. forward;

        // Set the target position above the player of the camera
        m_TargetPosition = m_Player.position + (Vector3.up * m_Offset.y) - (forward * m_Offset.x);

        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Time.deltaTime * m_SmoothTime);

        transform.LookAt(m_Player.position + (Vector3.up * m_Angle), Vector3.up);

        if (m_InMenu)
        {
            // menustuff
        }
        else
        {
            //play mode
            
        }

        //Testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_InMenu = !m_InMenu; // toggles onoff at each press
        }

        // Debug line drawing;
        Debug.DrawRay(m_Player.position, Vector3.up * m_Offset.y, Color.green); // Line for offset on the y Axis
        Debug.DrawRay(m_Player.position, -1f * forward * m_Offset.x, Color.blue); // Line for offset on the x (z) technically
        Debug.DrawLine(m_Player.position, m_TargetPosition, Color.magenta); // Line to the cam target position

    }

    public void WatchGhost()
	{
        m_Player = GameObject.FindGameObjectWithTag("Ghost").transform;
	}
}


