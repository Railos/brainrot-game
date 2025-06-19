using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToOtherScene : NPC, IInteractable
{

    [SerializeField] private string sceneName;

    public override void Interact()
    {
        LevelLoader lv = FindFirstObjectByType<LevelLoader>();
        StartCoroutine(lv.LoadLevel(sceneName));
    }
}
