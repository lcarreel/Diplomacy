using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Home : Planet {

    private int civil = 50;
    public int mood = 50;
    private int state = 1;

    private float food = 0;
    private float foodNeeded;
    private float powr = 0;
    private float powrNeeded;
    private float iron = 0;
    private float ironNeeded;

    [SerializeField]
    private SpriteRenderer haloOut;
    [SerializeField]
    private SpriteRenderer haloOGU;


    private HomeUI homeUI;

    public AudioClip spawnAmoi;
    public AudioClip unhappy;
    public AudioClip newOGU;
    public AudioClip killCivil;
    public AudioClip destroyHome;
    public AudioClip healHome;


    //temporary
    public bool peopleHaveEnoughToLive = true;
    public int shipCreationRange = 3;

    public List<Ship> wholeBadArmada = new List<Ship>();

    public void Start()
    {
        this.name = "PlanetHome " + this.nameInGame;

        homeUI = Instantiate(GameMaster.Instance.homeUI).GetComponent<HomeUI>();
        homeUI.transform.SetParent(GameMaster.Instance.canvasWorld.transform);
        homeUI.transform.position = this.transform.position;
        homeUI.transform.localScale = Vector3.one;
        homeUI.name = "HomeUI of " + nameInGame;

        SetCivil( (int)Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
        InvokeRepeating( "CreateShip", Random.Range(0, StaticValue.tempo * shipCreationRange), StaticValue.tempo * shipCreationRange);

        foodNeeded = civil+1;
        powrNeeded = civil+1;
        ironNeeded = civil+1;

        food = (int)Random.Range(100, 280);
        iron = (int)Random.Range(100, 280);
        powr = (int)Random.Range(100, 280);

        SetMood((int)Random.Range(25, 100));
        if(mood > 60)
        {
            joinOGU();
        }

        UpdateValueAndVisual();
    }

    private void UpdateValueAndVisual()
    {
        peopleHaveEnoughToLive = !(food < foodNeeded || powr < powrNeeded || iron < ironNeeded);

        UpdateUI();
    }

    public int GetCivil()
    {
        return civil;
    }
    public void SetCivil(int value)
    {
        civil = value;
        if (civil < 0)
            civil = 0;
        UpdateCivilText();
        foodNeeded = civil+1;
        powrNeeded = civil+1;
        ironNeeded = civil+1;
        UpdateValueAndVisual();
    }

    public void SetMood(int value)
    {
        mood = Mathf.Min(value,100);
        mood = Mathf.Max(value, 0);
        if (mood < 40 && inOGU)
        {
            quitOGU();
            _audioSource.PlayOneShot(unhappy);
        }
        if(mood > 60 && !inOGU)
        {
            joinOGU();
            _audioSource.PlayOneShot(newOGU);
        }

        UpdateValueAndVisual();
    }

    private void quitOGU()
    {
        inOGU = false;
        haloOGU.enabled = false;
        haloOut.enabled = true;

        //TO DO : Kill all ship near him or convert their ?


        GameMaster.Instance.RemovePlanetInPeace();
    }
    private void joinOGU()
    {
        inOGU = true;
        haloOGU.enabled = true;
        haloOut.enabled = false;
        foreach(Ship ship in wholeBadArmada)
        {
            ship.GoToOGU();
        }
        GameMaster.Instance.AddPlanetInPeace();
    }


    public bool Attacked(Ship ship)
    {
        bool destroyInAttack = true;
        if(this.getNumberOfShipOnIt() != 0)
        {
            destroyAShipAnchor();
        } else if(GetCivil() > 0)
        {
            _audioSource.PlayOneShot(killCivil);
            destroyCivil(1);
        } else
        {
            destroyInAttack = false;
        }
        return destroyInAttack;
    }
    public void destroyCivil(int numberOfShipAttacked)
    {
        int deadNumber = GetCivil() - numberOfShipAttacked * StaticValue.numberOfCivilDeadByShip;
        SetCivil(deadNumber);
        if(GetCivil()!=0)
            GameMaster.Instance.AddCasualties(deadNumber);
    }

    private void UpdateCivilText()
    {
        homeUI.civilNumber.text = civil.ToString();
    }

    public void AddCivilPeriodically()
    {
        if (Time.timeScale != 0)
            AddCivilNow();
    }
    private void AddCivilNow()
    {

        iron -= civil / StaticValue.consomation;
        if (iron < 0)
            iron = 0;
        food -= civil / StaticValue.consomation;
        if (food < 0)
            food = 0;
        powr -= civil / StaticValue.consomation;
        if (powr < 0)
            powr = 0;
        if (peopleHaveEnoughToLive)
        {
            SetCivil(GetCivil() + 1);
            SetMood(mood + 1);
            if (iron >= ironNeeded * 2 || food >= foodNeeded * 2 || powr >= powrNeeded * 2)
                SetMood(mood + 2);
        }
        else
        {
            SetMood(mood - 1);
            if (iron < ironNeeded && food < foodNeeded && powr < powrNeeded)
            {
                SetCivil(GetCivil() - 1);
                if (GetCivil() != 0)
                    GameMaster.Instance.AddCasualties(1);
            }
        }
        if (GetCivil() == 0 && state == 1)
        {
            _audioSource.PlayOneShot(destroyHome);
            state = 0;
        }
        if (GetCivil() > 0 && state == 0)
            _audioSource.PlayOneShot(healHome);
    }
    public void CreateShip()
    {
        if (Time.deltaTime != 0)
        {
            Ship shipCreate = Instantiate(GameMaster.Instance.ship).GetComponent<Ship>();
            shipCreate.transform.position = this.transform.position + ((Vector3)Vector2.right);
            shipCreate.transform.SetParent(orbit.transform);
            this.addShipAnchor(shipCreate);
            //define shipCamp
            shipCreate.origin = this;
            shipCreate.SetLocation(this);
            if (inOGU)
            {
                shipCreate.GoToOGU();
                _audioSource.PlayOneShot(spawnAmoi);
            }
            else
            {
                wholeBadArmada.Add(shipCreate);
            }
        }
        
    }


    #region GAUGE

    public void UpdateUI()
    {
        homeUI.ChangeValue(new Vector3(food,iron,powr),mood,new Vector3(foodNeeded,ironNeeded,powrNeeded));
    }

    public void AddRessources(Vector3 supply)
    {
       // print("Add supply : " + supply);
        food += supply.x;
        iron += supply.y;
        powr += supply.z;

        UpdateValueAndVisual();
    }


    #endregion




    #region IA






    #endregion

}
