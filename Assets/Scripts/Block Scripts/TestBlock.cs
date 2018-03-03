using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bendable))]
public class TestBlock : MonoBehaviour {

	void Start () {
		Bendable bendable = GetComponent<Bendable> ();
		bendable.OnBend += OnBend;
	}
	
	void OnBend (GameObject player) {
		print ("WHOA YOU ARE A BENDER");
	}
}
