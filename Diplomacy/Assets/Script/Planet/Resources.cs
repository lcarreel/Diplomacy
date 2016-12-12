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
            if (randomInt < 1)
                randomValue = UtilType.Supply.Powr;
            else if (randomInt < 2)
                randomValue = UtilType.Supply.Food;
            else if (randomInt < 3)
                randomValue = UtilType.Supply.Iron;
            slot.Add(randomValue);
            slotVisual[i].sprite = GameMaster.Instance.getResourcesVisual(i, randomValue);

        }

        resourcesUI.UpdateIcon(slot);

        joinOGU();

    }
    #endregion

    public void joinOGU()
    {
        inOGU = true;
        haloOGU.gameObject.SetActive(true);
        haloOut.gameObject.SetActive(false);
    }
    public void quitOGU()
    {
        inOGU = false;
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
        switch (GameMaster.Instance.difficulty)
        {
            case UtilType.Difficulty.Easy:
                foodResources = (int) (food - 1.5f * flux_nbr);
                ironResources = (int) (iron - 1.5f * flux_nbr);
                powrResources = (int) (powr - 1.5f * flux_nbr);
                break;
            case UtilType.Difficulty.Normal:
                foodResources = food - 2 * flux_nbr;
                ironResources = iron - 2 * flux_nbr;
                powrResources = powr - 2 * flux_nbr;
                break;
            case UtilType.Difficulty.Hard:
                foodResources = food / flux_nbr;
                ironResources = iron / flux_nbr;
                powrResources = powr / flux_nbr;
                break;
            case UtilType.Difficulty.Hell:
                foodResources = food / flux_nbr;
                ironResources = iron / flux_nbr;
                powrResources = powr / flux_nbr;
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
        print("Emptied + "+this.name);
        List<Flux> _tmp = new List<Flux>();
        foreach (Flux flux in _flux)
        {
            _tmp.Add(flux);
        }
        _flux.Clear();
        foreach (Flux flux in _tmp)
        {
            Destroy(flux);
        }
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
