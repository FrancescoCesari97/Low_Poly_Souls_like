using UnityEngine;
using UnityEngine.UI;
public class UI_Stat_Bar : MonoBehaviour
{
    private Slider slider;

    protected virtual void Awake() 
    {
        slider = GetComponent<Slider>();    
    }

    public virtual void SetStat(int newValue)
    { 
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue) 
    {
        slider.maxValue = maxValue;
    }
}
