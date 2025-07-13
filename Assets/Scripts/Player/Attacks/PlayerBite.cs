using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerBite : MonoBehaviour, IPlayerAttack
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDash = 2f;
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 0.5f);
    [SerializeField] private int damageAmount = 10;
    public float attackCooldown;
    [SerializeField] private LayerMask attackableLayers;

    [SerializeField] private Camera cam;
    public bool resettedAttack = true;
    public bool canAttack = true;
    private PlayerMovement movement;
    private Player player;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }
    
    public void PerformAttack()
    {
        resettedAttack = false;
        canAttack = false;

        movement.enabled = false;

        // Determine direction based on player's facing
        Vector2 attackDirection = movement.isFacingRight ? Vector2.right : Vector2.left;

        // Set dash velocity in attack direction
        GetComponent<Rigidbody2D>().linearVelocity = attackDirection * attackDash;

        // Perform box cast
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            transform.position,
            boxSize,
            0,
            attackDirection,
            attackRange,
            attackableLayers
        );

        // Apply damage
        foreach (RaycastHit2D hit in hits)
        {
            IDamageable health = hit.collider.GetComponent<IDamageable>();
            if (health != null)
            {
                health.Damage(damageAmount);
            }
        }
    }
}