using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public InputSystem_Actions actions;
    public float PlayerSpeed; //sets player speed multiplier
    public float JumpForce; //determines how high jump is
    float movementX;
    Rigidbody2D rb;
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
        if (context.performed)
        {
            rb.linearVelocityY = JumpForce;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = movementX * PlayerSpeed;
    }
}
