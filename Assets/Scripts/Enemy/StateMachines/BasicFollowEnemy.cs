using UnityEngine;
using System;

public class BasicFollowEnemy : EnemyStateMachine
{
    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    protected override EnemyState BuildTransitions()
    {
        return chaseState;
    }
}
