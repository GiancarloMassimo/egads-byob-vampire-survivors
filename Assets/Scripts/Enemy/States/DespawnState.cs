using UnityEngine;
using System;

[Serializable]
public class DespawnState : EnemyState
{
    GameObject enemy;
    [SerializeField] float despawnTime;

    float timer;

    protected override void Init(GameObject enemy){
        this.enemy = enemy;
    }

    public override void Setup(){
        
    }
    public override void Cleanup() { }
    public override void Execute() { 
        timer += Time.deltaTime;
        if (timer > despawnTime){
            UnityEngine.Object.Destroy(enemy);
            RaiseExit();
        }
    }
    public override void ExecutePhysics() { }
}
