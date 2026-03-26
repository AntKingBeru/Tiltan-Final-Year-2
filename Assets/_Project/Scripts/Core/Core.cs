using UnityEngine;
using System;

public class Core : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 500f;

    private float _currentHealth;
    
    public float CurrentHealth => _currentHealth;
    public float MaxMaxHealth => maxHealth;
    public float HealthPercent => _currentHealth / maxHealth;
    
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        EnemySpawner.Instance?.SetCore(this);
    }
    
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0f);

        OnHealthChanged?.Invoke(HealthPercent);

        if (_currentHealth <= 0)
            OnDestroyed();
    }

    private void OnDestroyed()
    {
        // TODO: Game Over logic
    }
}