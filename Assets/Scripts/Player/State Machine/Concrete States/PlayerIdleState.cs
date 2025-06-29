using System;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        
        player.animator.CrossFade("PlayerIdle", 0, 0, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) > 0f)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }
        
        if (player.playerMovement.rb.linearVelocityY != 0f)
        {
            player.StateMachine.ChangeState(player.JumpState);
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