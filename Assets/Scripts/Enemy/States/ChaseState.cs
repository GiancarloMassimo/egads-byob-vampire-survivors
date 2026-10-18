using System;
using UnityEngine;

[Serializable]
public class ChaseState : EnemyState
{
    [SerializeField] float speed;
    [SerializeField] float stopDistance;

    Transform target;
    Transform self;
    Rigidbody2D rb;

    protected override void Init(GameObject enemy)
    {
        target = GameManager.Instance.Player;
        self = enemy.transform;
        rb = enemy.GetComponent<Rigidbody2D>();
    }   

    public override void Setup() {}

    public override void Cleanup()
    {
        rb.linearVelocity = Vector2.zero;
    }
    
    public override void Execute() {}

    public override void ExecutePhysics()
    {
        rb.linearVelocity = (target.position - self.position).normalized * speed;
    }
}
