using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class UnitAnimator : MonoBehaviour
{
    protected static readonly int SpeedHash = Animator.StringToHash("Speed");
    protected static readonly int InCombatHash = Animator.StringToHash("InCombat");
    protected static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    protected static readonly int AttackHash = Animator.StringToHash("Attack");
    protected static readonly int HitHash = Animator.StringToHash("Hit");

    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent agent;
    
    protected float SmoothSpeed;

    protected virtual void Awake()
    {
        animator.applyRootMotion = false;
    }

    protected virtual void Update()
    {
        UpdateMovement();
    }

    protected virtual void UpdateMovement()
    {
        if (IsDead())
        {
            animator.SetFloat(SpeedHash, 0f);
            return;
        }
        
        var targetSpeed = agent.velocity.magnitude;
        SmoothSpeed = Mathf.Lerp(SmoothSpeed, targetSpeed, Time.deltaTime * 10f);
        
        animator.SetFloat(SpeedHash, SmoothSpeed, 0.1f, Time.deltaTime);
    }
    
    protected virtual bool IsDead()
    {
        return animator.GetBool(IsDeadHash);
    }
    
    #region External API
    
    public virtual void SetCombat(bool inCombat)
    {
        animator.SetBool(InCombatHash, inCombat);
    }

    public virtual void TriggerAttack()
    {
        animator.SetTrigger(AttackHash);
    }
    
    public virtual void TriggerHit()
    {
        animator.SetTrigger(HitHash);
    }

    public virtual void Die()
    {
        animator.SetBool(IsDeadHash, true);
        
        agent.isStopped = true;
        agent.enabled = false;
    }

    #endregion
}