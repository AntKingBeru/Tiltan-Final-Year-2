using UnityEngine;
using System.Collections;

public class FlameTrap : Trap
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float tickRate = 0.5f;

    protected override void Activate(Enemy enemy)
    {
       // TODO: play particle effect
       
        StartCoroutine(BurnArea());
    }

    private IEnumerator BurnArea()
    {
        var elapsed = 0f;
        
        while (elapsed < duration)
        {
            var count = Physics.OverlapSphereNonAlloc(
                transform.position,
                2f,
                HitsBuffer
            );

            for (var i = 0; i < count; i++)
            {
                if (HitsBuffer[i].TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(5f);
            }
            
            elapsed += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
    }
}