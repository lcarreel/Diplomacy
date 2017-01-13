using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public float speed = 1;

    [SerializeField]
    private GameObject target;
    public Home origin;
    public Planet onOrbitOn;
    public bool inCamp = false;
    //Component
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Planet location;

    //Part made by Lise Careel
    private AudioSource _audioSource;
    public AudioClip selectIdle;
    public AudioClip selectMining;
    public AudioClip sendToWar;
    public AudioClip sendToMine;
    public AudioClip destroyShip;
    //END Part made by Lise Careel

    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        //Part made by Lise Careel
        _audioSource = GetComponent<AudioSource>();
        //END Part made by Lise Careel
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetLocation(Planet planet)
    {
        location = planet;
    }


    public Planet getLocation()
    {
        return location;
    }


    //creation AND visual :
    public void GoToCamp()
    {
        inCamp = true;
        if (_spriteRenderer==null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = Color.blue+Color.green;
        _trailRenderer.material = GameMaster.Instance.trailGood;

        if (target != null)
            target = null;
        Resources planetResources = onOrbitOn.GetComponent<Resources>();
        if (planetResources != null)
        {
            if( !planetResources.inCamp)
            {
                planetResources.joinCamp();
            }
        }
    }
    public void GetOutCamp()
    {
        if (target == null)
        {
            inCamp = false;
            if (this != null)
            {
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = this.GetComponent<SpriteRenderer>();
                }
                _spriteRenderer.color = Color.red;
                _trailRenderer.material = GameMaster.Instance.trailBad;
            }
        } 
    }


    #region mouseDrag&DropHandler
    private void OnMouseDown()
    {
        if (inCamp)
        {
            _rigidbody2D.isKinematic = true;
            _animator.SetBool("HighLight", true);
            GameMaster.Instance.cursorCreator.Create(this.gameObject);

            //Part made by Lise Careel
            if (location.GetComponent<Home>())
                _audioSource.PlayOneShot(selectIdle, 0.1f);
            else if (location.GetComponent<Resources>())
                _audioSource.PlayOneShot(selectMining, 0.1f);
            //END Part made by Lise Careel
        }
    }
    private void OnMouseDrag()
    {
        if (inCamp)
        {
            GameMaster.Instance.cursorCreator.UpdatePosition();
        }
    }
    private void OnMouseUp()
    {
        if (inCamp)
        {
            target = GameMaster.Instance.cursorCreator.ReturnTargetAndDisappear();
            ReactionWithTarget(target);


            _rigidbody2D.isKinematic = false;
            _animator.SetBool("HighLight", false);
            
        }

    }
    public void ReactionWithTarget(GameObject newTarget)
    {
        this.target = newTarget;
        if (target != null)
        {
            Ship shipAttacked = target.GetComponent<Ship>();
            if (shipAttacked != null)
            {
                if (shipAttacked.GetComponentInParent<Planet>() != null && shipAttacked.GetComponentInParent<Planet>() != this.origin)
                {
                    target = shipAttacked.origin.gameObject;
                }
            }
            StartCoroutine(GoToTargetPoint(speed));
        }

        //Part made by Lise Careel
        if (target.GetComponent<Resources>())
            if (target.GetComponent<Resources>().GetFlux().Count == 0)
                _audioSource.PlayOneShot(sendToMine, 0.1f);
            else if (target.GetComponent<Planet>())
                if (!target.GetComponent<Planet>().inCamp)
                    _audioSource.PlayOneShot(sendToWar, 0.1f);
        //END Part made by Lise Careel

    }
    #endregion

    public void GoToPlanetByIA(Planet planet)
    {
        target = planet.gameObject;
        if (target != null)
        {
            StartCoroutine(GoToTargetPoint(speed));
        }
    }



    IEnumerator GoToTargetPoint(float speed)
    {
        transform.SetParent(null);
        onOrbitOn.removeShipAnchor(this);
        onOrbitOn = null;

        //look at target ( Code take on http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html , thanks to LEDWORKS for it )
        Quaternion rotation = Quaternion.LookRotation
             (target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        //end

        float distanceDone = 0;
        while(distanceDone < 1 && target!=null)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, distanceDone);
            distanceDone += speed * Time.deltaTime;
//            print("speed * Time.deltaTime = "+ speed * Time.deltaTime +"   with : "+ distanceDone);
            yield return new WaitForSeconds(0.02f);
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
            //print("Target = null by contact with planet");
            target = null;
        }
    }

    

    private void AttackPlanet(Planet planetAttacked)
    {
        if (planetAttacked.inCamp!=inCamp) {
            defineDefenseForThisAttack(planetAttacked);
        } else if (!inCamp)
        {
            if (planetAttacked.getNumberOfShipOnIt() == 0)
            {
                AttackUndefendedPlanet(planetAttacked);
            }
            else if(planetAttacked.getFatherOfShipOnIt().gameObject != origin.gameObject)
            {
                AttackDefendedPlanet(planetAttacked);
            } else
            {
                planetAttacked.GoOrbit(this);
            }
        }
        else
        {
            planetAttacked.GoOrbit(this);
        }
    }
    private void defineDefenseForThisAttack(Planet planetAttacked)
    {
        if (planetAttacked.getNumberOfShipOnIt() != 0)
        {
            AttackDefendedPlanet(planetAttacked);
        }
        else
        {
            AttackUndefendedPlanet(planetAttacked);
        }
    }

    private void AttackDefendedPlanet(Planet planetAttacked)
    {
        planetAttacked.destroyAShipAnchor();
        DestroyShip();
    }

    private void AttackUndefendedPlanet(Planet planetAttacked)
    {
        Home homeAttacked = planetAttacked.GetComponent<Home>();
        Resources resources = planetAttacked.GetComponent<Resources>();
        if (homeAttacked != null)
        {
            if (homeAttacked.inCamp == inCamp)
            {
                if(inCamp)
                    homeAttacked.GoOrbit(this);
                else
                    AttackHome(homeAttacked);
            }
            else
                AttackHome(homeAttacked);
        }
        else if(resources != null)
        {
            if (inCamp)
            {
                //IMPORTANT : planetAttacked != Home ! (because of the previous else)
                resources.joinCamp();
            } else
            {
                TakePlanetOver(resources);
            }
            planetAttacked.GoOrbit(this);
        }
    }
    private void TakePlanetOver(Resources resources)
    {
        //print("I'm here to take it");
        resources.quitCamp();
        resources.emptyFlux();
        resources.SetFlux(origin, gameObject.AddComponent<Flux>());
    }

    private void AttackHome(Home homeAttacked)
    {
        if (homeAttacked.Attacked(this))
        {
            origin.AddRessources(Vector3.one * 80);
            DestroyShip();
        } else
        {
            homeAttacked.GoOrbit(this);
        }
        
    }


    public void ChangeKinematicState(bool value)
    {
        //Security
        if (_rigidbody2D == null)
        {
            if(this != null)
                _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        //Security end
        _rigidbody2D.isKinematic = value;
    }
    
    public void DestroyShip()
    {
        //Part made by Lise Careel
        _audioSource.PlayOneShot(destroyShip, 0.1f);
        //END Part made by Lise Careel
        if (origin.wholeBadArmada.Contains(this))
            origin.wholeBadArmada.Remove(this);
        if(onOrbitOn != null)
            if (onOrbitOn._shipAnchorToThisPlanet.Contains(this))
                onOrbitOn.removeShipAnchor(this);
        GameMaster.Instance.AddCasualties(1);
        Destroy(this.gameObject);
    }


}
