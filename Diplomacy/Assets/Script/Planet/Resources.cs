using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources : MonoBehaviour {

    private List<Flux> _flux = new List<Flux>();
    public List<UtilType.Supply> slot = new List<UtilType.Supply>();

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

    public void SetFlux(Flux flux)
    {
        _flux.Add(flux);
        UpdateFluxValues();
    }

    public void removeFlux(Flux flux)
    {
        _flux.Remove(flux);
        UpdateFluxValues();
    } 
}
