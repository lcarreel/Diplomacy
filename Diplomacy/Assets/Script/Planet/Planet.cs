using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Planet : MonoBehaviour {

    public UtilType.PlanetID nameInGame;

    [SerializeField]
    public bool inOGU = false;/*{ get; private set; }*/

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
    
    public void destroyAShipAnchor()
    {

        if (_shipAnchorToThisPlanet.Count != 0)
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
        return _shipAnchorToThisPlanet[0].origin;
    }

    public abstract Vector3 getSupplyValue();

    public void GoOrbit(Ship ship)
    {
        if(_shipAnchorToThisPlanet.Count >=6)
        {
            ship.DestroyShip();
        } else
        {
            addShipAnchor(ship);
            ship.transform.SetParent(orbit.transform);
            Quaternion q = ship.transform.localRotation;
            q.eulerAngles += eulerAnglesRotationOfShipInOrbit;
            ship.transform.localRotation = q;
        }
    }


    private void OnMouseDown()
    {
        if (inOGU)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.SetBool("HighLight", true);
            GameMaster.Instance.cursorCreator.Create(this.gameObject);
        }
        
    }

    private void OnMouseDrag()
    {
        if(inOGU)
            GameMaster.Instance.cursorCreator.UpdatePosition();
    }

    private void OnMouseUp()
    {
        if (inOGU)
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
                //print("target ok");
            }
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.SetBool("HighLight", false);
        }
    }
}
