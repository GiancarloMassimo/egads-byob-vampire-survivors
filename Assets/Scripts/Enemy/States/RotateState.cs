using UnityEngine;
using System;

[Serializable]
public class RotateState : EnemyState
{
    [SerializeField] float speed;
    [SerializeField] float radius;

    Rigidbody2D rb;
    Vector2 center;
    float angle;

    protected override void Init(GameObject enemy){
        rb = enemy.GetComponent<Rigidbody2D>();
        center = rb.position;
    }
    
    public override void Setup(){}
    public override void Cleanup(){
        rb.linearVelocity = Vector2.zero;
    }
    public override void Execute(){}
    public override void ExecutePhysics(){

        angle += speed * Time.fixedDeltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        rb.MovePosition(center + new Vector2(x, y));
    }
    
    public override void OnCollisionEnter2D(Collision2D collision) { }
}
