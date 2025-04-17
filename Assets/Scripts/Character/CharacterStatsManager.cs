using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public int CalculateStaminaBasedOnEndurancelevel(int endurance) 
    {
        float stamina = 0;

        // equation to calculate the stamina

        stamina = endurance * 10;
        
        return Mathf.RoundToInt(stamina);
    }
}
