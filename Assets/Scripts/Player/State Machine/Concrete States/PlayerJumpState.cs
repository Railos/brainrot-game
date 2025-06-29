using System;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        
        player.animator.CrossFade("PlayerJump", 0, 0, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.playerMovement.rb.linearVelocityY != 0f) return;
        
        if (player.playerMovement.rb.linearVelocityX == 0f)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }

        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) > 0f)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }
        
        if (player.biteAction.triggered && player.playerBite.resettedAttack && player.playerBite.canAttack)
        {
            player.StateMachine.ChangeState(player.BiteState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationEventTrigger(Player.AnimationTriggerType triggerType)
    {
        base.AnimationEventTrigger(triggerType);
    }
}