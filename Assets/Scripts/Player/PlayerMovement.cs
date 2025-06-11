using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float acceleration = 0.2f; // How quickly we reach max speed
    [SerializeField] private float deceleration = 0.1f; // How quickly we slow down
    [SerializeField] private float airAcceleration = 0.1f; // Slower acceleration in air
    [SerializeField] private float airDeceleration = 0.05f; // Slower deceleration in air
    [SerializeField] private float initialSpeed = 2f; // Starting speed when movement begins
    [SerializeField] private float airJumpForceMultiplier = 0.7f; // Each air jump will be 70% of the previous jump force
    private float currentSpeed = 0f;
    private float moveDirection;
    private int jumpsRemaining = 2; // Track available jumps
    private float currentJumpForce;
    
    private Controls controls;
    private InputAction moveAction;
    private InputAction jumpAction;
    
    private Rigidbody2D rb;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool isFacingRight = true;

    private void Awake()
    {
        controls = new Controls();
        moveAction = controls.Player.Move;
        jumpAction = controls.Player.Jump;
        jumpAction.Enable();
        moveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>().x;
        Jumping();
        FlipCharacter();
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveDirection * moveSpeed;
        float currentAcceleration = IsGrounded() ? acceleration : airAcceleration;
        float currentDeceleration = IsGrounded() ? deceleration : airDeceleration;
        
        // Accelerate or decelerate based on input
        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            // If we're just starting to move, use initial speed
            if (Mathf.Abs(currentSpeed) < 0.1f)
            {
                currentSpeed = Mathf.Sign(moveDirection) * initialSpeed;
            }
            // Then accelerate to full speed
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, currentAcceleration);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, currentDeceleration);
        }

        rb.linearVelocity = new Vector2(
            currentSpeed,
            rb.linearVelocityY
        );
    }

    private void Jumping()
    {
        if (IsGrounded())
        {
            jumpsRemaining = 2;
            currentJumpForce = jumpForce; // Reset jump force when grounded
        }

        if (jumpAction.triggered && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);
            jumpsRemaining--;
            currentJumpForce *= airJumpForceMultiplier; // Reduce force for next jump
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.2f), 0, groundLayer);
    }

    private void FlipCharacter()
    {
        if (moveDirection > 0 && !isFacingRight || moveDirection < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
