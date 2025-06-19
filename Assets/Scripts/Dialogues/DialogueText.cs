using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NewDialogueContainer")]
public class DialogueText : ScriptableObject
{
    public string speakerName;

    [TextArea(5, 10)]
    public string[] paragraphs;
}
