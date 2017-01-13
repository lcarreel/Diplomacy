using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script was made by Lise Careel
/// </summary>
public class Sound : MonoBehaviour {

    public static string muteStringInPlayerPrefs = "MUTE";


    public static Sound Instance;

    [SerializeField]
    private Text muteText;

    bool mute = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        mute = (PlayerPrefs.GetInt(muteStringInPlayerPrefs,1) == 0);
        if (mute)
        {
            Mute();
        } else
        {
            UnMute();
        }
    }

    public void ChangeMuteOrNot()
    {
        if (mute)
        {
            UnMute();
        } else
        {
            Mute();
        }
    }

    void Mute()
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.mute = true;
        }
        muteText.text = "✓Mute";
        mute = true;
        PlayerPrefs.SetInt(muteStringInPlayerPrefs, 0);
    }
    void UnMute()
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.mute = false;
        }
        muteText.text = "✗Mute";
        mute = false;
        PlayerPrefs.SetInt(muteStringInPlayerPrefs, 1);
    }



}
