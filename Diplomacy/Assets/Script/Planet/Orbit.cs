using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    public Vector3 rotation = Vector3.right;

	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(rotation*Time.deltaTime);
    }
}
