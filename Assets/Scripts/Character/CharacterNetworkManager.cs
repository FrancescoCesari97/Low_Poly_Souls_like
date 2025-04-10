using UnityEngine;
using Unity.Netcode;
using System.Runtime.CompilerServices;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager character;

    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPositionVelocity;

    public float networkPositionSmoothTime = 0.1f;

    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

   
    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }
    // A ServerRpc is a method that a client can call, but the server executes.
    /// When a client calls a ServerRpc, the request gets sent over the network to the server/host, which then executes the method.
    [ServerRpc]

    public void NotyfyTheServerOfActionAniamtionServerRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        // If this character is the host/server, then activate the client RPC
        if (IsServer)
        { 
         PLayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]  
    public void PLayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion) 
    {

        // This checks if the current client (the one this code is running on) is not the one who originally triggered the animation.
        // If it matches clientID, that means it’s the original sender, so it skips the animation to avoid duplication.
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
    
    }

    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion) 
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }
}
