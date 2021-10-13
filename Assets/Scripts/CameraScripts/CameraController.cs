using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector2 m_Offset; // Cam Offset
    [SerializeField] private float m_Angle = 2.0f; // Cam Angle
    [SerializeField] private float m_SmoothTime = 2.0f; // Movement Smoothing Time
    public GameObject m_Target;
    private Vector3 m_DesiredPos;

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
    }

    // Late update since we're using physics and our calculations should be after everything else
    private void FixedUpdate()
    {
        Vector3 forward = m_Target.transform.forward;

        // Set the target position above the player of the camera
        m_DesiredPos = m_Target.transform.position + (Vector3.up * m_Offset.y) - (forward * m_Offset.x);
        

        transform.position = Vector3.Lerp(transform.position, m_DesiredPos, Time.deltaTime * m_SmoothTime);
        transform.LookAt(m_Target.transform.position + (Vector3.up * m_Angle), Vector3.up);

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
        Debug.DrawRay(m_Target.transform.position, Vector3.up * m_Offset.y, Color.green); // Line for offset on the y Axis
        Debug.DrawRay(m_Target.transform.position, -1f * m_Target.transform.transform.forward * m_Offset.x, Color.blue); // Line for offset on the x (z) technically
        Debug.DrawLine(m_Target.transform.position, m_Target.transform.position, Color.magenta); // Line to the cam target position

    }

    public void WatchGhost()
	{
        m_Target = GameObject.FindGameObjectWithTag("Ghost");
	}

    private void OnDrawGizmos()
    {
        
    }
}


