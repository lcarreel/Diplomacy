using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public float speed = 1;

    private GameObject target;

    [SerializeField]
    private GameObject cursorTargetPrefab;
    private cursorTarget cursorTarget;

    private void OnMouseDown()
    {
        print("Down");
        if(cursorTarget == null)
            cursorTarget = Instantiate(cursorTargetPrefab).GetComponent<cursorTarget>();
    }
    private void OnMouseDrag()
    {
        cursorTarget.transform.position = (Vector2) GameMaster.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        print("Up");
        target = cursorTarget.calculateTarget();
        //TO DO : detect if planet under mouse
        if (target != null)
        {
            StartCoroutine(GoToTargetPoint(speed));
        }
        else
        {
            cursorTarget.disappear();
            cursorTarget = null;
        }
    }

    IEnumerator GoToTargetPoint(float speed)
    {
        print("Go To !");
        float distanceDone = 0;
        while(distanceDone < 1)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, distanceDone);
            distanceDone += speed * Time.deltaTime;
            print("speed * Time.deltaTime = "+ speed * Time.deltaTime +"   with : "+ distanceDone);
            yield return new WaitForSeconds(0.1f);
        }
        target = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == target)
        {
            //reaction
            Planet planetAttacked = target.GetComponent<Planet>();
            if (planetAttacked != null)
            {
                AttackPlanet(planetAttacked);
            }
            print("Target = null by contact with planet");
            target = null;
        }
    }



    private void AttackPlanet(Planet planetAttacked)
    {
        if (planetAttacked != null) {
            if (planetAttacked.getNumberOfShipOnIt() != 0)
            {
                AttackDefendedPlanet(planetAttacked);
            } else
            {
                AttackUndefendedPlanet(planetAttacked);
            }
        }
    }

    private void AttackDefendedPlanet(Planet planetAttacked)
    {
        planetAttacked.destroyShipAnchor(1);
        Destroy(this.gameObject);
    }

    private void AttackUndefendedPlanet(Planet planetAttacked)
    {
        Home homeAttacked = planetAttacked.GetComponent<Home>();
        if (homeAttacked != null)
        {
            AttackHome(homeAttacked);
        }
        else
        {
            //TO DO : reaction on Resources planet : planet are now free AND if AI did it : a flux is born !
        }
    }


    private void AttackHome(Home homeAttacked)
    {
        homeAttacked.destroyCivil(1);
        Destroy(this.gameObject);
    }
    private void AttackResources(Resources resourcesAttacked)
    {
        
    }

}
