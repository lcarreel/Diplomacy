using UnityEngine;
    using UnityEngine.UI;
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

    public void EndGameDisplay(float score)
    {
        pauseTitle.enabled = false;
        resume.enabled = false;

        endGameTitle.enabled = true;
        scoreText.enabled = true;

    }

    public void bestScoreActive()
    {
        bestScoreText.enabled = true;
    }


}
