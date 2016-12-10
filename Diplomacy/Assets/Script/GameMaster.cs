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

    public PauseMenu pauseMenu;
    private bool onPause = false;

    //method 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InversePause();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }
    }

    public void InversePause()
    {

        if (onPause)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        onPause = !onPause;

    }

}
