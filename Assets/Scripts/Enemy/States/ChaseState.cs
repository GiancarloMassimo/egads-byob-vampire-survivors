using System;
using UnityEngine;

[Serializable]
public class ChaseState : EnemyState
{
    [SerializeField] float speed;
    [SerializeField] float stopDistance;
    [SerializeField] float acceleration;
    float actualSpeed;

    Transform target;
    Transform self;
    Rigidbody2D rb;

    protected override void Init(GameObject enemy)
    {
        target = GameManager.Instance.Player;
        self = enemy.transform;
        rb = enemy.GetComponent<Rigidbody2D>();
        actualSpeed = speed;
    }   

    public override void Setup() {}

    public override void Cleanup()
    {
        actualSpeed = speed;
        rb.linearVelocity = Vector2.zero;
    }
    
    public override void Execute() {}

    public override void ExecutePhysics()
    {
        if (actualSpeed < 100) {
            actualSpeed += Time.fixedDeltaTime * acceleration;
        } 

        rb.linearVelocity = (target.position - self.position).normalized * actualSpeed;
    }
}
