using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    private Controls controls;
    private InputAction interactAction;
    [SerializeField] private SpriteRenderer interactSprite;
    private Transform playerTransform;

    private const float INTERACT_DISTANCE = 5f;
    
    private void Awake()
    {
        controls = new Controls();
        interactAction = controls.Player.Interact;
        interactAction.Enable();
    }

    public virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame() && IsWithingInteractDistance())
        {
            Interact();
        }

        if (!IsWithingInteractDistance())
        {
            interactSprite.DOFade(0, 0.2f);
        }
        else
        {
            interactSprite.DOFade(0.5f, 0.25f);
        }
    }

    public abstract void Interact();

    private bool IsWithingInteractDistance()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < INTERACT_DISTANCE) return true;
        else return false;
    }
}
