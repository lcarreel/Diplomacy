using UnityEngine;
using System.Collections;

public abstract class Planet : MonoBehaviour {

    [SerializeField]
    public bool inOGU;/*{ get; private set; }*/

    private int _shipAnchorToThisPlanet = 0;

    private GameObject target;

    //TO DO : Add flux list : to where the flux go. Limit = 4. 

    //Method part

    public void addShipAnchor()
    {
        addShipAnchor(1);
    }
    public void addShipAnchor(int numberOfShipToAdd)
    {
        _shipAnchorToThisPlanet += numberOfShipToAdd;
    }
    
    public void destroyShipAnchor()
    {
        destroyShipAnchor(1);
    }
    public void destroyShipAnchor(int numberOfShipToDestroy)
    {
        _shipAnchorToThisPlanet += numberOfShipToDestroy;
    }

    public int getNumberOfShipOnIt()
    {
        return _shipAnchorToThisPlanet;
    }

    private void OnMouseDown()
    {
        print("Down");
        GameMaster.Instance.cursorCreator.Create();
    }

    private void OnMouseDrag()
    {
        GameMaster.Instance.cursorCreator.UpdatePosition();
    }

    private void OnMouseUp()
    {
        Home home;

        print("Up");
        target = GameMaster.Instance.cursorCreator.ReturnTargetAndDisappear();
        if (target != null)
        {
            if (target.GetComponent<Home>())
            {
                home = target.GetComponent<Home>();
                //TODO VERIFICATION FLUX EXISTE OU NON
                if (GetComponent<Resources>())
                    GetComponent<Resources>().SetFlux(home, gameObject.AddComponent<Flux>());
            }
            else if (target.GetComponent<Resources>())
            {
                                
            }
            print("target ok");
        }
    }
}
