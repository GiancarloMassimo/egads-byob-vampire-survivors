using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Vector2 moveInput;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetMoveInput();
    }

    void GetMoveInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.aKey.isPressed) moveInput += Vector2.left; 
        if (Keyboard.current.dKey.isPressed) moveInput += Vector2.right;
        if (Keyboard.current.wKey.isPressed) moveInput += Vector2.up;
        if (Keyboard.current.sKey.isPressed) moveInput += Vector2.down;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
}
