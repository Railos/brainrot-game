using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerTailAttackState : PlayerState
{
    public PlayerTailAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Tail Attack State");
        player.animator.CrossFade("PlayerTailAttack", 0, 0, 0);

        player.playerMovement.enabled = true;
        player.playerTailAttack.enabled = true;
        player.playerBite.enabled = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!player.playerTailAttack.resettedAttack || !player.playerTailAttack.canAttack) return;

        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) > 0.1f)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }

        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) < 0.1f)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }

        if (Mathf.Abs(player.playerMovement.rb.linearVelocityY) > 0.1f)
        {
            player.StateMachine.ChangeState(player.JumpState);
        }
        
        player.attackAction.performed += ctx => {
            if (ctx.interaction is PressInteraction && player.playerBite.resettedAttack && player.playerBite.canAttack)
            {
                player.StateMachine.ChangeState(player.BiteState);
            }
        };
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