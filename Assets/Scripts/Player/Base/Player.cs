using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }
    private bool canReceiveDamage = true;
    [SerializeField] private float invincibilityTimeAfterTakingDamage = 0.5f;
    public PlayerState curState;
    public ParticleSystem deathParticle;
    [SerializeField] private HealthBar healthBar;
    public PlayerMovement playerMovement;
    public PlayerBite playerBite;
    public PlayerTailAttack playerTailAttack;
    private Controls controls;
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction attackAction;

    public Animator animator;
    
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerJumpState JumpState { get; set; }
    public PlayerBiteState BiteState { get; set; }
    public PlayerTailAttackState TailAttackState { get; set; }
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerBite = GetComponent<PlayerBite>();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        BiteState = new PlayerBiteState(this, StateMachine);
        TailAttackState = new PlayerTailAttackState(this, StateMachine);

        controls = new Controls();
        moveAction = controls.Player.Move;
        moveAction.Enable();
        jumpAction = controls.Player.Jump;
        jumpAction.Enable();
        attackAction = controls.Player.Attack;
        attackAction.Enable();
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(CurrentHealth);
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();
    }
    
    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }
    
    public void Damage(float damageAmount)
    {
        if (!canReceiveDamage) return;
        CurrentHealth -= damageAmount;
        canReceiveDamage = false;
        if (CurrentHealth <= 0f)
        {
            Die();
        }
        healthBar.SetHealth(CurrentHealth);
        Invoke(nameof(ResetCanReceiveDamage),invincibilityTimeAfterTakingDamage);
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Invoke(nameof(GoToMenu), 1f);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetCanReceiveDamage()
    {
        canReceiveDamage = true;
    }
    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationEventTrigger(triggerType);
    }
    
    public enum AnimationTriggerType
    {
        PlayerDamaged,
        PlayFootstepSound
    }
    
}
