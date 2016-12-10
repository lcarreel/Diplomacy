using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {


    public void ResumeGame()
    {
        GameMaster.Instance.InversePause();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
