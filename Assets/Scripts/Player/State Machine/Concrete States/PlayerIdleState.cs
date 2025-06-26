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