using UnityEngine;

public class TalkingNPC : NPC, ITalkable
{

    [SerializeField] private DialogueText _dialogueText;
    [SerializeField] private DialogueController _dialogueController;

    public override void Start()
    {
        base.Start();
        _dialogueController = FindFirstObjectByType<DialogueController>().GetComponent<DialogueController>();
        _dialogueController.gameObject.SetActive(false);
    }
    
    public override void Interact()
    {
        Talk(_dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        _dialogueController.DisplayNextParagraph(dialogueText);
    }
}
