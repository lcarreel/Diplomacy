using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Resources : Planet {

    private List<Flux> _flux = new List<Flux>();
    public List<UtilType.Supply> slot = new List<UtilType.Supply>();

    #region visual
    [SerializeField]
    private List<SpriteRenderer> slotVisual = new List<SpriteRenderer>();

    [SerializeField]
    private SpriteRenderer haloOut;
    [SerializeField]
    private SpriteRenderer haloOGU;

    private ResourcesUI resourcesUI;

    private void Start()
    {        
        this.name = "PlanetResources " + this.nameInGame;

        resourcesUI = Instantiate(GameMaster.Instance.resourcesUI).GetComponent<ResourcesUI>();
        resourcesUI.transform.SetParent(GameMaster.Instance.canvasWorld.transform);
        resourcesUI.transform.position = this.transform.position;
        resourcesUI.transform.localScale = Vector3.one;
        resourcesUI.name = "ResourcesUI of " + nameInGame;

        slot.Clear();
        UtilType.Supply randomValue = UtilType.Supply.Food;
        for (int i = 0; i < 3; i++)
        {
            int randomInt = UnityEngine.Random.Range(0, 3);
            //print("Value : " + randomInt + " in " + GameMaster.Instance.numberOfPowrMax + ", "+ GameMaster.Instance.numberOfFoodMax+", "+ GameMaster.Instance.numberOfIronMax + " for " + this.name);
            int lowerForLast = 2;
            if (randomInt < 1 && GameMaster.Instance.numberOfPowrMax<=0)
            {
                randomInt++;
                lowerForLast = 1; // so if both powr and iron is <= 0, it will go to food
            }
            if (randomInt >= 1 && randomInt < 2 && GameMaster.Instance.numberOfFoodMax <= 0)
            {
                randomInt++;
            }
            if (randomInt >= 2 && randomInt < 3 && GameMaster.Instance.numberOfIronMax <= 0)
            {
                randomInt -= lowerForLast;
            }


            if (randomInt < 1)
            {
                randomValue = UtilType.Supply.Powr;
                GameMaster.Instance.numberOfPowrMax--;
            }
            else if (randomInt < 2)
            {
                randomValue = UtilType.Supply.Food;
                GameMaster.Instance.numberOfFoodMax--;
            }
            else if (randomInt < 3)
            {
                randomValue = UtilType.Supply.Iron;
                GameMaster.Instance.numberOfIronMax--;
            }
            slot.Add(randomValue);
            slotVisual[i].sprite = GameMaster.Instance.getResourcesVisual(i, randomValue);
        }

        resourcesUI.UpdateIcon(slot);

        joinCamp();

    }
    #endregion

    public void joinCamp()
    {
        inCamp = true;
        haloOGU.gameObject.SetActive(true);
        haloOut.gameObject.SetActive(false);
    }
    public void quitCamp()
    {
        inCamp = false;
        haloOGU.gameObject.SetActive(false);
        haloOut.gameObject.SetActive(true);
    }


    private void UpdateFluxValues()
    {
        int food = 0;
        int iron = 0;
        int powr = 0;
        int flux_nbr = _flux.Count;

        foreach(UtilType.Supply resource in slot)
        {
            if (resource == UtilType.Supply.Food)
                food += StaticValue.production;
            else if (resource == UtilType.Supply.Iron)
                iron += StaticValue.production;
            else if (resource == UtilType.Supply.Powr)
                powr += StaticValue.production;
        }
        int foodResources = 0;
        int ironResources = 0;
        int powrResources = 0;

        int nbr_Flux_corrected = flux_nbr;
        if (nbr_Flux_corrected == 0)
            nbr_Flux_corrected = 1;
        switch (GameMaster.Instance.difficulty)
        {
            case UtilType.Difficulty.Easy:
                foodResources = (int) (food - 1.5f * nbr_Flux_corrected);
                ironResources = (int) (iron - 1.5f * nbr_Flux_corrected);
                powrResources = (int) (powr - 1.5f * nbr_Flux_corrected);
                break;
            case UtilType.Difficulty.Normal:
                foodResources = food - 2 * nbr_Flux_corrected;
                ironResources = iron - 2 * nbr_Flux_corrected;
                powrResources = powr - 2 * nbr_Flux_corrected;
                break;
            case UtilType.Difficulty.Hard:
                foodResources = food / nbr_Flux_corrected;
                ironResources = iron / nbr_Flux_corrected;
                powrResources = powr / nbr_Flux_corrected;
                break;
            case UtilType.Difficulty.Hell:
                foodResources = food / nbr_Flux_corrected;
                ironResources = iron / nbr_Flux_corrected;
                powrResources = powr / nbr_Flux_corrected;
                break;
        }
        foreach(Flux flux in _flux)
        {
            flux.SetResourses(foodResources, ironResources, powrResources);
        }
    }

    public List<Flux> GetFlux()
    {
        return _flux;
    }
    public bool FluxThisPlanet(Planet planet)
    {
        foreach(Flux flux in _flux)
        {
            if (flux.GetHomePlanet().gameObject == planet.gameObject)
                return true;
        }
        return false;
    }

    public void SetFlux(Home planet, Flux flux)
    {
        int exists = 0;
        //ParticleSystem paticle_system;
        foreach (Flux flu in _flux)
        {
            if (flu.GetHomePlanet() == planet)
                exists = 1;
        }
        if (exists == 0 && _flux.Count < 4)
        {
            flux.SetHomePlanet(planet);
            _flux.Add(flux);
            UpdateFluxValues();
            flux.CreateFlux(GetComponent<Resources>(), planet);

            //print(flux.GetResources());
            //print(flux.GetHomePlanet().ToString());
        }
    }

    public void removeFlux(Flux flux)
    {
        _flux.Remove(flux);
        UpdateFluxValues();
    }

    public void emptyFlux()
    {
        //print("Emptied + "+this.name);
        foreach (Flux flux in _flux) 
		{
			flux.removeFlux();
		}
		_flux.Clear();
		UpdateFluxValues();
    }


    public override Vector3 getSupplyValue()
    {
        Vector3 res = Vector3.zero;
        foreach (UtilType.Supply supply in slot)
        {
            switch (supply)
            {
                case UtilType.Supply.Food:
                    res += Vector3.right;
                    break;
                case UtilType.Supply.Iron:
                    res += Vector3.up;
                    break;
                case UtilType.Supply.Powr:
                    res += Vector3.forward;
                    break;
            }
        }
        return res;
    }
}