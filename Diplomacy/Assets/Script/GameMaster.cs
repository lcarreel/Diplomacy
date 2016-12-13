using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance;

    public UtilType.Difficulty difficulty = UtilType.Difficulty.Normal;
    public UtilType.Speed speed = UtilType.Speed.CruisingSpeed;

    [SerializeField]
    private List<Sprite> resourcesVisualPowr = new List<Sprite>();
    [SerializeField]
    private List<Sprite> resourcesVisualFood = new List<Sprite>();
    [SerializeField]
    private List<Sprite> resourcesVisualIron = new List<Sprite>();

    public List<Sprite> supplyIcon = new List<Sprite>();
    
    public List<Home> allHomePlanets = new List<Home>();
    public List<Resources> allResourcesPlanets = new List<Resources>();

    private AudioSource _audioSource;

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

        //set start value
        int diffNumber = PlayerPrefs.GetInt("DIFFICULTY");
        int spdNumber = PlayerPrefs.GetInt("SPEED");

        switch (diffNumber)
        {
            case 0:
                difficulty = UtilType.Difficulty.Easy;
                break;
            case 1:
                difficulty = UtilType.Difficulty.Normal;
                break;
            case 2:
                difficulty = UtilType.Difficulty.Hard;
                break;
            case 3:
                difficulty = UtilType.Difficulty.Hell;
                break;
        }

        switch (spdNumber)
        {
            case 0:
                speed = UtilType.Speed.FirstStep;
                break;
            case 1:
                speed = UtilType.Speed.CruisingSpeed;
                break;
            case 2:
                speed = UtilType.Speed.High;
                break;
            case 3:
                speed = UtilType.Speed.LightSpeed;
                break;
        }

        //end set start value

        allHomePlanets.Clear();
        allResourcesPlanets.Clear();
        List<UtilType.PlanetID> nameUsed = new List<UtilType.PlanetID>();
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            TryRandomTillSuccess(planet, nameUsed);
            if (planet.GetComponent<Home>())
                allHomePlanets.Add(planet.GetComponent<Home>());
            else
                allResourcesPlanets.Add(planet.GetComponent<Resources>());
        }

        int badMoodInt = Random.Range(0, allHomePlanets.Count);
        allHomePlanets[(int)badMoodInt].badMood = true;
        int invers = Mathf.Abs(allHomePlanets.Count - (int)badMoodInt);
        if (invers < allHomePlanets.Count) 
            allHomePlanets[invers].goodMood = true;

        badMoodInt = Random.Range(0, allHomePlanets.Count);
        if(!allHomePlanets[(int)badMoodInt].goodMood)
            allHomePlanets[(int)badMoodInt].badMood = true;
        invers = Mathf.Abs(allHomePlanets.Count - (int)badMoodInt);
        if (invers < allHomePlanets.Count)
            if (!allHomePlanets[Mathf.Abs(allHomePlanets.Count - (int)badMoodInt)].badMood)
            allHomePlanets[Mathf.Abs(allHomePlanets.Count - (int)badMoodInt)].goodMood = true;


        switch (difficulty)
        {
            case UtilType.Difficulty.Easy:
                StaticValue.production = 24;
                StaticValue.consomation = 6;
                StaticValue.numberOfCivilDeadByShip = 6;
                break;
            case UtilType.Difficulty.Normal:
                break;
            case UtilType.Difficulty.Hard:
                StaticValue.consomation = 12;
                break;
            case UtilType.Difficulty.Hell:
                StaticValue.consomation = 12;
                StaticValue.numberOfCivilDeadByShip = 24;
                break;
        }
        switch (speed)
        {
            case UtilType.Speed.FirstStep:
                StaticValue.tempo = 2f;
                break;
            case UtilType.Speed.CruisingSpeed:
                StaticValue.tempo = 1f;
                break;
            case UtilType.Speed.High:
                StaticValue.tempo = 0.5f;
                break;
            case UtilType.Speed.LightSpeed:
                StaticValue.tempo = 0.1f;
                break;
        }
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
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
    private int planetInPeace = 0;


    public Camera currentCamera;

    public PauseMenu pauseMenu;
    private bool onPause = false;

    public cursorCreator cursorCreator;

    public GameObject ship;
    public GameObject fluxParticleFood;
	public GameObject fluxParticleIron;
	public GameObject fluxParticlePowr;

    public GameObject homeUI;
    public GameObject resourcesUI;
    public GameObject radar;
    
    public GameObject canvasWorld;
    public GameObject canvasCamera;

    [SerializeField]
    private Text scoreUI;
    [SerializeField]
    private Text progressUI;
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

    public void AddPlanetInPeace()
    {
        planetInPeace++;
        UpdateProgressWindow(planetInPeace);
        VerificationOfGameProgress();
    }
    public void RemovePlanetInPeace()
    {
        planetInPeace--;
        UpdateProgressWindow(planetInPeace);
        VerificationOfGameProgress();
    }

    public void AddCasualties(int dead)
    {
        score += dead;
        UpdateScoreWindow();

        //TO DO : idea quick to develop : civil/military and famine / war death
    }

    #region UI-ScoreAndProgress
    private void UpdateScoreWindow()
    {
        scoreUI.text = "Casualties = "+score;
    }

    private void UpdateProgressWindow(int progress)
    {
        progressUI.text = progress + "/" + allHomePlanets.Count + " planet in peace";
    }

    #endregion

    public bool VerificationOfGameProgress()
    {
        bool gameFinish = true;
        int planetInPeace = 0;
        foreach(Home homePlanet in FindObjectsOfType<Home>())
        {
            if (!homePlanet.inOGU)
            {
                gameFinish = false;
            } else
            {
                planetInPeace++;
            }
        }
        UpdateProgressWindow(planetInPeace);
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
