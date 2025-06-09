using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpTime;
    private float curJumpTime = 0f;
    private bool isJumping = false;
    private float moveDirection;
    
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
        rb.AddForceX(moveDirection * moveSpeed, ForceMode2D.Force);
        
        rb.linearVelocity = new Vector2(
            Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed),
            rb.linearVelocityY
        );
    }

    private void Jumping()
    {
        if (jumpAction.triggered && IsGrounded())
        {
            rb.AddForceY(jumpForce * Time.deltaTime, ForceMode2D.Impulse);
            isJumping = true;
            return;
        }

        if (jumpAction.inProgress && isJumping)
        {
            if (curJumpTime < maxJumpTime)
            {
                rb.AddForceY(jumpForce * Time.deltaTime, ForceMode2D.Impulse);
                curJumpTime += Time.deltaTime;
            }
        }

        if (jumpAction.WasReleasedThisDynamicUpdate())
        {
            isJumping = false;
            curJumpTime = 0f;
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
