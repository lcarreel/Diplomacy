using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script was made by Lise Careel 
/// </summary>
public class Flux : MonoBehaviour {

    private Home home_planet;
    private Vector3 resources;

	private GameObject fluxParticleFood, fluxParticleIron, fluxParticlePowr;
	private List<GameObject> fluxParticles = new List<GameObject>();
    [SerializeField]
    private float fluxSpeed = 48f;

    public void SetResourses(int food, int iron, int powr)
    {
        resources.Set(food, iron, powr);
    }

    public void CreateFlux(Resources origin, Home dest)
    {
        double angle;
        double x, y;
        double distance;
		ParticleSystem.EmissionModule food, iron, powr;

        x = dest.transform.position.x - origin.transform.position.x;
        y = dest.transform.position.y - origin.transform.position.y;
        fluxParticleFood = Instantiate(GameMaster.Instance.fluxParticleFood);
		fluxParticleIron = Instantiate(GameMaster.Instance.fluxParticleIron);
		fluxParticlePowr = Instantiate(GameMaster.Instance.fluxParticlePowr);
		food = fluxParticleFood.GetComponent<ParticleSystem> ().emission;
		iron = fluxParticleIron.GetComponent<ParticleSystem> ().emission;
		powr = fluxParticlePowr.GetComponent<ParticleSystem> ().emission;
		food.rate = resources.x / 5;
		iron.rate = resources.y / 5;
		powr.rate = resources.z / 5;
		fluxParticles.Add (fluxParticleFood);
		fluxParticles.Add (fluxParticleIron);
		fluxParticles.Add (fluxParticlePowr);
		foreach (GameObject fluxParticle in fluxParticles) 
		{
			fluxParticle.transform.SetParent(origin.transform);
			fluxParticle.transform.localPosition = Vector3.zero;
			angle = System.Math.Atan(x / y);
			//        angle = (dest.transform.position.y > origin.transform.position.y ? -angle : angle);
			angle = UnityEngine.Mathf.Rad2Deg * -angle;
			angle = (dest.transform.position.y >= origin.transform.position.y ? angle : angle + 180);

			fluxParticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (float)angle);
			//        print(fluxParticle);
			distance = System.Math.Sqrt(x * x + y * y);
			fluxParticle.GetComponent<ParticleSystem>().startLifetime = (float)distance / 2.5f;
		}


        InvokeRepeating("GiveResourcesToHomePlanet", 0.1f, StaticValue.tempo * Time.deltaTime* fluxSpeed);
    }

	public void removeFlux()
	{
		foreach(GameObject fluxParticle in fluxParticles)
			Destroy(fluxParticle);
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

    //Give ressources
    public void GiveResourcesToHomePlanet()
    {
        if(Time.deltaTime != 0)
            home_planet.AddRessources(resources);
    }

}