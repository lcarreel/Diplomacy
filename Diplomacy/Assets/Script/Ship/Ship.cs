using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public float speed = 1;

    private GameObject target;
    
    //Component
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    #region mouseDrag&DropHandler
    private void OnMouseDown()
    {
        print("Down");
        _rigidbody2D.isKinematic = true;
        GameMaster.Instance.cursorCreator.Create();
    }
    private void OnMouseDrag()
    {
        GameMaster.Instance.cursorCreator.UpdatePosition();
    }
    private void OnMouseUp()
    {
        print("Up");
        target = GameMaster.Instance.cursorCreator.ReturnTargetAndDisappear();
        if (target != null)
        {
            StartCoroutine(GoToTargetPoint(speed));
        }
        _rigidbody2D.isKinematic = false;
    }
    #endregion

    IEnumerator GoToTargetPoint(float speed)
    {
        transform.SetParent(null);
        
        //look at target
        Quaternion rotation = Quaternion.LookRotation
             (target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        //

        float distanceDone = 0;
        while(distanceDone < 1 && target!=null)
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


    public void ChangeKinematicState(bool value)
    {
        _rigidbody2D.isKinematic = value;
    }

}
