using UnityEngine;

public abstract class CharacterStateBehaviour : StateMachineBehaviour
{
    protected CharacterAnimatorController Controller;

    protected CharacterAnimatorController GetController(Animator animator)
    {
        if (!Controller)
            Controller = animator.GetComponent<CharacterAnimatorController>();
        
        return Controller;
    }
}