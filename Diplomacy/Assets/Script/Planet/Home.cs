using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Home : Planet {

    private int civil = 50;

    [SerializeField]
    private Text civilNumber;


    //temporary
    public bool peopleHaveEnoughToLive = true;

    public void Start()
    {
        SetCivil( (int)Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
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

}
