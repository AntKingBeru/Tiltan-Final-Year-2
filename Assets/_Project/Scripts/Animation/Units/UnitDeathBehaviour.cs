using UnityEngine;
using UnityEngine.AI;

public class UnitDeathBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
    }
}