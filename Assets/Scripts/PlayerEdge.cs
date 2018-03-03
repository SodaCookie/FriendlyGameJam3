using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
	Up, Down, Left, Right
}

[RequireComponent(typeof(EdgeCollider2D))]
public class PlayerEdge : MonoBehaviour {

	public Direction direction;
	public PlayerControls controls;

	void OnTriggerEnter2D(Collider2D other) {
		controls.input.collisions [(int)direction] = true;
		if (other.GetComponent<Bendable> ()) {
			controls.input.colliders [(int)direction] = other;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		controls.input.collisions [(int)direction] = false;
		controls.input.colliders [(int)direction] = null;
	}
}
