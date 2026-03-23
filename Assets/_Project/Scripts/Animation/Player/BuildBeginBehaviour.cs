using UnityEngine;

public class BuildBeginBehaviour : CharacterStateBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnBuildBegin();
    }
}