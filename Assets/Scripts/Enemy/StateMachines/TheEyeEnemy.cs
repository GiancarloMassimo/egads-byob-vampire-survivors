using UnityEngine;

public class TheEyeEnemy : EnemyStateMachine
{
    [SerializeField] float attackDistance;

    [SerializeReference, SubclassSelector] 
    EnemyState rotateState;

    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    EnemyState attackState;

    float distanceToPlayer = Mathf.Infinity;

    protected override EnemyState BuildTransitions()
    {
        AddTransition(attackState, () => distanceToPlayer < attackDistance, rotateState);
        AddTransitionOnExit(rotateState, attackState);
        
        return rotateState;
    }

    protected override void Update()
    {
        base.Update();

        distanceToPlayer = Vector2.Distance(GameManager.Instance.Player.position, transform.position);
    }
}
