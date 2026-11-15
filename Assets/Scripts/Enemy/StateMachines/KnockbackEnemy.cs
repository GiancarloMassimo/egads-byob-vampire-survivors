using UnityEngine;

public class KnockbackEnemy : EnemyStateMachine
{
    [SerializeField] float attackDistance;

    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    EnemyState attackState;

    float distanceToPlayer = Mathf.Infinity;

    protected override EnemyState BuildTransitions()
    {
        AddTransition(attackState, () => distanceToPlayer < attackDistance, chaseState);
        AddTransitionOnExit(chaseState, attackState);
        
        return chaseState;
    }

    protected override void Update()
    {
        base.Update();

        distanceToPlayer = Vector2.Distance(GameManager.Instance.Player.position, transform.position);
    }
}
