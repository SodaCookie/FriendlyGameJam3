using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {

	private GameSystem system;

	void Start() {
		system = GameObject.Find ("[Game System]").GetComponent<GameSystem> ();
		Debug.Assert (system != null);
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			system.Reset ();
		} else {
			Destroy (collider.gameObject);
		}
	}
}
