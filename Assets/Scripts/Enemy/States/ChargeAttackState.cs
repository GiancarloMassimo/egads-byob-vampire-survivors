using UnityEngine;

public class ChargeAttackState : EnemyState
{
    enum ChargeState {Waiting, Charging}
    [SerializeField] float chargeSpeed;
    [SerializeField] float waitDuration;
    [SerializeField] float chargeDuration;
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
                currentState = ChargeState.Charging;
                stateTimer = chargeDuration;
                break;

            case ChargeState.Charging:
                RaiseExit();
                break;
        }

    }

}
