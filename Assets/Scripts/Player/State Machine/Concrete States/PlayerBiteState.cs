using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBiteState : PlayerState
{

    public PlayerBiteState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        player.animator.CrossFade("PlayerBite", 0, 0, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!player.playerBite.canAttack || !player.playerBite.resettedAttack) return;
        
        if (player.playerMovement.rb.linearVelocityX == 0f)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
        
        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) > 0f)
        {
            player.StateMachine.ChangeState(player.WalkState);
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