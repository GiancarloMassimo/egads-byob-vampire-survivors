using UnityEngine;

public class RangedEnemy : EnemyStateMachine
{
    [SerializeField] float shootDistance;

    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    EnemyState ShootState;

    float distanceToPlayer = Mathf.Infinity;

    protected override EnemyState BuildTransitions()
    {
        AddTransition(ShootState, () => distanceToPlayer < shootDistance, chaseState);
        AddTransitionOnExit(chaseState, ShootState);
        
        return chaseState;
    }

    protected override void Update()
    {
        base.Update();

        distanceToPlayer = Vector2.Distance(GameManager.Instance.Player.position, transform.position);
    }
}
