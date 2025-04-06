using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;

    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterNetworkManager characterNetworkManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool canRotate = true;
    public bool canMove = true;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);  

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
        
        characterNetworkManager = GetComponent<CharacterNetworkManager>();  
    }

    protected virtual void Update() 
    {
        // if character is being controlled from our side, then assign it's network position to the position of our transform
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // if this character is being controlled from else where, then assing it's position here locallly by the position of it's network transform 
        else 
        {
            // Position
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);

            // Rotation
            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
    
    }
}
