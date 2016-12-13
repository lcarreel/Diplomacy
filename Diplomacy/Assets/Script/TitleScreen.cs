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


    private void Awake()
    {
        diff = UtilType.Difficulty.Easy;
        PlayerPrefs.SetInt("DIFFICULTY", 0);
        spd = UtilType.Speed.CruisingSpeed;
        PlayerPrefs.SetInt("SPEED", 0);
    }
    Animator _animator;
	// Update is called once per frame
	public void OnClick () {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetTrigger("OnClick");

    }

    public void ChargeFollowingScene()
    {

        SceneManager.LoadScene("Space01");
    }

    public void Skip()
    {
        _animator.SetTrigger("Skip");
    }


    public void ChangeDifficulty()
    {
        string resString = "Easy";
        string nameInPlayerPref = "DIFFICULTY";
        switch (diff)
        {
            case UtilType.Difficulty.Easy:
                diff = UtilType.Difficulty.Normal;
                resString = "Normal";
                PlayerPrefs.SetInt(nameInPlayerPref, 1);
                break;
            case UtilType.Difficulty.Normal:
                diff = UtilType.Difficulty.Hard;
                resString = "Hard";
                PlayerPrefs.SetInt(nameInPlayerPref, 2);
                break;
            case UtilType.Difficulty.Hard:
                diff = UtilType.Difficulty.Hell;
                resString = "Hell";
                PlayerPrefs.SetInt(nameInPlayerPref, 3);
                break;
            case UtilType.Difficulty.Hell:
                diff = UtilType.Difficulty.Easy;
                resString = "Easy";
                PlayerPrefs.SetInt(nameInPlayerPref, 0);
                break;
        }
        difficultyText.text = resString;
    }
    public void ChangeSpeed()
    {
        string resString = "FirsStep";
        string nameInPlayerPref = "SPEED";
        switch (spd)
        {
            case UtilType.Speed.FirstStep:
                spd = UtilType.Speed.CruisingSpeed;
                resString = "CruisingSpeed";
                PlayerPrefs.SetInt(nameInPlayerPref, 1);
                break;
            case UtilType.Speed.CruisingSpeed:
                spd = UtilType.Speed.High;
                resString = "High";
                PlayerPrefs.SetInt(nameInPlayerPref, 2);
                break;
            case UtilType.Speed.High:
                spd = UtilType.Speed.LightSpeed;
                resString = "LightSpeed";
                PlayerPrefs.SetInt(nameInPlayerPref, 3);
                break;
            case UtilType.Speed.LightSpeed:
                spd = UtilType.Speed.FirstStep;
                resString = "FirstStep";
                PlayerPrefs.SetInt(nameInPlayerPref, 0);
                break;
        }

        speedText.text = resString;
    }


}