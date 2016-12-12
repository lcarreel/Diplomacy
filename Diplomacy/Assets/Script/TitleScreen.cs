using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {

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
}
