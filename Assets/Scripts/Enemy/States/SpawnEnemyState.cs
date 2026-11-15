using UnityEngine;
using System;

[Serializable]
public class SpawnEnemyState : EnemyState
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnTime;
    [SerializeField] float spawnRadius;
    [SerializeField] float cooltime;

    Transform self;
    Transform target;
    Rigidbody2D rb;
    float nextReadyTime; 

    public bool canSpawn => Time.time >= nextReadyTime;

    float stateTimer;

    protected override void Init(GameObject enemy){
        rb = enemy.GetComponent<Rigidbody2D>();
        target = GameManager.Instance.Player;
        self = enemy.transform;
    }
    
    public override void Setup(){
        
        stateTimer = spawnTime;
        rb.linearVelocity = Vector2.zero;
    }
    public override void Cleanup(){
        rb.linearVelocity = Vector2.zero;
    }

    public override void Execute(){
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f){
            SpawnEnemy();
            nextReadyTime = Time.time + cooltime;
            RaiseExit();
        }
    }

    void SpawnEnemy(){
        Vector2 direction = (target.position - self.position).normalized;
        Vector2 spawnPos = (Vector2)self.position + direction * spawnRadius;
        UnityEngine.Object.Instantiate(enemyPrefab, (Vector2)self.position + spawnPos, Quaternion.identity);
    }

    public override void ExecutePhysics(){}
    
    public override void OnCollisionEnter2D(Collision2D collision) { }
}