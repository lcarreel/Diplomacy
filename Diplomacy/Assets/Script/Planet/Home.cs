using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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

        SetCivil( (int)UnityEngine.Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
        InvokeRepeating( "CreateShip", UnityEngine.Random.Range(0, StaticValue.tempo * shipCreationRange), StaticValue.tempo * shipCreationRange);

        foodNeeded = civil+1;
        powrNeeded = civil+1;
        ironNeeded = civil+1;

        food = (int)UnityEngine.Random.Range(100, 280);
        iron = (int)UnityEngine.Random.Range(100, 280);
        powr = (int)UnityEngine.Random.Range(100, 280);

        SetMood((int)UnityEngine.Random.Range(25, 100));
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
            _audioSource.PlayOneShot(unhappy, 0.1f);
        }
        if(mood > 60 && !inOGU)
        {
            joinOGU();
            _audioSource.PlayOneShot(newOGU, 0.1f);
        }

        UpdateValueAndVisual();
    }

    private void quitOGU()
    {
        inOGU = false;
        haloOGU.gameObject.SetActive(false);
        haloOut.gameObject.SetActive(true);

        //TO DO : Kill all ship near him or convert their 
        
        foreach(Ship ship in _shipAnchorToThisPlanet)
        {
            //convert :
            ship.GetOutOGU();
            //kill
            //ship.DestroyShip();
        }

        AttackAroundHim();

        GameMaster.Instance.RemovePlanetInPeace();
    }
    private void joinOGU()
    {
        inOGU = true;
        haloOGU.gameObject.SetActive(true);
        haloOut.gameObject.SetActive(false);
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
            _audioSource.PlayOneShot(killCivil, 0.1f);
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
            _audioSource.PlayOneShot(destroyHome, 0.1f);
            state = 0;
        }
        if (GetCivil() > 0 && state == 0)
            _audioSource.PlayOneShot(healHome, 0.1f);
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
                _audioSource.PlayOneShot(spawnAmoi, 0.1f);
            }
            else
            {
                shipCreate.GetOutOGU();
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
        //First IA Method
        UpdateSupplyWanted();
        if(!inOGU && supplyWanted.magnitude > 0)
        {
            StartCoroutine(seekForPlanetToInvade());
        }
    }
    private IEnumerator seekForPlanetToInvade()
    {
        if(radar == null)
            radar = Instantiate(GameMaster.Instance.radar).GetComponent<Radar>();
        radar.transform.position = this.transform.position;
        radar.origin = this;
        radar.keepSeeking = true;
        yield return new WaitUntil(() => (radar.planetTouched.Count >= 5));
        radar.keepSeeking = false;

        TreatmentRadarDate(radar.planetTouched);

    }

    private IEnumerator seekForMorePlanetToInvade()
    {
        radar.origin = this;
        radar.keepSeeking = true;
        radar.planetTouched.Clear();
        yield return new WaitUntil(() => (radar.planetTouched.Count >= 5));
        radar.keepSeeking = false;

        TreatmentRadarDate(radar.planetTouched);
    }

    private void TreatmentRadarDate(List<Planet> planetData)
    {
        List<Planet> noRisk = new List<Planet>();
        List<float> risqueLvl = new List<float>();
        int planetUseless = planetData.Count;
        foreach(Planet planet in planetData)
        {
            if (planet.getNumberOfShipOnIt() == 0)
            {
                noRisk.Add(planet);
                risqueLvl.Add(2047);
            }
            else if(planet.getFatherOfShipOnIt().gameObject == this.gameObject)
                {
                    planetUseless++;
                    risqueLvl.Add(2047);
            }
            else
            {
                float riskValue = (planet.transform.position - transform.position).magnitude;
                riskValue *= (planet.getNumberOfShipOnIt() + 1);
                float supplyValue = planet.getSupplyValue().x * supplyWanted.x + planet.getSupplyValue().y * supplyWanted.y + planet.getSupplyValue().z * supplyWanted.z;
                if(supplyValue == 0)
                {
                    planetUseless++;
                    risqueLvl.Add(2047);
                } else
                {
                    riskValue /= supplyValue;
                    risqueLvl.Add(riskValue);
                }
            }

        } // fin du foreach

        if(planetUseless == 0)
        {
            StartCoroutine(seekForMorePlanetToInvade());
        } else
        {
            Destroy(radar.gameObject);
            ChooseBestPlanet(planetData, noRisk, risqueLvl);
        }
        //if no risk ==> verifier si resources ou home
    }

    private void ChooseBestPlanet(List<Planet> planetData, List<Planet> noRisk, List<float> risqueLvl )
    {
        int maxIndex = 0;
        for ( int i = 0; i < risqueLvl.Count; i++ )
        {
            if(risqueLvl[i] > risqueLvl[maxIndex])
            {
                maxIndex = i;
            }
        }
        noRisk.Add(planetData[maxIndex]);
        LaunchShipTo(noRisk);
    }

    private void LaunchShipTo(List<Planet> planetToInvade)
    {
        foreach (Planet planet in planetToInvade)
        {
            LaunchIndividualShipTo(planet);
            //envoie getNumberOfShipOnIt + 1
        }
    }

    private void LaunchIndividualShipTo(Planet planet)
    {
        if (this.getNumberOfShipOnIt() > 0)
        {
            _shipAnchorToThisPlanet[0].GoToPlanetByIA(planet);
            _shipAnchorToThisPlanet.Remove(_shipAnchorToThisPlanet[0]);
        } else
        {
            Invoke("AttackAroundHim",5*StaticValue.tempo);
        }
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
