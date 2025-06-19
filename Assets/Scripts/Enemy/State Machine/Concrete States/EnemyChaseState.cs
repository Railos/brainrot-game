using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform _player;
    private float direction;
    private float _movementSpeed = 5f;
    
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void EnterState()
    {
        base.EnterState();
        
        Debug.Log("I SEE U");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        direction = Mathf.Sign(_player.position.x - enemy.transform.position.x);

        if (enemy.isWithingAttackDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }

        if (!enemy.isAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        enemy.MoveEnemy(new Vector2(direction * _movementSpeed, enemy.rb.linearVelocityY));
    }

    public override void AnimationEventTrigger(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationEventTrigger(triggerType);
    }
}
