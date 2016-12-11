using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance;

    [SerializeField]
    private List<Sprite> resourcesVisualPowr = new List<Sprite>();
    [SerializeField]
    private List<Sprite> resourcesVisualFood = new List<Sprite>();
    [SerializeField]
    private List<Sprite> resourcesVisualIron = new List<Sprite>();

    public List<Sprite> supplyIcon = new List<Sprite>();

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

        List<UtilType.PlanetID> nameUsed = new List<UtilType.PlanetID>();
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            TryRandomTillSuccess(planet, nameUsed);
        }

    }
    private void TryRandomTillSuccess(Planet planet, List<UtilType.PlanetID> nameUsed)
    {
        planet.nameInGame = (UtilType.PlanetID)Random.Range(0, StaticValue.numberOfPlanetName);
        if (nameUsed.Contains(planet.nameInGame))
        {
            TryRandomTillSuccess(planet, nameUsed);
        } else
        {
            nameUsed.Add(planet.nameInGame);
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
    public GameObject homeUI;
    public GameObject resourcesUI;
    
    public GameObject canvasWorld;
    public GameObject canvasCamera;

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

    #region GetVisual
    public Sprite getResourcesVisual(int numberOnCircle, UtilType.Supply type)
    {
        Sprite res = null;
        switch (type)
        {
            case UtilType.Supply.Powr:
                res = resourcesVisualPowr[numberOnCircle];
                break;
            case UtilType.Supply.Food:
                res = resourcesVisualFood[numberOnCircle];
                break;
            case UtilType.Supply.Iron:
                res = resourcesVisualIron[numberOnCircle];
                break;
        }
        return res;

    }

    public Sprite getResourcesIcon(UtilType.Supply type)
    {
        Sprite spriteRes = supplyIcon[0];
        switch (type)
        {
            case UtilType.Supply.Food:
                spriteRes = supplyIcon[0];
                break;
            case UtilType.Supply.Iron:
                spriteRes = supplyIcon[1];
                break;
            case UtilType.Supply.Powr:
                spriteRes = supplyIcon[2];
                break;
        }

        return spriteRes;
    }

    #endregion 
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
