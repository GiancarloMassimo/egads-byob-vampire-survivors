using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] AttackSpawner attack;
    [SerializeField] LayerMask damageLayers;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) Attack();
    }

    void Attack()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = cam.transform.position.z;

        Vector2 attackDir = (cam.ScreenToWorldPoint(mouseScreenPos) - transform.position).normalized;

        attack.SpawnAttackInstance(damageLayers, transform.position, Quaternion.FromToRotation(Vector2.right, attackDir));
    }
}
