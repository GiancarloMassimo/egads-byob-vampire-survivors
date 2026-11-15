using UnityEngine;

public class SpawnerEnemy : EnemyStateMachine
{
    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    SpawnEnemyState spawnState;

    protected override EnemyState BuildTransitions(){
        AddTransition(spawnState, () => spawnState.canSpawn, chaseState);
        AddTransitionOnExit(chaseState, spawnState);

        return chaseState;
    }
}
