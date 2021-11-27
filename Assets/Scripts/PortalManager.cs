using System.Collections;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public enum PortalStates
    {
        VOID,
        STARTSPAWNED,
        ENDSPAWNED
    }

    public static PortalManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    private InputManager playerInput;

    [Header("Game Objects")]
    private GameObject m_Player = null;
    public GameObject m_PortalPrefab = null;
    public Camera m_PortalCamera = null;
    public Transform m_PlayerCamera = null;
    public Material m_PortalMaterial = null;
    private Transform m_StartPortalEntrance;
    [SerializeField] private Transform m_StartPortalExit = null;
    [SerializeField] private Transform m_GatePortalEntrance = null;
    [SerializeField] private Transform m_GatePortalExit = null;
    [Space(10)]
    public float m_SpawnDist = 30.0f;
    public float m_SpawnHeight = 30.0f;
    public float m_DissolveDuration = 5.0f;
    private float m_DissolveAmount = 1f;
    public bool m_PPressed;
    public bool m_PlayerOverlapping = false;

    private GameObject m_SpawnedPortal;

    float m_RotationVector;
    float m_CurrentAngle;

    public LayerMask m_LayerMask;

    private PortalStates m_State;
    public int GetState() { return (int)m_State; }
    

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = m_Player.gameObject.GetComponent<InputManager>();

        m_State = PortalStates.VOID;
    }

    // Update is called once per frame
    void Update()
    {
        // Portal spawn set to button (P) for testing,
        // will eventually be bound to the start option of the main menu
        if (m_PPressed && m_State == PortalStates.VOID)
        {
            SpawnPortal();
            m_PPressed = false;

            m_PortalMaterial.SetFloat("_Step_Cutoff", m_DissolveAmount);
            
            StartCoroutine(LerpDissolve(0f, m_DissolveDuration));
        }

        
        // If the portal has been spawned, mirror main camera movement with portal camera
        if (m_State == PortalStates.STARTSPAWNED)
        {
            MovePortalCamera(m_StartPortalEntrance, m_StartPortalExit);
        }

        if (m_State == PortalStates.ENDSPAWNED)
        {
            MovePortalCamera(m_GatePortalEntrance, m_GatePortalExit);
        }

        // If player has entered portal trigger, teleport them
        if (m_State == PortalStates.STARTSPAWNED && m_PlayerOverlapping)
        {
            TeleportPlayer(m_StartPortalEntrance, m_StartPortalExit);
            m_State = PortalStates.ENDSPAWNED;
        }

        // If player has entered the end portal trigger, teleport them
        if (m_State == PortalStates.ENDSPAWNED && m_PlayerOverlapping)
        {
            TeleportPlayer(m_GatePortalEntrance, m_GatePortalExit);
            m_State = PortalStates.VOID;
        }

        Debug.Log("Portal State: " + m_State);
    }

    IEnumerator LerpDissolve(float _endValue, float _duration)
    {
        float time = 0;
        float startValue = m_PortalMaterial.GetFloat("_Step_Cutoff");

        while (time < _duration)
        {
            m_PortalMaterial.SetFloat("_Step_Cutoff", Mathf.Lerp(startValue, _endValue, time / _duration));
            time += Time.deltaTime;
            yield return null;
        }
        m_PortalMaterial.SetFloat("_Step_Cutoff", 0f);
    }

    void SpawnPortal()
    {
        // Get player's move direction
        Vector3 moveDir = m_Player.transform.forward;

        // Find postion to spawn portal
        Vector3 spawnPos = m_Player.transform.position + (moveDir * m_SpawnDist);
        //Raycast down from above that position, finding collision on the selected layer
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + (Vector3.up * m_SpawnHeight), -Vector3.up, out hit, m_LayerMask))
        {
            Debug.DrawRay(spawnPos, -Vector3.up, Color.red);
            Debug.Log("Portal raycast hit!" + hit.collider.gameObject.name);
        }

        // Get portal prefabs collider height
        float portalHeight = m_PortalPrefab.GetComponentInChildren<BoxCollider>().size.y;

        // Set portal position to raycast hit point + portal collider halfheight, so it's on the ground nicely
        spawnPos = hit.point + (Vector3.up * (portalHeight / 2 - 15f));

        m_SpawnedPortal = Instantiate(m_PortalPrefab, spawnPos, Quaternion.LookRotation(-moveDir, Vector3.up));

        // For debugging
        m_StartPortalEntrance = m_SpawnedPortal.transform;

        // Set up portal texture
        if (m_PortalCamera.targetTexture != null)
        {
            m_PortalCamera.targetTexture.Release();
        }

        m_PortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_PortalMaterial.SetTexture("_End_Texture", m_PortalCamera.targetTexture);

        m_State = PortalStates.STARTSPAWNED;
    }

    void MovePortalCamera(Transform _portalEntrance, Transform _portalExit)
    {
        // Set cam2's position and rotation to match cam1
        m_PortalCamera.transform.position = m_PlayerCamera.position;
        m_PortalCamera.transform.rotation = m_PlayerCamera.rotation;

        // Rotate cam 2 around portal, on the y axis with inverted z
        m_PortalCamera.transform.RotateAround(_portalEntrance.position, Vector3.up, 180f);
        // Calculate the local position of cam2 to portal
        Vector3 localPos = _portalEntrance.InverseTransformPoint(m_PortalCamera.transform.position);
        // Calculate local rotation of the camera to portal rotation
        Quaternion localRot = Quaternion.Inverse(_portalEntrance.rotation) * m_PortalCamera.transform.rotation;

        // Set cam2's position and rotation to the local offsets from the exit
        m_PortalCamera.transform.position = _portalExit.TransformPoint(localPos);
        m_PortalCamera.transform.rotation = _portalExit.rotation * localRot;

        m_PortalCamera.GetComponent<Camera>().fieldOfView = m_PlayerCamera.GetComponent<CameraController>().GetCamFOV();

    }

    void TeleportPlayer(Transform _portalEntrance, Transform _portalExit)
    {
        // Get the vector from the portal entrance to the player
        Vector3 portalToPlayer = m_Player.transform.position - _portalEntrance.position;

        // Store the cameras current offset from the player. This is to help with a seamless teleport
        Vector3 playerCamOffset = m_PlayerCamera.position - m_Player.transform.position;
        // Store the cameras current rotation from the player.

        // dot product checking if the player entered the portal from the right side
        float dotProduct = Vector3.Dot(_portalEntrance.up, portalToPlayer);

        if (dotProduct < 0f)
        {
            // Find the difference in rotation between the entrance and exit and invert it
            float rotDifference = -Quaternion.Angle(_portalEntrance.rotation, _portalExit.rotation);

            Debug.Log("Rot diff: " + rotDifference);
            // Add the rotation offset from entrance to exit 
            rotDifference += 180f;

            // Check the dot product of the portal entrance
            float dot = Vector3.Dot(_portalEntrance.forward, Vector3.left);

            if (dot > 0f)
            {
                rotDifference *= -1f;
            }

            Debug.Log("Rot diff: " + rotDifference);

            //Rotate player with the calculated rotation
            m_Player.transform.Rotate(Vector3.up, rotDifference);

            // Create and offset with the already calculated player position offset from the portal,
            // rotated around the y by the rot difference 
            Vector3 posOffset = Quaternion.Euler(0f, rotDifference, 0f) * portalToPlayer;

            // Set player's position to the exit's position + offset
            m_Player.transform.position = _portalExit.position + posOffset;
            m_Player.GetComponent<PlayerController>().playerMovement.m_BullShitLevelBoatBool = true;
            //Rotate the player's velocity to the exit portals forward direction
            Rigidbody rb = m_Player.GetComponent<Rigidbody>();
            Vector3 currentVel = rb.velocity;
            rb.velocity = _portalExit.forward * currentVel.magnitude;

            // Set camera position and rotation using the player's position + the camera offset calulated earlier
            m_PlayerCamera.transform.position = m_Player.transform.position + playerCamOffset;

            //m_PlayerCamera.transform.rotation = currentRotation;
            m_PlayerCamera.transform.Rotate(Vector3.up, rotDifference);

            m_PlayerOverlapping = false;

            if (m_State == PortalStates.STARTSPAWNED)
            {
                Destroy(m_SpawnedPortal);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_StartPortalExit.position, Vector3.one * 3.0f);
        Gizmos.DrawLine(m_StartPortalExit.position, m_StartPortalExit.position + m_StartPortalExit.transform.forward * 100.0f);

        if (m_StartPortalEntrance)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(m_StartPortalEntrance.position, Vector3.one * 3.0f);
            Gizmos.DrawLine(m_StartPortalEntrance.position, m_StartPortalEntrance.position + m_StartPortalEntrance.transform.forward * 100.0f);
        }
    }

    public void SpawnPortalFromMenu()
	{
        if(m_State == PortalStates.VOID)
		{
            SpawnPortal();

            m_PortalMaterial.SetFloat("_Step_Cutoff", m_DissolveAmount);

            StartCoroutine(LerpDissolve(0f, m_DissolveDuration));
		}
    }
    
}




