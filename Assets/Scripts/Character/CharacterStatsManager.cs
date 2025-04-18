using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamima Regeneration")]
    private float staminaRegenerationTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;
    [SerializeField] float staminaTickTimer = 0;
    [SerializeField] float staminaRegenAmount = 2;

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }
    public int CalculateStaminaBasedOnEndurancelevel(int endurance) 
    {
        float stamina = 0;

        // equation to calculate the stamina

        stamina = endurance * 10;
        
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        // Only owners can edit their network variables
        if (!character.IsOwner)
            return;

        // We don't want to regenerate stamina if we are using it
        if (character.characterNetworkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenAmount;
                }
            }
        }

    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount) 
    {
        // reset the regeneration if the action comsume stamina 
        // reset the regeneration if we already regenerating stamina
        if (currentStaminaAmount < previousStaminaAmount) 
        {
            staminaRegenerationTimer = 0;
        }
    }
}
