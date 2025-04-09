using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSorce;

    protected virtual void Awake() 
    {
        audioSorce = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX() 
    {
        audioSorce.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
    }
}
