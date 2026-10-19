using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicFollowEnemy : EnemyStateMachine
{
    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    protected override EnemyState BuildTransitions()
    {
        return chaseState;
    }
}
