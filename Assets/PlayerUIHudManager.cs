using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_Stat_Bar staminaBar;

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(float maxStamina) 
    {
        staminaBar.SetMaxStat(Mathf.RoundToInt(maxStamina));
    }
}
