using UnityEngine;
using System.Collections;

public class Flux : MonoBehaviour {

    private Home home_planet;
    private Vector3 resources;

    private GameObject fluxParticle;
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

        x = dest.transform.position.x - origin.transform.position.x;
        y = dest.transform.position.y - origin.transform.position.y;
        fluxParticle = Instantiate(GameMaster.Instance.fluxParticles);
        fluxParticle.transform.SetParent(origin.transform);
        fluxParticle.transform.localPosition = Vector3.zero;
        angle = System.Math.Atan(x / y);
//        angle = (dest.transform.position.y > origin.transform.position.y ? -angle : angle);
        angle = UnityEngine.Mathf.Rad2Deg * -angle;
        angle = (dest.transform.position.y >= origin.transform.position.y ? angle : angle + 180);

        fluxParticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (float)angle);
        print(fluxParticle);
        distance = System.Math.Sqrt(x * x + y * y);
        fluxParticle.GetComponent<ParticleSystem>().startLifetime = (float)distance / 2.5f;


        InvokeRepeating("GiveResourcesToHomePlanet", 0.1f, StaticValue.tempo * Time.deltaTime* fluxSpeed);
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
        home_planet.AddRessources(resources);
    }

}