using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {
    
	// Update is called once per frame
	public void OnClick () {
        SceneManager.LoadScene("Space01");
	}
}
