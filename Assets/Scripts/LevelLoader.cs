using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    
    public IEnumerator LoadLevel(string sceneName)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(sceneName);
    }
}
