using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;

   //  1. find a way to read the values of a joystick
   //  2. move the character based of those values

    PlayerControls playerControls;

    [Header("Player Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    public float cameraMoveAmount;

    [Header("Player Action Input")]
    [SerializeField] bool dodgeInput = false;

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

        // When the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;

        FindPlayerInScene();

    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If we are loading into our world scene enble our player controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else 
        {
            instance.enabled = false;
        }
    }

    private void FindPlayerInScene()
    {
        // Find the player in the new scene
        player = Object.FindFirstObjectByType<PlayerManager>();

        if (player == null)
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    private void OnEnable()
    {
        if (playerControls == null) 
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // destroy this object, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // If we minimize or lower the window, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        { 
        
            if (focus)
            {
                playerControls.Enable();
            }
            else 
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandlePlayerMovementInput() 
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // Return the absolute number
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // Clamp the value to 0, 0.5, 1
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1; 
        }
        if (player == null)
            return;

        // 0 on the horizontal parameter because we get only NON-STRAFING movement
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

        // 
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;

    }

    private void HandleDodgeInput() 
    {
        if(dodgeInput == true) 
        {
            dodgeInput = false;

            // perform dodge 
        }
    }

}
