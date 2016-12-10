using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public bool gameEnd = false;
    public int score = 0;


    public Camera currentCamera;

    public PauseMenu pauseMenu;
    private bool onPause = false;

    public cursorCreator cursorCreator;

    public GameObject ship;
    public GameObject fluxParticles;

    //method 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InversePause();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }
    }

    public void InversePause()
    {
        if (onPause)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        onPause = !onPause;
    }


    public bool VerificationOfGameProgress()
    {
        bool gameFinish = true;
        foreach(Home homePlanet in FindObjectsOfType<Home>())
        {
            if (!homePlanet.inOGU)
            {
                gameFinish = false;
            }
        }
        gameEnd = gameFinish;
        if (gameEnd)
        {
            DeployEndGameScreen();
        }
        return gameEnd;
    }

    private void DeployEndGameScreen()
    {
        pauseMenu.EndGameDisplay(score);
        pauseMenu.gameObject.SetActive(true);
        if (score < PlayerPrefs.GetFloat(StaticValue.maxScore))
        {
            pauseMenu.bestScoreActive();
            PlayerPrefs.SetFloat(StaticValue.maxScore, score);
        }
    }

}
