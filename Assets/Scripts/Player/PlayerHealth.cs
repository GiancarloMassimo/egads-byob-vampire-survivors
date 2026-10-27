using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int hp;
    [SerializeField] int maxHP;

    [SerializeField] float knockBackDuration;

    [SerializeField] Slider healthBarSlider;

    bool isStunned;

    PlayerMovement playerMovement;
    Rigidbody2D rb;

    IEnumerator StunAndKnockback(Vector2 knockback, float duration)
    {
        isStunned = true;
        rb.linearVelocity = knockback;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
    
    public void TakeDamage(int damage, Vector2 knockback = default)
    {
        if (isStunned) return;
        
        hp -= damage;
        StartCoroutine(StunAndKnockback(knockback, knockBackDuration));
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        healthBarSlider.value = hp / (float)maxHP;
        
        if (isStunned)
        {
            playerMovement.enabled = false;   
        }
        else
        {
            playerMovement.enabled = true;
        }
    }
}