using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1; // the bigger the number the slower the camera will be to reach the target (player)  
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownrotationSpeed = 220;
    [SerializeField] float minimunPivot = -30;  // lowest point able to look down
    [SerializeField] float maximunPivot = 60;   // highest point able to look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;


    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; // Used for camera collision (move sthe camera to this position when colliding) 
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;  // Values used for camera colisions
    private float targetCameraZPosiotion;  // Values used for camera colisions
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
        cameraZPosition = cameraObject.transform.localPosition.z;

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
            HandleCollisions();

        }
    }

    private void FollowPlayer() 
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp( // function that gradually moves a vector from a start position to a target position, ensuring smooth movement.
        transform.position,        // Current camera position
        player.transform.position, // Target position (player's position)
        ref cameraVelocity,        // Reference to velocity (used internally by SmoothDamp)
        cameraSmoothSpeed * Time.deltaTime); // Smooth time factor


        transform.position = targetCameraPosition;
    }

    private void HandleRotations() 
    {
        // if locked on, force rotation towards the target

        // Normal Rotations
        // Rotate left and right based on the horizontal movement of the camera inputs
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

        // Rotate left and right based on the vertical movement of the camera inputs
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownrotationSpeed) * Time.deltaTime;

        // Clamp the up and down angle between a min and max value
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimunPivot, maximunPivot);



        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // rotate this gameobject on the left and right
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // rotate the pivot gameobject up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }


    private void HandleCollisions() 
    {
        // targetCameraZPosiotion is initially set to cameraZPosition, which represents the camera's default distance from the pivot.
        targetCameraZPosiotion = cameraZPosition;
        RaycastHit hit;

        // Calculates the direction from the pivot to the camera.
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();


        // Check if there is an objet in frot our desired direction found above
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosiotion), collideWithLayers)) 
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);  // Calculates the distance from the pivot to the detected obstacle.
            targetCameraZPosiotion = -(distanceFromHitObject - cameraCollisionRadius);        // Moves the camera forward so that it stops just before hitting the object.
        }

        // Ensures that the camera never gets too close to the pivot, preventing extreme clipping
        if (Mathf.Abs(targetCameraZPosiotion) < cameraCollisionRadius)
        { 
            targetCameraZPosiotion = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosiotion, 0f);  // Smoothly interpolates (Lerp) the camera's position toward the new valid position to avoid sudden snapping.
        cameraObject.transform.localPosition = cameraObjectPosition;   // Updates the camera's local position to prevent clipping.
    }
}
