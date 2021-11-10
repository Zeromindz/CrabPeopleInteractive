using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    private InputManager playerInput;

    [Header("Game Objects")]
    private GameObject m_Player;
    public GameObject m_PortalPrefab;
    public Camera m_PortalCamera;
    public Transform m_PlayerCamera;
    [SerializeField] private Transform m_PortalExit;
    public Material m_PortalMaterial;
    private Transform m_Entrance;
    [Space(10)]
    public float m_SpawnDist = 30.0f;
    public bool m_PPressed;
    public bool m_PlayerOverlapping = false;
    public bool m_PortalHasBeenSpawned = false;

    float m_RotationVector;
    float m_CurrentAngle;

    int layerMask;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = m_Player.gameObject.GetComponent<InputManager>();
        layerMask = 1 << 10; // Bit shift index of layer 10 for bitmask
    }

    // Update is called once per frame
    void Update()
    {
        // Portal spawn set to button (P) for testing,
        // will eventually be bound to the start option of the main menu
        if (m_PPressed && !m_PortalHasBeenSpawned)
        {
             SpawnPortal();
        }

        // If the portal has been spawned, mirror main camera movement with portal camera
        if(m_PortalHasBeenSpawned)
        {
            MovePortalCamera();
        }

        // If player has entered portal trigger, teleport them
        if (m_PlayerOverlapping)
        {
            TeleportPlayer();
        }
    }

    void SpawnPortal()
    {
        // Get player's move direction
        Vector3 moveDir = m_Player.transform.forward;

        // Find postion to spawn portal
        Vector3 spawnPos = m_Player.transform.position + (moveDir * m_SpawnDist);
        //Raycast down from above that position, finding collision on the selected layer
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + (Vector3.up * 100f), -Vector3.up, out hit, layerMask))
        {
            Debug.DrawRay(spawnPos, -Vector3.up, Color.red);
            Debug.Log("Portal raycast hit!" + hit.collider.gameObject.name);
        }

        // Get portal prefabs collider height
        float portalHeight = m_PortalPrefab.GetComponentInChildren<BoxCollider>().size.y;

        // Set portal position to raycast hit point + portal collider halfheight, so it's on the ground nicely
        spawnPos = hit.point + (Vector3.up * (portalHeight / 2));

        GameObject portal = Instantiate(m_PortalPrefab, spawnPos, Quaternion.LookRotation(-moveDir, Vector3.up));

        m_Entrance = portal.transform;

        // Set up portal texture
        if (m_PortalCamera.targetTexture != null)
        {
            m_PortalCamera.targetTexture.Release();
        }

        m_PortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_PortalMaterial.mainTexture = m_PortalCamera.targetTexture;

        m_PortalHasBeenSpawned = true;
    }

    void MovePortalCamera()
    {
        // Set cam2's position and rotation to match cam1
        m_PortalCamera.transform.position = m_PlayerCamera.position;
        m_PortalCamera.transform.rotation = m_PlayerCamera.rotation;

        // Rotate cam 2 around portal, on the y axis with inverted z
        m_PortalCamera.transform.RotateAround(m_Entrance.transform.position, Vector3.up, 180f);
        // Calculate the local position of cam2 to portal
        Vector3 localPos = m_Entrance.transform.InverseTransformPoint(m_PortalCamera.transform.position);
        // Calculate local rotation of the camera to portal rotation
        Quaternion localRot = Quaternion.Inverse(m_Entrance.transform.rotation) * m_PortalCamera.transform.rotation;
        
        // Set cam2's position and rotation to the local offsets from the exit
        m_PortalCamera.transform.position = m_PortalExit.transform.TransformPoint(localPos);
        m_PortalCamera.transform.rotation = m_PortalExit.transform.rotation * localRot;

        m_PortalCamera.GetComponent<Camera>().fieldOfView = m_PlayerCamera.GetComponent<CameraController>().GetCamFOV();

    }

    void TeleportPlayer()
    {
        
        // Get the vector from the portal entrance to the player
        Vector3 portalToPlayer = m_Player.transform.position - m_Entrance.position;

        // Store the cameras current offset from the player. This is to help with a seamless teleport
        Vector3 playerCamOffset = m_PlayerCamera.position - m_Player.transform.position;
        // Store the cameras current rotation from the player.

        //Quaternion playerCamRotation = Quaternion.Inverse(m_Player.transform.rotation);
        m_RotationVector = m_PlayerCamera.eulerAngles.y;

        float desiredAngle = m_RotationVector;

        Quaternion currentRotation = Quaternion.Euler(0f, desiredAngle, 0f);

        // dot product checking if the player entered the portal from the right side
        float dotProduct = Vector3.Dot(m_Entrance.up, portalToPlayer);
        
        if (dotProduct < 0f)
        {
            // Find the difference in rotation between the entrance and exit and invert it
            float rotDifference = -Quaternion.Angle(m_Entrance.rotation, m_PortalExit.rotation);

            Debug.Log("Rot diff: " + rotDifference);
            // Add the rotation offset from entrance to exit 
            rotDifference += 180f;

            // Check the dot product of the portal entrance
            float dot = Vector3.Dot(m_Entrance.forward, Vector3.left);

            if(dot > 0f)
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
            m_Player.transform.position = m_PortalExit.position + posOffset;

            //Rotate the player's velocity to the exit portals forward direction
            Rigidbody rb = m_Player.GetComponent<Rigidbody>();
            Vector3 currentVel = rb.velocity;
            rb.velocity = m_PortalExit.forward * currentVel.magnitude;

            
            // Set camera position and rotation using the player's position + the camera offset calulated earlier
            m_PlayerCamera.transform.position = m_Player.transform.position + playerCamOffset;

            m_PlayerCamera.transform.rotation = currentRotation;

            m_PlayerOverlapping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_PortalExit.position, Vector3.one * 3.0f);
        Gizmos.DrawLine(m_PortalExit.position, m_PortalExit.position + m_PortalExit.transform.forward * 100.0f);

        if(m_Entrance)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(m_Entrance.position, Vector3.one * 3.0f);
            Gizmos.DrawLine(m_Entrance.position, m_Entrance.position + m_Entrance.transform.forward * 100.0f);
        }
    }
}
