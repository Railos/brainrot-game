using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerBite : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDash = 2f;
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 0.5f);
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackCooldown;
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

    private void Update()
    {
        if (player.biteAction.triggered && resettedAttack && canAttack)
        {
            PerformAttack();
        }
    }
    
    public void PerformAttack()
    {
        resettedAttack = false;
        canAttack = false;
        StartCoroutine(nameof(waitUntilOnTheGround));
        
        movement.enabled = false;
        Invoke(nameof(ResetSpeedControl), 0.1f);
        
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;
        float attackAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().linearVelocity = attackDirection * attackDash;
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            transform.position,
            boxSize,
            attackAngle,
            attackDirection,
            attackRange,
            attackableLayers
        );

        foreach (RaycastHit2D hit in hits)
        {
            IDamageable health = hit.collider.GetComponent<IDamageable>();
            if (health != null)
            {
                health.Damage(damageAmount);
            }
        }
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetSpeedControl()
    {
        movement.enabled = true;
    }

    IEnumerator waitUntilOnTheGround()
    {
        yield return new WaitUntil(movement.IsGrounded);
        canAttack = true;
    }

    private void ResetAttack()
    {
        resettedAttack = true;
    }
}