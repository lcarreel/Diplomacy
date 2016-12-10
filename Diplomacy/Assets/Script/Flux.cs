using UnityEngine;
using System.Collections;

public class Flux : MonoBehaviour {

    private Home home_planet;
    private Vector3 resources;

    public void SetResourses(int food, int iron, int powr)
    {
        resources.Set(food, iron, powr);
    }

    public Vector3 GetResources()
    {
        return resources;
    }

    public void SetHomePlanet(Home planet)
    {
        home_planet = planet;
    }

    public Home GetHomePlanet()
    {
        return home_planet;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
