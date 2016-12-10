using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Home : Planet {

    private int civil = 50;
    private int mood = 50;

    [SerializeField]
    private Text civilNumber;

    [SerializeField]
    private SpriteRenderer haloOut;
    [SerializeField]
    private SpriteRenderer haloOGU;
    [SerializeField]
    private GameObject orbit;

    //temporary
    public bool peopleHaveEnoughToLive = true;
    public int shipCreationRange = 3;

    public void Start()
    {
        SetCivil( (int)Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
        InvokeRepeating( "CreateShip", Random.Range(0, StaticValue.tempo * shipCreationRange), StaticValue.tempo * shipCreationRange);
    }

    public int GetCivil()
    {
        return civil;
    }
    public void SetCivil(int value)
    {
        civil = value;
        UpdateCivilText();
    }

    public void destroyCivil(int numberOfShipAttacked)
    {
        SetCivil(GetCivil() - numberOfShipAttacked * StaticValue.numberOfCivilDeadByShip );
    }

    private void UpdateCivilText()
    {
        civilNumber.text = civil.ToString();
    }

    public void AddCivilPeriodically()
    {
        if (peopleHaveEnoughToLive)
            SetCivil(GetCivil() + 1);
    }
    public void CreateShip()
    {
        Ship shipCreate = Instantiate( GameMaster.Instance.ship).GetComponent<Ship>();
        shipCreate.transform.position = this.transform.position + ((Vector3)Vector2.right);
        shipCreate.transform.SetParent(orbit.transform);
    }

}
