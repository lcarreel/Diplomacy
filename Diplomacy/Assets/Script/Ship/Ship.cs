using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public float speed = 1;

    private GameObject target;
    public Home origin;
    public bool inOGU = false;
    //Component
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Planet location;

    private AudioSource _audioSource;
    public AudioClip selectIdle;
    public AudioClip selectMining;
    public AudioClip sendToWar;
    public AudioClip sendToMine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
    public void GoToOGU()
    {
        inOGU = true;
        if (_spriteRenderer==null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = Color.blue+Color.green;

        if (target != null)
            target = null;
    }


    #region mouseDrag&DropHandler
    private void OnMouseDown()
    {
        if (inOGU)
        {
            _rigidbody2D.isKinematic = true;
            _animator.SetBool("HighLight", true);
            GameMaster.Instance.cursorCreator.Create(this.gameObject);
            if (location.GetComponent<Home>())
                _audioSource.PlayOneShot(selectIdle);
            else if (location.GetComponent<Resources>())
                _audioSource.PlayOneShot(selectMining);
        }
    }
    private void OnMouseDrag()
    {
        if (inOGU)
        {
            GameMaster.Instance.cursorCreator.UpdatePosition();
        }
    }
    private void OnMouseUp()
    {
        if (inOGU)
        {
            target = GameMaster.Instance.cursorCreator.ReturnTargetAndDisappear();
            if (target != null)
            {
                StartCoroutine(GoToTargetPoint(speed));

            }
            _rigidbody2D.isKinematic = false;
            _animator.SetBool("HighLight", false);
            if (target.GetComponent<Resources>() && target.GetComponent<Resources>().GetFlux().Count == 0)
                _audioSource.PlayOneShot(sendToMine);
            if (!target.GetComponent<Planet>().inOGU)
                _audioSource.PlayOneShot(sendToWar);
        }

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
        if (!planetAttacked.inOGU) {
            if (planetAttacked.getNumberOfShipOnIt() != 0)
            {
                AttackDefendedPlanet(planetAttacked);
            } else
            {
                AttackUndefendedPlanet(planetAttacked);
            }
        } else
        {
            planetAttacked.GoOrbit(this);
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
        if (homeAttacked != null)
        {
            if (homeAttacked.inOGU)
                homeAttacked.GoOrbit(this);
            else
                AttackHome(homeAttacked);
        }
        else
        {
            Resources resources = planetAttacked.GetComponent<Resources>();
            if (inOGU)
            {
                //IMPORTANT : planetAttacked != Home ! (because of the previous else)
                resources.joinOGU();
            } else
            {
                resources.quitOGU();
                foreach(Flux flux in resources.GetFlux())
                {
                    resources.removeFlux(flux);
                }
                resources.SetFlux(origin, gameObject.AddComponent<Flux>());
            }
            planetAttacked.GoOrbit(this);
        }
    }


    private void AttackHome(Home homeAttacked)
    {
        if (homeAttacked.Attacked(this))
        {
            DestroyShip();
        } else
        {
            homeAttacked.GoOrbit(this);
        }
        
    }


    public void ChangeKinematicState(bool value)
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = value;
    }

    private void DestroyShip()
    {
        if (origin.wholeBadArmada.Contains(this))
            origin.wholeBadArmada.Remove(this);
        GameMaster.Instance.AddCasualties(1);
        Destroy(this.gameObject);
    }


}
