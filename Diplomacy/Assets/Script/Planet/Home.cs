using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Home : Planet {









    private int civil = 50;
    public int mood = 50;
    private int state = 1;

    public bool badMood = false;
    public bool goodMood = false;

    private float food = 0;
    private float foodNeeded;
    private float powr = 0;
    private float powrNeeded;
    private float iron = 0;
    private float ironNeeded;

    private float changeValueNormal = 3;

    [SerializeField]
    private SpriteRenderer haloOut;
    [SerializeField]
    private SpriteRenderer haloCamp;


    private HomeUI homeUI;

    public AudioClip spawnAmoi;
    public AudioClip unhappy;
    public AudioClip newCamp;
    public AudioClip killCivil;
    public AudioClip destroyHome;
    public AudioClip healHome;


    //temporary
    public bool peopleHaveEnoughToLive = true;
    public int shipCreationRange = 3;

    public List<Ship> wholeBadArmada = new List<Ship>();

    [SerializeField]
    private SpriteRenderer groundImage;
    [SerializeField]
    private SpriteRenderer cloudImage;

    public void Start()
    {
        this.name = "PlanetHome " + this.nameInGame;
        this.inCamp = false;

        Vector3 rotation = Vector3.forward * UnityEngine.Random.Range(0, 360);
        groundImage.sprite = GameMaster.Instance.GetRandomSpriteGround();
        groundImage.transform.Rotate(rotation);
        cloudImage.sprite = GameMaster.Instance.GetRandomSpriteCloud();
        cloudImage.transform.Rotate(rotation);

        homeUI = Instantiate(GameMaster.Instance.homeUI).GetComponent<HomeUI>();
        homeUI.transform.SetParent(GameMaster.Instance.canvasWorld.transform);
        homeUI.transform.position = this.transform.position;
        homeUI.transform.localScale = Vector3.one*0.8f;
        homeUI.name = "HomeUI of " + nameInGame;

        SetCivil( (int)UnityEngine.Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
        InvokeRepeating( "CreateShip", UnityEngine.Random.Range(0, StaticValue.tempo * shipCreationRange), StaticValue.tempo * shipCreationRange);

        CalculNeededValue();

        food = (int)UnityEngine.Random.Range(100, 280);
        iron = (int)UnityEngine.Random.Range(100, 280);
        powr = (int)UnityEngine.Random.Range(100, 280);

        if(goodMood)
            SetMood((int)UnityEngine.Random.Range(65, 100));
        else if (badMood)
            SetMood((int)UnityEngine.Random.Range(0, 35));
        else
            SetMood((int)UnityEngine.Random.Range(20, 90));
        //print("Depart mood = " + mood + " for " + this.name);
        if (mood > 60)
        {
            joinCamp();
        } else
        {
            quitOGU();
        }

        UpdateValueAndVisual();
    }
    private void CalculNeededValue()
    {
        switch (GameMaster.Instance.difficulty)
        {
            case UtilType.Difficulty.Easy:
                foodNeeded = 200;
                powrNeeded = 200;
                ironNeeded = 200;
                break;
            case UtilType.Difficulty.Normal:
                foodNeeded = civil + changeValueNormal;
                powrNeeded = civil + changeValueNormal;
                ironNeeded = civil + changeValueNormal;
                changeValueNormal = 0;
                break;
            case UtilType.Difficulty.Hard:
                foodNeeded = civil + 1;
                powrNeeded = civil + 1;
                ironNeeded = civil + 1;
                break;
            case UtilType.Difficulty.Hell:
                foodNeeded = civil + 1;
                powrNeeded = civil + 1;
                ironNeeded = civil + 1;
                break;
        }
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
        CalculNeededValue();
        UpdateValueAndVisual();
    }

    public void SetMood(int value)
    {
        //print("SetMood " + this.name);
        mood = Mathf.Min(value,100);
        mood = Mathf.Max(value, 0);
        if (mood < 40 && inCamp)
        {
//            print("SetMood and quit " + this.name);
            quitOGU();
            _audioSource.PlayOneShot(unhappy, 0.1f);
        }
        if(mood > 60 && !inCamp)
        {
            joinCamp();
            _audioSource.PlayOneShot(newCamp, 0.1f);
        } 

        UpdateValueAndVisual();
    }

    private void quitOGU()
    {
        inCamp = false;
        haloCamp.gameObject.SetActive(false);
        haloOut.gameObject.SetActive(true);

        //TO DO : Kill all ship near him or convert their 
        
        foreach(Ship ship in _shipAnchorToThisPlanet)
        {
            //convert :
            ship.GetOutCamp();
            //killjoinCamp
            //ship.DestroyShip();
        }

//        print("quit Camp : " + this.name);
        AttackAroundHim();

        GameMaster.Instance.RemovePlanetInPeace();
    }
    private void joinCamp()
    {
        inCamp = true;
        haloCamp.gameObject.SetActive(true);
        haloOut.gameObject.SetActive(false);
        foreach(Ship ship in wholeBadArmada)
        {
            ship.GoToCamp();
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
            _audioSource.PlayOneShot(killCivil, 0.1f);
            destroyCivil(1);
        } else
        {
            print("What ?");
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
        {
            AddCivilNow();

            if(GameMaster.Instance.difficulty == UtilType.Difficulty.Normal)
            {
                if (powr < powrNeeded)
                    changeValueNormal -= 3;
                if (iron < ironNeeded)
                    changeValueNormal -= 3;
                if (food < foodNeeded)
                    changeValueNormal -= 3;

                if (changeValueNormal == 0)
                    changeValueNormal = 6;
            }
        }
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
            CivilStarvation();
        }
        if (GetCivil() == 0 && state == 1)
        {
            _audioSource.PlayOneShot(destroyHome, 0.1f);
            state = 0;
        }
        if (GetCivil() > 0 && state == 0)
            _audioSource.PlayOneShot(healHome, 0.1f);
    }
    public void CivilStarvation()
    {
        int deathValue = 1;
        switch (GameMaster.Instance.difficulty)
        {
            case UtilType.Difficulty.Easy:
                deathValue = 0;
                break;
            case UtilType.Difficulty.Normal:

                break;
            case UtilType.Difficulty.Hard:
                deathValue = 2;
                break;
            case UtilType.Difficulty.Hell:
                deathValue = 2;
                break;
        }
        if (iron < ironNeeded && food < foodNeeded && powr < powrNeeded)
        {
            SetCivil(GetCivil() - deathValue );
            if (GetCivil() != 0)
                GameMaster.Instance.AddCasualties(deathValue);
        }
    }
    public void CreateShip()
    {
        if (Time.deltaTime != 0 && getNumberOfShipOnIt() <= 5)
        {
            Ship shipCreate = Instantiate(GameMaster.Instance.ship).GetComponent<Ship>();
            shipCreate.transform.position = this.transform.position + ((Vector3)Vector2.right);
            shipCreate.transform.SetParent(orbit.transform);
            this.addShipAnchor(shipCreate);
            //define shipCamp
            shipCreate.origin = this;
            shipCreate.onOrbitOn = this;
            shipCreate.SetLocation(this);
            if (inCamp)
            {
                shipCreate.GoToCamp();
                _audioSource.PlayOneShot(spawnAmoi, 0.1f);
            }
            else
            {
                shipCreate.GetOutCamp();
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

    public override Vector3 getSupplyValue()
    {
        Vector3 res = Vector3.zero;
        if (food < 0)
        {
            res += Vector3.right;
        }
        if (iron < 0)
        {
            res += Vector3.up;
        }
        if (powr < 0)
        {
            res += Vector3.forward;
        }
        return new Vector3();
    }
    #endregion




    #region IA

    Vector3 supplyWanted = Vector3.zero;

    Radar radar;

    private void AttackAroundHim()
    {
     //   print("Attack around him " + this.name);
        //First IA Method
        UpdateSupplyWanted();
        if (getNumberOfShipOnIt() == 0)
        {
            LaterSend();
        }
        else if(!inCamp && supplyWanted.magnitude > 0)
        {
            StartCoroutine(seekForPlanetToInvade());
        } else
        {
            LaterSend();
        }
    }
    private IEnumerator seekForPlanetToInvade()
    {
        if(radar == null)
            radar = Instantiate(GameMaster.Instance.radar).GetComponent<Radar>();
        radar.transform.position = this.transform.position;
        radar.origin = this;
        radar.keepSeeking = true;
       // print("Before waitUntil in seekForPlanet for " + this.name);
        yield return new WaitUntil(() => (radar.planetTouched.Count >= 5));
      //  print("After waitUntil in seekForPlanet for for " + this.name);
        radar.keepSeeking = false;

        TreatmentRadarData(radar.planetTouched);

    }

    private void LaterSend()
    {
        Invoke("AttackAroundHim", StaticValue.tempo * 10);
        Invoke("MessageForSend" , StaticValue.tempo * 10);
    }
    private void MessageForSend()
    {
        //print("recalcul !");
    }

    private IEnumerator seekForMorePlanetToInvade()
    {
        //print("Re looking for");
        radar.origin = this;
        radar.keepSeeking = true;
        radar.planetTouched.Clear();
        yield return new WaitUntil(() => (radar.planetTouched.Count >= 5 || radar.tooLarge));
        radar.keepSeeking = false;
        if (radar.tooLarge)
        {
            radar.transform.localScale = Vector3.one;
            LaterSend();
            radar.tooLarge = false;
        }
        else
        {
            TreatmentRadarData(radar.planetTouched);
        }

    }

    private void TreatmentRadarData(List<Planet> planetData)
    {
   //     print("TreatmentRadarData for "+this.name);
        List<Planet> noRisk = new List<Planet>();
        Dictionary<Planet, float> risqueLvl = new Dictionary<Planet, float>();
        foreach(Planet planet in planetData)
        {
            if(planet.getNumberOfShipOnIt() == 0)
            {

            }
            else if(!planet.getFatherOfShipOnIt().gameObject == this.gameObject)
            {
                float riskValue = (planet.transform.position - transform.position).magnitude;
                riskValue *= (planet.getNumberOfShipOnIt() + 1);
                float supplyValue = planet.getSupplyValue().x * supplyWanted.x + planet.getSupplyValue().y * supplyWanted.y + planet.getSupplyValue().z * supplyWanted.z;
                if(supplyValue != 0)
                {
                    Resources planetResources = planet.GetComponent<Resources>();
                    if (planetResources != null)
                    {
                        if (!planetResources.FluxThisPlanet(this))
                        {
                            riskValue /= supplyValue;
                            print("add in resources");
                            risqueLvl.Add(planet, riskValue);
                        }
                    } else
                    {
                        riskValue /= supplyValue;
                        print("add in home ");
                        risqueLvl.Add(planet, riskValue);
                    }
                    
                }

                
            }
            if (!risqueLvl.ContainsKey(planet) && planet.getNumberOfShipOnIt() == 0)
            {
                noRisk.Add(planet);
            }
        } // fin du foreach
        string risqueList = "";
        string noRiskList = "";
        foreach (Planet planet in risqueLvl.Keys)
        {
            risqueList += ", " + planet.name + " (" + risqueLvl[planet] + ")";
        }
        foreach (Planet planet in noRisk)
        {
            noRiskList += ", " + planet.name;
        }
//        print("RisqueLvl (" + risqueLvl.Count +") : "+ risqueList);
//        print("noRisk (" + noRisk.Count + ") = "+noRiskList);
        if(risqueLvl.Count == 0 && noRisk.Count == 0)
        {
            StartCoroutine(seekForMorePlanetToInvade());
        } else
        {
            Destroy(radar.gameObject);
            ChooseBestPlanet(noRisk, risqueLvl);
        }
    }

    private void ChooseBestPlanet(List<Planet> noRisk, Dictionary<Planet, float> risqueLvl )
    {
        string printLog = "value = ";
        Planet planetMax = null;
        foreach(Planet planet in risqueLvl.Keys)
        {
            if (planetMax == null)
                planetMax = planet;
            if(risqueLvl[planet] < risqueLvl[planetMax])
            {
                planetMax = planet;
                printLog += " new max = ";
            }
            printLog = printLog + risqueLvl[planet] + " , ";
        }
        //print(printLog);
        List<Planet> planetToSendShip = new List<Planet>();
        planetToSendShip.Add(planetMax);
        planetToSendShip.AddRange(noRisk);
        LaunchShipTo(noRisk);
    }

    private void LaunchShipTo(List<Planet> planetToInvade)
    {
        for (int i = 0; i < planetToInvade.Count; i++)
        {
            if (this.getNumberOfShipOnIt() > 0)
            {
//                print("Foreach planet : " + this.name);
                for(int j = 0; j < 1; j++) // i < planetToInvade[i]
                    LaunchIndividualShipTo(planetToInvade[i]);
            }
            else
            {
                i = planetToInvade.Count;
                //print("Remis a plus tard : " + this.name);
                LaterSend();
            }
        }

    }

    private void LaunchIndividualShipTo(Planet planet)
    {
        _shipAnchorToThisPlanet[0].GoToPlanetByIA(planet);
    }


    private void UpdateSupplyWanted()
    {
        supplyWanted = Vector3.zero;
        if (food < foodNeeded)
        {
            supplyWanted += Vector3.right;
        } 
        if(iron < ironNeeded)
        {
            supplyWanted += Vector3.up;
        }
        if (powr < powrNeeded)
        {
            supplyWanted += Vector3.forward;
        }
    } 


    #endregion

}
