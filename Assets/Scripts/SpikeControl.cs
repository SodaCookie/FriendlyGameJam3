using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeControl : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		PlayerControls controls = other.GetComponent<PlayerControls> ();
		if (controls != null) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}
}
