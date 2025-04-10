using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public float verticalMovement;

    PlayerManager player; 

    public float horizontalMovement;
    public float verticalmovement;
    public float moveAmount;


    [Header("Movement Setting")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float rotationSpeed = 10;

    [Header ("Dodge")]
    private Vector3 rollDirection;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.moveAmount.Value = verticalMovement;
        }
        else
        { 
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // if not lock on pass only the move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);



        }
          
    }
    public void HandleAllMovement() 
    {
        // Grounded Movement
        HandleGroundedMovement();
        HandleRotation();
        // Aerial MOvement
    }

    private void GetMovementValues() 
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement() 
    {
        if (!player.canMove)
            return;

        GetMovementValues();


        // Get Forward Movement Direction
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;

        // Get Movement Direction and Add to Move Direction Above
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;

        // To ensures that the total movement doesn't exceed a magnitude of 1
        moveDirection.Normalize();

        moveDirection.y = 0;



        if (PlayerInputManager.instance.moveAmount > 0.5f)
            // Running speed
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            // Walking speed
        {
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }


    }


    private void HandleRotation() 
    {
        if (!player.canRotate)
            return;
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

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;

        // if moving perform a roll 
        if (PlayerInputManager.instance.moveAmount > 0) 
        {
        
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            rollDirection.y = 0;
            rollDirection.Normalize();

            // find the rotation of the player
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            // applay the new rotation to the player
            player.transform.rotation = playerRotation;

            // perform a roll animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Foward_1", true, true);
        }
        // if stationary perform a step back
        else 
        {
            // perform a backstep animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Standing Dodge Backward", true, true);
        }
    }

    public void HandleSprinting() 
    {
        if (player.isPerformingAction) 
        {
            // set sprinting to flase

        }

        // If we are out of stamina, set sprinting to false


        // If we are moving set sprinting to true
        // If we are stationary set sprinting to flase 

    }
}
