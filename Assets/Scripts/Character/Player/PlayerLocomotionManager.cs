using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public float verticalMovement;

    PlayerManager player; 

    public float horizontalMovement;
    public float verticalmovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float rotationSpeed = 10;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public void HandleAllMovement() 
    {
        // Grounded Movement
        HandleGroundedMovement();
        HandleRotation();
        // Aerial MOvement
    }

    private void GetVerticalAndHorizontalInputs() 
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
    }

    private void HandleGroundedMovement() 
    {
        GetVerticalAndHorizontalInputs();

        // Get Forward Movement Direction
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;

        // Get Movement Direction and Add to Move Direction Above
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;

        // To ensures that the total movement doesn't exceed a magnitude of 1
        moveDirection.Normalize();

        moveDirection.y = 0;



        if (PlayerInputManager.Instance.moveAmount > 0.5f)
            // Running speed
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            // Walking speed
        {
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }


    }


    private void HandleRotation() 
    {
        targetRotationDirection = Vector3.zero;
        // The player's forward movement (W/S) is determined by the camera’s forward direction (transform.forward)
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;

        // The player's side movement (A/D) is determined by the camera’s right direction (transform.right).
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

        targetRotationDirection.Normalize();

        targetRotationDirection.y = 0;

        // If the player is not moving, keep the rotation unchanged (prevents snapping to (0,0,0)).
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        // Creates a new rotation that points in the direction of movement.
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);

        // Smoothly interpolates (rotates) from the current rotation to the new rotation over time, using rotationSpeed.
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = targetRotation;
    }
}
