using System;
using System.Collections;
using UnityEngine;

public class JoshTeleport : MonoBehaviour, IDespawnNotifier, IDamagesLayers
{

    [SerializeField] float radius;
    [SerializeField] float delay;
    [SerializeField] AudioClip six7;

    public event Action<GameObject> OnDestroyRequested;

    LayerMask damageLayers;

    bool shouldFire = false;

    public void SetDamageLayers(LayerMask layers)
    {
        damageLayers = layers;
    }

    void OnEnable()
    {
        transform.position += transform.right * radius;
        shouldFire = true;
    }

    void Update()
    {
        if (shouldFire)
        {
            StartCoroutine(Fire());
            shouldFire = false;
        }
    }

    IEnumerator Fire()
    {
        GameManager.Instance.Player.position = transform.position;

        if (TryGetComponent<PlayerHealth>(out var health))
        {
            health.SetInvul(delay);
        }

        AudioManager.instance.PlaySound(six7);

        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var other))
            {
                other.TakeDamage(1000000, default);
            }
        }

        yield return new WaitForSeconds(delay);

        OnDestroyRequested?.Invoke(gameObject);
    }
}
