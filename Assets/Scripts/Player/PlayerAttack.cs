using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
struct AttackData
{
    public AttackSpawner ability;
    public float cooldown;

    [HideInInspector]    
    public float lastUsed;
}

enum AttackBinding
{
    Primary,
    Secondary
}

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] AttackData primaryAttack;
    [SerializeField] AttackData secondayAttack;

    [SerializeField] LayerMask damageLayers;


    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        PollAttack(ref primaryAttack, AttackBinding.Primary);
        PollAttack(ref secondayAttack, AttackBinding.Secondary);
    }

    void PollAttack(ref AttackData data, AttackBinding binding)
    {
        bool attackButtonPressed = false;

        switch (binding)
        {
            case AttackBinding.Primary:
                attackButtonPressed = Mouse.current.leftButton.wasPressedThisFrame;
                break;
            case AttackBinding.Secondary:
                attackButtonPressed = Mouse.current.rightButton.wasPressedThisFrame;
                break;
        }

        if (attackButtonPressed && Time.time - data.lastUsed > data.cooldown)
        {
            Attack(data.ability);
            data.lastUsed = Time.time;
        }
    }

    void Attack(AttackSpawner spawner)
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = cam.transform.position.z;

        Vector2 attackDir = (cam.ScreenToWorldPoint(mouseScreenPos) - transform.position).normalized;

        spawner.SpawnAttackInstance(damageLayers, transform.position, Quaternion.FromToRotation(Vector2.right, attackDir));
    }
}
