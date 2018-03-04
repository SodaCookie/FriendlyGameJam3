using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLocation : MonoBehaviour {

	[Tooltip("When player enters what scene will be loaded.")]
	public string NextScene;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (NextScene != "") {
			SceneManager.LoadScene (NextScene);
		}
	}
}
