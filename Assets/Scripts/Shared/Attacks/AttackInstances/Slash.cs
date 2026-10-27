using System;
using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour, IDespawnNotifier, IDamagesLayers
{
    [SerializeField] float spinSpeed;
    [SerializeField] int damage;
    [SerializeField] float knockbackForce;
    [SerializeField] float duration;
    [SerializeField] float distanceFromOrigin;
    [SerializeField] float radius;

    public event Action<GameObject> OnDestroyRequested;
    LayerMask damageLayers;

    bool shouldSlash = false;

    public void SetDamageLayers(LayerMask layers)
    {
        damageLayers = layers;
    }

    void OnEnable()
    {
        shouldSlash = true;
        transform.position += transform.right * distanceFromOrigin;
    }

    void Update()
    {
        if (shouldSlash)
        {
            StartCoroutine(SlashRoutine());
            shouldSlash = false;
        }
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + spinSpeed * Time.deltaTime);
    
        if (spinSpeed != 0)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var other))
                {
                    other.TakeDamage(damage, transform.right * knockbackForce);
                }
            }
        }
    }

    IEnumerator SlashRoutine()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var other))
            {
                other.TakeDamage(damage, transform.right * knockbackForce);
                OnDestroyRequested?.Invoke(gameObject);
            }
        }

        yield return new WaitForSeconds(duration);
        OnDestroyRequested?.Invoke(gameObject);
    }
}
