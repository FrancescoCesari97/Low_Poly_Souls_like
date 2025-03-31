using JetBrains.Annotations;
using System.ComponentModel;
using UnityEngine;

public class PlayerManager : CharacterManager
{

    PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        // if we do not own this gameobject, we do not control or edit it
        if (!IsOwner)
            return;

        // Handle movement
        playerLocomotionManager.HandleAllMovement();

    }

    protected override void LateUpdate()
    {
        if(!IsOwner)
            return;    

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // If this is the player object owned by this client 
        if (IsOwner)
        {
            PlayerCamera.instance.player = this;  

        }
    }
}
