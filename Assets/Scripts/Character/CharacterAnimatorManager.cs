using JetBrains.Annotations;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applayRootMotion = true) 
    {
        character.animator.applyRootMotion = applayRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        // Stop character from attempting new actions
        // for example, if you get damaged, and begin perfoming a damage animation
        // check this condition before attempting new actions
        character.isPerformingAction = isPerformingAction;
    }
}
