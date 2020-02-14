using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private RawImage barImg;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private SceneController sceneController;

    public void SetScore(int score)
    {
        scoreLabel.SetText(score.ToString());
    }

    public void OnOpenSettings()
    {
        sceneController.paused = true;
    }

    public void ShowPauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public void SetTimeOutValue(float timeout)
    {
        barImg.transform.localScale = new Vector3(timeout, 1.0f);
    }

    public void EnableTimeOutBar(bool enable)
    {
        barImg.gameObject.SetActive(enable);
    }
}
