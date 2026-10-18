using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IDespawnNotifier, IDamagesLayers
{
    [SerializeField] float knockbackForce;
    [SerializeField] int damage;
    [SerializeField] float speed;

    [SerializeField] LayerMask walls;
    LayerMask damageLayers;

    Rigidbody2D rb;

    public event Action<GameObject> OnDestroyRequested;

    public void SetDamageLayers(LayerMask layers)
    {
        damageLayers = layers;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb.linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.MatchesLayer(collision.gameObject.layer, walls)) 
            OnDestroyRequested?.Invoke(gameObject);

        if (!Utils.MatchesLayer(collision.gameObject.layer, damageLayers)) return;

        if (collision.TryGetComponent<IDamageable>(out var other))
        {
            other.TakeDamage(damage, transform.right * knockbackForce);
            OnDestroyRequested?.Invoke(gameObject);
        }
    }
}
