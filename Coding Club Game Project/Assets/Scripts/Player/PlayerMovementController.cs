using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public InputSystem_Actions actions;

    [Header("Movement")]
    public float playerSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;          // upward speed applied while jumping
    public float maxJumpHoldTime = 0.2f;   // how long player can hold jump for extra height

    float movementX;
    Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform groundedChecker;
    public float groundCheckerRadius = 0.2f;
    public LayerMask groundLayer;
    bool isGrounded;

    bool isJumping;
    float jumpTimeCounter;

    void Awake()
    {
        actions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += Movement;
        actions.Player.Move.canceled += Movement;

        actions.Player.Jump.started += StartJump;
        actions.Player.Jump.canceled += StopJump;
    }

    void OnDisable()
    {
        actions.Player.Move.performed -= Movement;
        actions.Player.Move.canceled -= Movement;

        actions.Player.Jump.started -= StartJump;
        actions.Player.Jump.canceled -= StopJump;

        actions.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Movement(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
    }

    void StartJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpHoldTime;
            rb.linearVelocityY = jumpForce;
        }
    }

    void StopJump(InputAction.CallbackContext context)
    {
        isJumping = false;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundedChecker.position,
            groundCheckerRadius,
            groundLayer
        );

        rb.linearVelocityX = movementX * playerSpeed;

        if (isJumping && actions.Player.Jump.IsPressed())
        {
            if (jumpTimeCounter > 0f)
            {
                rb.linearVelocityY = jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (groundedChecker != null)
        {
            Gizmos.DrawWireSphere(groundedChecker.position, groundCheckerRadius);
        }
    }
}