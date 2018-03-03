using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bendable : MonoBehaviour {
	
	public Action<GameObject> OnBend;

	public void Bend(GameObject player) {
		if (OnBend != null) {
			OnBend (player);
		}
	}
}
