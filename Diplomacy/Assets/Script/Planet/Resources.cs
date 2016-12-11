using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources : Planet {

    private List<Flux> _flux = new List<Flux>();
    public List<UtilType.Supply> slot = new List<UtilType.Supply>();

    #region visual
    [SerializeField]
    private List<SpriteRenderer> slotVisual = new List<SpriteRenderer>();


    private ResourcesUI resourcesUI;

    private void Start()
    {
        resourcesUI = Instantiate(GameMaster.Instance.resourcesUI).GetComponent<ResourcesUI>();
        resourcesUI.transform.SetParent(GameMaster.Instance.canvasWorld.transform);
        resourcesUI.transform.position = this.transform.position;
        resourcesUI.transform.localScale = Vector3.one;


        slot.Clear();
        UtilType.Supply randomValue = UtilType.Supply.Food;
        for (int i = 0; i < 3; i++)
        {
            int randomInt = Random.Range(0, 3);
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

    }
    #endregion

    private void UpdateFluxValues()
    {
        int food = 0;
        int iron = 0;
        int powr = 0;
        int flux_nbr = _flux.Count;

        foreach(UtilType.Supply resource in slot)
        {
            if (resource == UtilType.Supply.Food)
                food += 12;
            else if (resource == UtilType.Supply.Iron)
                iron += 12;
            else if (resource == UtilType.Supply.Powr)
                powr += 12;
        }
        foreach(Flux flux in _flux)
        {
            flux.SetResourses(food / flux_nbr , iron / flux_nbr , powr / flux_nbr);
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

            print(flux.GetResources());
            print(flux.GetHomePlanet().ToString());
        }
    }

    public void removeFlux(Flux flux)
    {
        _flux.Remove(flux);
        UpdateFluxValues();
    } 
}
