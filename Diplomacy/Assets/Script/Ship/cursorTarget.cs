using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cursorTarget : MonoBehaviour {

    [SerializeField]
    private List<Planet> planetUnderCursor = new List<Planet>();
    [SerializeField]
    private List<Flux> fluxUnderCursor = new List<Flux>();
    [SerializeField]
    private List<Ship> shipUnderCursor = new List<Ship>();


    public float fadeSpeed = 3;

    //Component :
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        //Add : tp to position at first
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Planet planetOver = collision.gameObject.GetComponent<Planet>();
        if (planetOver != null)
        {
            print("Planet Under");
            if(!planetUnderCursor.Contains(planetOver))
                planetUnderCursor.Add(planetOver);
        }

        Flux fluxOver = collision.gameObject.GetComponent<Flux>();
        if (fluxOver != null)
        {
            print("Flux Under");
            if (!fluxUnderCursor.Contains(fluxOver))
                 fluxUnderCursor.Add(fluxOver);
        }

        Ship shipOver = collision.gameObject.GetComponent<Ship>();
        if (shipOver != null)
        {
            print("Ship Under");
            if (!shipUnderCursor.Contains(shipOver))
                 shipUnderCursor.Add(shipOver);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Planet planetOver = collision.gameObject.GetComponent<Planet>();
        if (planetOver != null)
        {
            if (planetUnderCursor.Contains(planetOver))
                planetUnderCursor.Remove(planetOver);
        }

        Flux fluxOver = collision.gameObject.GetComponent<Flux>();
        if (fluxOver != null)
        {
            if (fluxUnderCursor.Contains(fluxOver))
                fluxUnderCursor.Remove(fluxOver);
        }

        Ship shipOver = collision.gameObject.GetComponent<Ship>();
        if (shipOver != null)
        {
            if (shipUnderCursor.Contains(shipOver))
                shipUnderCursor.Remove(shipOver);
        }

    }


    public GameObject calculateTarget()
    {
        GameObject res = null;
        //ship
        if (shipUnderCursor.Count != 0)
            res = shipUnderCursor[0].gameObject;
        //planet
         else if (planetUnderCursor.Count != 0)
            res = planetUnderCursor[0].gameObject;
        //flux
        else if (fluxUnderCursor.Count != 0)
            res = fluxUnderCursor[0].gameObject;
        return res;
    }

    public void disappear()
    {
        StartCoroutine(fadeAway());
    }

    IEnumerator fadeAway()
    {
        while(_spriteRenderer.color.a > 0)
        {
            _spriteRenderer.color -= Color.black * fadeSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        DestroyThis();
    }



    public void DestroyThis()
    {

        Destroy(this.gameObject);
    }
}
