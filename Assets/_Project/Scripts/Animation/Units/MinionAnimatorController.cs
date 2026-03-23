using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class MinionAnimatorController : UnitAnimator
{
    private static readonly int CanAttackHash = Animator.StringToHash("CanAttack");

    public override void TriggerAttack()
    {
        if (!animator.GetBool(CanAttackHash))
            return;
        
        base.TriggerAttack();
    }
    
    public void SetCanAttack(bool canAttack)
    {
        animator.SetBool(CanAttackHash, canAttack);
    }
}