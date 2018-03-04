using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		PlayerControls controls = other.GetComponent<PlayerControls> ();
		if (controls != null) {
			controls.keys += 1;
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rot = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler (new Vector3 (rot.x, (rot.y + 1) % 360, rot.z));
	}
}
