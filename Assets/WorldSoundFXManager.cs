using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
   public static WorldSoundFXManager Instance;

    [Header("Action Sounds")]
    public AudioClip rollSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
}
