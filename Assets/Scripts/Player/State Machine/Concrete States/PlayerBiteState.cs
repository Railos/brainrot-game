using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerBiteState : PlayerState
{

    public PlayerBiteState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Bite State");
        player.animator.CrossFade("PlayerBite", 0, 0, 0);

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

        if (!player.playerBite.canAttack || !player.playerBite.resettedAttack) return;
        
        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) < 0.1f)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
        
        if (Mathf.Abs(player.playerMovement.rb.linearVelocityX) > 0.1f)
        {
            player.StateMachine.ChangeState(player.WalkState);
        }

        player.attackAction.performed += ctx =>
        {
            if (ctx.interaction is HoldInteraction && player.playerTailAttack.resettedAttack && player.playerTailAttack.canAttack)
            {
                player.StateMachine.ChangeState(player.TailAttackState);
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