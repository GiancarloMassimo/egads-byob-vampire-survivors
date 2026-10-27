using System;
using UnityEngine;

[Serializable]
public class SpawnAttackState : EnemyState
{
    [SerializeField] LayerMask damageLayers;
    [SerializeField] string attackSpawnerTag;
    AttackSpawner attack;
    [SerializeField] int maxAttacks;
    [SerializeField] float attackInterval;

    Transform target;
    Transform self;    

    int attackCounter = 0;
    float attackTimer;

    protected override void Init(GameObject enemy)
    {
        attack = GameManager.Instance.GetAttack(attackSpawnerTag);
        target = GameManager.Instance.Player;
        self = enemy.transform;
    }

    public override void Setup()
    {
        attackCounter = 0;
        attackTimer = attackInterval;
    }

    public override void Cleanup() {}

    public override void Execute() {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            Attack();

            attackTimer = attackInterval;
            if (++attackCounter == maxAttacks) RaiseExit();
        }
    }

    void Attack()
    {
        Vector2 attackDir = (target.position - self.position).normalized;
        attack.SpawnAttackInstance(damageLayers, self.position, Quaternion.FromToRotation(Vector2.right, attackDir));
    }

    public override void ExecutePhysics() {}
}
