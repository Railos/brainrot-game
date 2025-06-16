using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    
    [field: SerializeField] public float maxSpeed { get; set; }
    
    public Rigidbody2D rb { get; set; }
    public bool isFacingRight { get; set; } = true;
    
    #region State Machine Variables
    
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    
    #endregion
    
    public bool isAggroed { get; set; }
    public bool isWithingAttackDistance { get; set; }

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }
    
    void Start()
    {
        CurrentHealth = MaxHealth;

        rb = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health/Die functions
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        
        if (CurrentHealth <= 0f) Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    #endregion
    
    #region Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        rb.AddForce(velocity, ForceMode2D.Impulse);
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
        }
        CheckForLeftOrRightFacing(velocity);
    }
    

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (velocity.x > 0 && !isFacingRight || velocity.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    #endregion
    
    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationEventTrigger(triggerType);
    }
    
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
    
    #endregion

    #region Triggers

    public void SetAggroStatus(bool setIsAggroed)
    {
        isAggroed = setIsAggroed;
    }

    public void SetAttackStatus(bool isWithinAttackRange)
    {
        isWithingAttackDistance = isWithinAttackRange;
    }
    
    #endregion
}
