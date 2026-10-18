using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable, IDespawnNotifier
{
    [SerializeField] int maxHealth;
    int health;

    public event Action<GameObject> OnDestroyRequested;

    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 _)
    {
        health -= damage;
        if (health <= 0) OnDestroyRequested?.Invoke(gameObject);
    }
}
