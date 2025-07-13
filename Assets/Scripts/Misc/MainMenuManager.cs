using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private TMP_Text volumeValue;
    public string sceneToLoad;
    
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SwitchToSettings()
    {
        mainCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        mainCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void OnVolumeChange(float value)
    {
        value *= 100;
        volumeValue.SetText(value.ToString("0.0"));
    }
}
