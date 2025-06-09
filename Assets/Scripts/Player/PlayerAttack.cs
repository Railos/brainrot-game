using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 0.5f);
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackCooldown;
    [SerializeField] private LayerMask attackableLayers;

    [SerializeField] private Camera cam;
    private Controls controls;
    private InputAction attackAction;
    private bool canAttack = true;

    private void Awake()
    {
        controls = new Controls();
        attackAction = controls.Player.Attack;
        attackAction.Enable();
    }
    
    private void Update()
    {
        if (attackAction.triggered && canAttack)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        canAttack = false;
        
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;
        float attackAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            transform.position,
            boxSize,
            attackAngle,
            attackDirection,
            attackRange,
            attackableLayers
        );

        HashSet<Health> damagedTargets = new HashSet<Health>();

        foreach (RaycastHit2D hit in hits)
        {
            Health health = hit.collider.GetComponent<Health>();
            if (health != null && !damagedTargets.Contains(health))
            {
                health.Damage(damageAmount);
                damagedTargets.Add(health);
            }
        }
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}