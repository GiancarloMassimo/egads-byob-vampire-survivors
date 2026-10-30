using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum SlashMode
{
    Normal,
    Feedback
}

public class Slash : MonoBehaviour, IDespawnNotifier, IDamagesLayers
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    [SerializeField] int damage;
    [SerializeField] float knockbackForce;
    [SerializeField] float duration;
    [SerializeField] float distanceFromOrigin;
    [SerializeField] float radius;
    [SerializeField] SlashMode mode = SlashMode.Normal;
    [SerializeField] AudioClip feedbackSound;

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
    }

    IEnumerator SlashRoutine()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var other))
            {
                if (collider.TryGetComponent<Rigidbody2D>(out var otherRb))
                {
                    if (collider.TryGetComponent<EnemyStateMachine>(out var otherStateMachine))
                    {
                        if (mode == SlashMode.Feedback) StartCoroutine(ScuffedKnockback(otherStateMachine, otherRb));
                    }
                }
                other.TakeDamage(damage, transform.right * knockbackForce);
            }
        }

        yield return new WaitForSeconds(duration);
        OnDestroyRequested?.Invoke(gameObject);
    }

    IEnumerator ScuffedKnockback(EnemyStateMachine otherStateMachine, Rigidbody2D other)
    {
        AudioManager.instance.PlaySound(feedbackSound, 1f, 0f, true);
        otherStateMachine.enabled = false;
        other.linearVelocity = transform.right * knockbackForce;
        yield return _waitForSeconds0_5;
        other.linearVelocity = Vector2.zero;
        otherStateMachine.enabled = true;
    }
}
