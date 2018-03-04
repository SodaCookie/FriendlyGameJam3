using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		PlayerControls controls = other.GetComponent<PlayerControls> ();
		if (controls != null) {
			if (controls.keys > 0) {
				controls.keys -= 1;
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
	}
}
