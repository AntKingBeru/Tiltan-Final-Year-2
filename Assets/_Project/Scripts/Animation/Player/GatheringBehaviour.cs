using UnityEngine;

public class GatheringBehaviour : CharacterStateBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnGatherStart();
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var ctrl = GetController(animator);
        ctrl.OnGatherEnd();
    }
}