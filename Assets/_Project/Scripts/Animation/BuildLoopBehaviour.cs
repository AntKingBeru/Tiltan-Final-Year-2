using UnityEngine;

public class BuildLoopBehaviour : CharacterStateBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnBuildLoopStart();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnBuildLoopEnd();
    }
}