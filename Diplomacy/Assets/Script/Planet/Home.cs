using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Home : Planet {

    private int civil = 50;
    private int mood = 50;

    private float food = 0;
    private float foodNeeded;
    private float powr = 0;
    private float powrNeeded;
    private float iron = 0;
    private float ironNeeded;

    [SerializeField]
    private SpriteRenderer haloOut;
    [SerializeField]
    private SpriteRenderer haloOGU;
    [SerializeField]
    private GameObject orbit;

    private HomeUI homeUI;

    //temporary
    public bool peopleHaveEnoughToLive = true;
    public int shipCreationRange = 3;

    public void Start()
    {
        homeUI = Instantiate(GameMaster.Instance.homeUI).GetComponent<HomeUI>();
        homeUI.transform.SetParent(GameMaster.Instance.canvasWorld.transform);
        homeUI.transform.position = this.transform.position;
        homeUI.transform.localScale = Vector3.one;

        SetCivil( (int)Random.Range(50,230) );
        InvokeRepeating( "AddCivilPeriodically", StaticValue.tempo, StaticValue.tempo);
        InvokeRepeating( "CreateShip", Random.Range(0, StaticValue.tempo * shipCreationRange), StaticValue.tempo * shipCreationRange);

        foodNeeded = civil;
        powrNeeded = civil;
        ironNeeded = civil;

        food = (int)Random.Range(100, 280);
        iron = (int)Random.Range(100, 280);
        powr = (int)Random.Range(100, 280);

        UpdateValueAndVisual();
    }

    private void UpdateValueAndVisual()
    {
        peopleHaveEnoughToLive = (food < foodNeeded || powr < powrNeeded || iron < ironNeeded);

        UpdateUI();
    }

    public int GetCivil()
    {
        return civil;
    }
    public void SetCivil(int value)
    {
        civil = value;
        UpdateCivilText();
        foodNeeded = civil;
        powrNeeded = civil;
        ironNeeded = civil;
        UpdateValueAndVisual();
    }

    public void destroyCivil(int numberOfShipAttacked)
    {
        SetCivil(GetCivil() - numberOfShipAttacked * StaticValue.numberOfCivilDeadByShip );
    }

    private void UpdateCivilText()
    {
        homeUI.civilNumber.text = civil.ToString();
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
        //define shipCamp
    }


    #region GAUGE

    public void UpdateUI()
    {
        homeUI.ChangeValue(new Vector3(food,iron,powr),mood,new Vector3(foodNeeded,ironNeeded,powrNeeded));
    }

    public void AddRessources(Vector3 supply)
    {
        //food / flux_nbr , iron / flux_nbr , powr / flux_nbr

        food += supply.x;
        iron += supply.y;
        powr += supply.z;

        UpdateValueAndVisual();
    }


    #endregion

}
