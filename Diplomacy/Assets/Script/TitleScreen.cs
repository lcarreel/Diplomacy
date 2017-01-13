using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    [SerializeField]
    private Text difficultyText;
    [SerializeField]
    private Text speedText;

    private UtilType.Difficulty diff = UtilType.Difficulty.Easy;
    private UtilType.Speed spd = UtilType.Speed.FirstStep;

    private Animator _animator;

    private void Awake()
    {
        diff = UtilType.Difficulty.Easy;
        PlayerPrefs.SetInt(StaticValue.nameForDiffInPlayerPref, 0);
        spd = UtilType.Speed.FirstStep;
        PlayerPrefs.SetInt(StaticValue.nameForSpeedInPlayerPref, 0);
    }

	// Update is called once per frame
	public void OnClick () {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetTrigger("OnClick");
        
    }

    //Call by the animator in the animation named "lastTransition"
    public void ChargeFollowingScene()
    {
        if( diff == UtilType.Difficulty.Easy )
        {
            SceneManager.LoadScene("Space00");
        }
        else if ( diff == UtilType.Difficulty.Normal )
        {
            SceneManager.LoadScene("Space01");
        }
        else
        {
            SceneManager.LoadScene("Space02");
        }
    }

    public void Skip()
    {
        _animator.SetTrigger("Skip");
    }


    public void ChangeDifficulty()
    {
        string resString = "Easy";
        switch (diff)
        {
            case UtilType.Difficulty.Easy:
                diff = UtilType.Difficulty.Normal;
                resString = "Normal";
                PlayerPrefs.SetInt(StaticValue.nameForDiffInPlayerPref, 1);
                break;
            case UtilType.Difficulty.Normal:
                diff = UtilType.Difficulty.Hard;
                resString = "Hard";
                PlayerPrefs.SetInt(StaticValue.nameForDiffInPlayerPref, 2);
                break;
            case UtilType.Difficulty.Hard:
                diff = UtilType.Difficulty.Hell;
                resString = "Hell";
                PlayerPrefs.SetInt(StaticValue.nameForDiffInPlayerPref, 3);
                break;
            case UtilType.Difficulty.Hell:
                diff = UtilType.Difficulty.Easy;
                resString = "Easy";
                PlayerPrefs.SetInt(StaticValue.nameForDiffInPlayerPref, 0);
                break;
        }
        difficultyText.text = resString;
    }
    public void ChangeSpeed()
    {
        string resString = "FirsStep";
        switch (spd)
        {
            case UtilType.Speed.FirstStep:
                spd = UtilType.Speed.CruisingSpeed;
                resString = "CruisingSpeed";
                PlayerPrefs.SetInt(StaticValue.nameForSpeedInPlayerPref, 1);
                break;
            case UtilType.Speed.CruisingSpeed:
                spd = UtilType.Speed.High;
                resString = "High";
                PlayerPrefs.SetInt(StaticValue.nameForSpeedInPlayerPref, 2);
                break;
            case UtilType.Speed.High:
                spd = UtilType.Speed.LightSpeed;
                resString = "LightSpeed";
                PlayerPrefs.SetInt(StaticValue.nameForSpeedInPlayerPref, 3);
                break;
            case UtilType.Speed.LightSpeed:
                spd = UtilType.Speed.FirstStep;
                resString = "FirstStep";
                PlayerPrefs.SetInt(StaticValue.nameForSpeedInPlayerPref, 0);
                break;
        }

        speedText.text = resString;
    }


}