using System;
using UnityEngine;

[Serializable]
public class ChargeAttackState : EnemyState
{
    enum ChargeState {Waiting, Charging}
    [SerializeField] float chargeSpeed;
    [SerializeField] float waitDuration;
    [SerializeField] float chargeDuration;
    [SerializeField] int chargeDamage;
    [SerializeField] float knockbackForce;
    
    [SerializeField] Sprite chargeSprite = null;

    Transform target;
    Transform self;
    Rigidbody2D rb;

    private ChargeState currentState;
    private Vector2 chargeDirection;
    private float stateTimer;

    protected override void Init(GameObject enemy)
    {
        target = GameManager.Instance.Player;
        self = enemy.transform;
        rb = enemy.GetComponent<Rigidbody2D>();
    }

    public override void Setup() {
        currentState = ChargeState.Waiting;
        stateTimer = waitDuration;

        rb.linearVelocity = Vector2.zero;

        chargeDirection = (target.position - self.position).normalized;
    }

    public override void Cleanup()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public override void Execute() {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            ChangeState();
        }
    }

    public override void ExecutePhysics()
    {
        if (currentState == ChargeState.Charging)
        {
            rb.linearVelocity = chargeDirection * chargeSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void ChangeState()
    {
        switch (currentState)
        {
            case ChargeState.Waiting:
                if (chargeSprite != null) self.GetComponent<SpriteRenderer>().sprite = chargeSprite;
                currentState = ChargeState.Charging;
                stateTimer = chargeDuration;
                break;

            case ChargeState.Charging:
                RaiseExit();
                break;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(chargeDamage, rb.linearVelocity.normalized * knockbackForce);
        } 
    }
}
