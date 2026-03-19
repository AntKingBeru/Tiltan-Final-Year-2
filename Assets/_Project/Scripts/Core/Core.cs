using UnityEngine;

public class Core : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 500f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            OnDestroyed();
    }

    private void OnDestroyed()
    {
        // TODO: Game Over logic
    }
}