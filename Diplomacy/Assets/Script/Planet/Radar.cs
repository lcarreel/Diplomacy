using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Script for the gameObject who detect the nearest planet (ONLY CALL BY HOME when control by AI). 
/// </summary>
public class Radar : MonoBehaviour {


    public float growingSpeed = 0.5f;

    public List<Planet> planetTouched = new List<Planet>();
    public Planet origin;

    public bool keepSeeking = true;
    public bool tooLarge = false;
    private CircleCollider2D _circleCollider2D;

    // Update is called once per frame
    void Update () {
        if (keepSeeking)
        {
            //Security
            if (_circleCollider2D == null)
                _circleCollider2D = GetComponent<CircleCollider2D>();
            //Security end
            _circleCollider2D.enabled = true;
            float value = 1 + (growingSpeed * Time.deltaTime);
            this.transform.localScale *= value;
           // print(value + " grown up");
           if(transform.localScale.x > 500)
            {
                tooLarge = true;
                //this is bad news, never happened since the debug (5 hours dev, after LudumDare) 
            }
        } else
        {
            //Security
            if (_circleCollider2D == null)
                _circleCollider2D = GetComponent<CircleCollider2D>();
            //Security end
            _circleCollider2D.enabled = false;
        }
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision + "+ collision.gameObject.name);
        Planet planetOver = collision.gameObject.GetComponent<Planet>();
        if (planetOver != null)
        {
            if(planetOver.gameObject != origin.gameObject && !planetTouched.Contains(planetOver))
            {
                planetTouched.Add(planetOver);
            }
        }

    }
}
