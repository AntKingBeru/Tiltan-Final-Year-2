using UnityEngine;

public class BuildEndBehaviour : CharacterStateBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnBuildFinished();
    }
}