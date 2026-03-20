using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SlowTrap : Trap
{
    [Header("Slow Settings")]
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float duration = 2f;

    protected override void Activate(Enemy enemy)
    {
        base.Activate(enemy);
        
        // TODO: play particle effect

        var agent = enemy.GetAgent();

        if (!agent)
            return;

        var effectiveSlow = slowMultiplier;

        if (agent)
            effectiveSlow *= (1f - enemy.EnemyType.slowResistance);
        
        StartCoroutine(ApplySlow(agent, effectiveSlow));
    }

    private IEnumerator ApplySlow(NavMeshAgent agent, float multiplier)
    {
        var originalSpeed = agent.speed;
        
        agent.speed *= multiplier;
        
        yield return new WaitForSeconds(duration);
        
        // Safety check for agent
        if (agent)
            agent.speed = originalSpeed;
    }
}
