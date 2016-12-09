using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {


    public Vector3 movement = new Vector3(0, 5, 0);


    private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start () {
        _particleSystem = this.GetComponent<ParticleSystem>();
        StartCoroutine(Appear(5) );
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerator Appear(int height)
    {
        while(this.transform.position.x < height)
        {
            this.transform.Translate(movement*Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
        _particleSystem.Play();
    }
}
