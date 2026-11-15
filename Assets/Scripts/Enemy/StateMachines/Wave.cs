using UnityEngine;

public class Wave : EnemyStateMachine
{
    [SerializeReference, SubclassSelector] 
    EnemyState chargeState;

    [SerializeReference, SubclassSelector] 
    EnemyState despawnState;

    protected override EnemyState BuildTransitions()
    {
        AddTransitionOnExit(despawnState, chargeState);
        return chargeState;
    }
}
