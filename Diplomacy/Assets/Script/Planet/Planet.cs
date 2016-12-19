using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Planet : MonoBehaviour {

    public UtilType.PlanetID nameInGame;

    [SerializeField]
    public bool inCamp = false;/*{ get; private set; }*/

    public List<Ship> _shipAnchorToThisPlanet = new List<Ship>();

    [SerializeField]
    protected GameObject orbit;
    public Vector3 eulerAnglesRotationOfShipInOrbit = new Vector3(0,0,90);

    private GameObject target;

    private Animator _animator;

    protected AudioSource _audioSource;
    public AudioClip destroyShipSound;

    //Method part

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void addShipAnchor(Ship ship)
    {
        ship.SetLocation(GetComponent<Planet>());
        _shipAnchorToThisPlanet.Add(ship);
    }
    public void addShipAnchor(List<Ship> ShipsToAdd)
    {
        foreach (Ship ship in ShipsToAdd)
            addShipAnchor(ship);
    }
    
    public bool removeShipAnchor(Ship ship)
    {
        bool res = _shipAnchorToThisPlanet.Contains(ship);
        if (res)
        {
            _shipAnchorToThisPlanet.Remove(ship);
        } 
        return res;
    }

    public void destroyAShipAnchor()
    {

        if (getNumberOfShipOnIt() != 0)
        {

            if(_audioSource != null)
                _audioSource.PlayOneShot(destroyShipSound, 0.1f);
            Ship shipWhoSacrificeFOrOther = _shipAnchorToThisPlanet[0];
            _shipAnchorToThisPlanet.Remove(shipWhoSacrificeFOrOther);
            shipWhoSacrificeFOrOther.DestroyShip();
            GameMaster.Instance.AddCasualties(1);
        }
        
    }
    public void destroyShipAnchor(List<Ship> ShipsToAdd)
    {
        foreach (Ship ship in ShipsToAdd)
        {
            destroyAShipAnchor();
            ship.name += " abandonned";
        }
    }

    public int getNumberOfShipOnIt()
    {
        return _shipAnchorToThisPlanet.Count;
    }

    public Planet getFatherOfShipOnIt()
    {
        Planet res = null;
        if (getNumberOfShipOnIt() != 0)
            res = _shipAnchorToThisPlanet[0].origin;
        return res;
    }

    public abstract Vector3 getSupplyValue();

    public void GoOrbit(Ship ship)
    {
        if(getNumberOfShipOnIt() >= 5)
        {
            ship.DestroyShip();
        } else
        {
            if(ship.onOrbitOn != null)
                ship.onOrbitOn.removeShipAnchor(ship);
            addShipAnchor(ship);
            ship.transform.SetParent(orbit.transform);
            Quaternion q = ship.transform.localRotation;
            q.eulerAngles += eulerAnglesRotationOfShipInOrbit;
            ship.transform.localRotation = q;
            ship.onOrbitOn = this;
        }
    }


    private void OnMouseDown()
    {
        if (inCamp)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.SetBool("HighLight", true);
            GameMaster.Instance.cursorCreator.Create(this.gameObject);
        }
        
    }

    private void OnMouseDrag()
    {
        if(inCamp)
            GameMaster.Instance.cursorCreator.UpdatePosition();
    }

    private void OnMouseUp()
    {
        if (inCamp)
        {
            Home targetHome;
            Resources targetResources;
            target = GameMaster.Instance.cursorCreator.ReturnTargetAndDisappear();
            if (target != null)
            {
                targetResources = target.GetComponent<Resources>();
                targetHome = target.GetComponent<Home>();
                if (targetHome!=null)
                {
                    //TODO VERIFICATION FLUX EXISTE OU NON
                    if (GetComponent<Resources>())
                        GetComponent<Resources>().SetFlux(targetHome, gameObject.AddComponent<Flux>());
                    else if (!targetHome.inCamp)
                    {
                        if(getNumberOfShipOnIt() != 0)
                        _shipAnchorToThisPlanet[0].ReactionWithTarget(target);
                    }
                }
                else if (targetResources!=null)
                {
                    if (getNumberOfShipOnIt() != 0)
                        _shipAnchorToThisPlanet[0].ReactionWithTarget(target);
                }
                //print("target ok");
            }
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.SetBool("HighLight", false);
        }
    }
}
