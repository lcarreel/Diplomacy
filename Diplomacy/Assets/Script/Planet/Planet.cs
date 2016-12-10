using UnityEngine;
using System.Collections;

public abstract class Planet : MonoBehaviour {

	public bool inOGU { get; private set; }

    private int _shipAnchorToThisPlanet = 0;

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


}
