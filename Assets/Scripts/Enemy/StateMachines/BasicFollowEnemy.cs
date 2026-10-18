using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicFollowEnemy : EnemyStateMachine
{
    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override EnemyState BuildTransitions()
    {
        return chaseState;
    }

    protected override void OnGetFromPool() {}
    protected override void OnReturnToPool() {}
}
