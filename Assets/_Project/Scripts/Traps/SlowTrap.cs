using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SlowTrap : Trap
{
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float duration = 2f;

    protected override void Activate(Enemy enemy)
    {
        base.Activate(enemy);
        
        // TODO: play particle effect

        var agent = enemy.GetAgent();
        
        if (agent)
            StartCoroutine(ApplySlow(agent));
    }

    private IEnumerator ApplySlow(NavMeshAgent agent)
    {
        var original = agent.speed;
        agent.speed *= slowAmount;
        
        yield return new WaitForSeconds(duration);
        
        agent.speed = original;
    }
}
