using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text NPCNameText;
    [SerializeField] private TMP_Text NPCDialogueText;
    [SerializeField] private float typeSpeed = 10f;

    private Queue<string> paragraphs = new Queue<string>();
    private bool convoEnded;

    private string p;
    private Coroutine typeDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";
    private bool isTyping;
    
    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if (paragraphs.Count == 0)
        {
            if (!convoEnded)
            {
                StartConversation(dialogueText);
            }
            else if (convoEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }

        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueCoroutine(p));
        }
        else
        {
            FinishParagraphEarly();
        }
        

        if (paragraphs.Count == 0)
        {
            convoEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            GetComponent<Image>().DOFade(0.5f, 0.3f);
        }

        NPCNameText.text = dialogueText.speakerName;

        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    private void EndConversation()
    {
        paragraphs.Clear();
        convoEnded = false;
        if (gameObject.activeSelf)
        {
            GetComponent<Image>().DOFade(0f, 0.2f);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogueCoroutine(string p)
    {
        yield return new WaitForSeconds(0.2f);
        isTyping = true;

        NPCDialogueText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogueText.text = displayedText;

            yield return new WaitForSeconds(0.1f / typeSpeed);
        }
        
        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueCoroutine);
        NPCDialogueText.text = p;
        isTyping = false;
    }
}
