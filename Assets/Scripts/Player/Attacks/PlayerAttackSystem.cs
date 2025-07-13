using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using System.Collections;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerBite playerBite;
    [SerializeField] private PlayerTailAttack playerTailAttack;



    private PlayerMovement movement;

    private void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<PlayerMovement>();
        playerBite = GetComponent<PlayerBite>();
        playerTailAttack = GetComponent<PlayerTailAttack>();
    }

    private void Update()
    {
        player.attackAction.performed += ctx =>
        {
            if (ctx.interaction is PressInteraction && playerBite.resettedAttack && playerBite.canAttack){
                playerBite.PerformAttack();
                Invoke(nameof(ResetAttack), playerBite.attackCooldown);
                StartCoroutine(nameof(waitUntilOnTheGround));
                Invoke(nameof(ResetSpeedControl), 0.1f);
            }

            if (ctx.interaction is HoldInteraction && playerTailAttack.resettedAttack && playerTailAttack.canAttack){
                playerTailAttack.PerformAttack();
                Invoke(nameof(ResetAttack), playerTailAttack.attackCooldown);
                StartCoroutine(nameof(waitUntilOnTheGround));
                Invoke(nameof(ResetSpeedControl), 0.1f);
            }
        };
    }

    private void ResetSpeedControl()
    {
        movement.enabled = true;
    }

    IEnumerator waitUntilOnTheGround()
    {
        yield return new WaitUntil(movement.IsGrounded);
        playerBite.canAttack = true;
        playerTailAttack.canAttack = true;
    }

    private void ResetAttack()
    {
        playerBite.resettedAttack = true;
        playerTailAttack.resettedAttack = true;
    }
}
