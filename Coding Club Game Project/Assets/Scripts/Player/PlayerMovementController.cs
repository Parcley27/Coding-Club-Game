using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public InputSystem_Actions actions;
    public float playerSpeed; //sets player speed multiplier
    public float jumpForce; //determines how high jump is
    float movementX;
    Rigidbody2D rb;
    public Transform groundedChecker;
    public float groundCheckerRadius;
    public LayerMask groundLayer;
    bool isGrounded;
    void Awake()
    {
        actions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += Movement;
        actions.Player.Jump.performed += Jump;

        actions.Player.Move.canceled += Movement;
        actions.Player.Jump.canceled += Jump;
    }
    void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= Movement;
        actions.Player.Jump.performed -= Jump;
    }
    void Movement(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundedChecker.position, groundCheckerRadius, groundLayer);
        rb.linearVelocityX = movementX * playerSpeed;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireSphere(groundedChecker.position, groundCheckerRadius);
    }
}
