using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance;

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
    }


    public Camera currentCamera;

}
