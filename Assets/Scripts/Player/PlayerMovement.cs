using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    public float groundMoveSpeed = 10f;
    public float groundAcceleration = 15f;
    public float groundDeceleration = 20f;
    
    public float airMoveSpeed = 8f;
    public float airAcceleration = 12f;
    public float airDeceleration = 15f;
    public float airJumpForceMultiplier;
    
    private float currentSpeed = 0f;
    private float moveDirection;
    public int jumpsRemaining = 2; // Track available jumps
    private float currentJumpForce;

    public Rigidbody2D rb;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool isFacingRight = true;
    public bool isControllingSpeed = true;
    private Player player;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        moveDirection = player.moveAction.ReadValue<Vector2>().x;
        
        Jumping();
        FlipCharacter();
    }

    private void FixedUpdate()
    {
        float currentSpeed = IsGrounded() ? groundMoveSpeed : airMoveSpeed;
        float currentAccel = IsGrounded() ? groundAcceleration : airAcceleration;
        float currentDecel = IsGrounded() ? groundDeceleration : airDeceleration;

        // Рассчитываем целевую скорость
        float targetSpeed = moveDirection * currentSpeed;
        float speedDifference = targetSpeed - rb.linearVelocity.x;

        // Определяем силу для ускорения/торможения
        float accelerationRate = (Mathf.Abs(moveDirection) > 0.1f) 
            ? currentAccel 
            : currentDecel;

        // Применяем силу
        float movementForce = speedDifference * accelerationRate;
        rb.AddForce(new Vector2(movementForce, 0), ForceMode2D.Force);

        // Ограничиваем максимальную скорость
        float maxSpeed = IsGrounded() ? groundMoveSpeed : airMoveSpeed;
        if (isControllingSpeed && Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
        }
    }

    private void Jumping()
    {
        if (IsGrounded())
        {
            jumpsRemaining = 1;
            currentJumpForce = jumpForce; // Reset jump force when grounded
        }

        if (player.jumpAction.triggered && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);
            jumpsRemaining--;
            currentJumpForce *= airJumpForceMultiplier; // Reduce force for next jump
        }
    }
    
    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.2f), 0, groundLayer);
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
