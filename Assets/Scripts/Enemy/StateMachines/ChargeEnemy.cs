using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChargeEnemy : EnemyStateMachine
{
    [SerializeField] float chargeDistance;

    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    EnemyState chargeState;

    float distanceToPlayer = Mathf.Infinity;

    protected override EnemyState BuildTransitions()
    {
        AddTransition(chargeState, () => distanceToPlayer < chargeDistance, chaseState);
        AddTransitionOnExit(chaseState, chargeState);
        
        return chaseState;
    }

    protected override void Update()
    {
        base.Update();

        distanceToPlayer = Vector2.Distance(GameManager.Instance.Player.position, transform.position);
    }
}
