using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera cameraObject;
    public PlayerManager player;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1; // the bigger the number the slower the camera will be to reach the target (player)  
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownrotationSpeed = 220;
    [SerializeField] float minimunPivot = -30;  // lowest point able to look down
    [SerializeField] float maximunPivot = 60;   // highest point able to look up

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float UpAndDownLookAngle;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);   
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void HandleAllCameraActions() 
    {
        if (player != null)
        {
            // Follow the player
            FollowPlayer();
            // Rotate around the player
            HandleRotations();
            // collide with the invarionment (cannot pass through walls)


        }
    }

    private void FollowPlayer() 
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp( // function that gradually moves a vector from a start position to a target position, ensuring smooth movement.
        transform.position,        // Current camera position
        player.transform.position, // Target position (player's position)
        ref cameraVelocity,        // Reference to velocity (used internally by SmoothDamp)
        cameraSmoothSpeed * Time.deltaTime // Smooth time factor
);

        transform.position = targetCameraPosition;
    }

    private void HandleRotations() 
    {
         // if locked on, force rotation towards the target

         // Normal Rotations
         //leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput) 
    }
}
