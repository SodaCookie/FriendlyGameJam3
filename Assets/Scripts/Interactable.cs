using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interactable : MonoBehaviour {
	
	public Action<GameObject, Direction> OnBend;
	public Action OnPunch;

	public void Bend(GameObject player, Direction direction) {
		if (OnBend != null) {
			OnBend (player, direction);
		}
	}

	public void Punch() {
		if (OnPunch != null) {
			OnPunch ();
		}
	}
}
