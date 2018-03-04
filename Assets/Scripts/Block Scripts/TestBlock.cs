using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class TestBlock : MonoBehaviour {

	void Start () {
		Interactable bendable = GetComponent<Interactable> ();
		bendable.OnBend += OnBend;
	}
	
	void OnBend (GameObject player, Direction direction) {
		print ("WHOA YOU ARE A BENDER");
	}
}
