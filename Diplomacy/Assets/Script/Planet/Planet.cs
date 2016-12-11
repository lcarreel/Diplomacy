using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Planet : MonoBehaviour {

    public UtilType.PlanetID nameInGame;

    [SerializeField]
    public bool inOGU;/*{ get; private set; }*/

    public List<Ship> _shipAnchorToThisPlanet = new List<Ship>();

    [SerializeField]
    protected GameObject orbit;
    public Vector3 eulerAnglesRotationOfShipInOrbit = new Vector3(0,0,90);

    private GameObject target;

    private Animator _animator;
    
    //Method part

    public void addShipAnchor(Ship ship)
    {
        _shipAnchorToThisPlanet.Add(ship);
    }
    public void addShipAnchor(List<Ship> ShipsToAdd)
    {
        foreach (Ship ship in ShipsToAdd)
            addShipAnchor(ship);
    }
    
    public void destroyAShipAnchor()
    {
        if (_shipAnchorToThisPlanet.Count != 0)
        {
            Ship shipWhoSacrificeFOrOther = _shipAnchorToThisPlanet[0];
            _shipAnchorToThisPlanet.Remove(shipWhoSacrificeFOrOther);
            Destroy(shipWhoSacrificeFOrOther.gameObject);
            GameMaster.Instance.AddCasualties(1);
        }
        
    }
    public void destroyShipAnchor(List<Ship> ShipsToAdd)
    {
        foreach (Ship ship in ShipsToAdd)
            destroyAShipAnchor();
    }

    public int getNumberOfShipOnIt()
    {
        return _shipAnchorToThisPlanet.Count;
    }

    public void GoOrbit(Ship ship)
    {
        addShipAnchor(ship);
        ship.transform.SetParent(orbit.transform);
        Quaternion q = ship.transform.localRotation;
        q.eulerAngles += eulerAnglesRotationOfShipInOrbit;
        ship.transform.localRotation = q;
    }


    private void OnMouseDown()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool("HighLight", true);
        GameMaster.Instance.cursorCreator.Create();
    }

    private void OnMouseDrag()
    {
        GameMaster.Instance.cursorCreator.UpdatePosition();
    }

    private void OnMouseUp()
    {
        Home home;

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
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool("HighLight", false);
    }
}
