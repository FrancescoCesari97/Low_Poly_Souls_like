using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("Network Join")]
    [SerializeField] bool startGameAsClient;

    private void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        if (startGameAsClient) 
        {
            startGameAsClient = false;
            // must first shut down because we have started as a host during the title screen 
            NetworkManager.Singleton.Shutdown();
            // then restart as a client
            NetworkManager.Singleton.StartClient();
        }
    }
}
