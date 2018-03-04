using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HurtBox : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		Interactable interact = other.GetComponent<Interactable> ();
		if (interact != null) {
			interact.Punch ();
		}
	}
}
