using UnityEngine;
    using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public Text pauseTitle;
    public Text endGameTitle;

    public Button resume;

    public Text scoreText;
    public Text bestScoreText;


    public void ResumeGame()
    {
        GameMaster.Instance.InversePause();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }

    public void EndGameDisplay(float score)
    {

        scoreText.text = score + " people get killed before it";
        pauseTitle.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);

        endGameTitle.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);

    }

    public void bestScoreActive()
    {
        bestScoreText.gameObject.SetActive(true);
    }


}
