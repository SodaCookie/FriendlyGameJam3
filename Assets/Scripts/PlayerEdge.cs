using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
	Up, Down, Left, Right
}

[RequireComponent(typeof(Collider2D))]
public class PlayerEdge : MonoBehaviour {

	public Direction direction;
	public PlayerControls controls;

	void OnTriggerEnter2D(Collider2D other) {
		controls.input.collisions [(int)direction] += 1;
		if (other.gameObject.GetComponent<Interactable> ()) {
			controls.input.colliders [(int)direction].Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		controls.input.collisions [(int)direction] -= 1;
		if (other.gameObject.GetComponent<Interactable> ()) {
			controls.input.colliders [(int)direction].Remove(other.gameObject);
		}
	}
}
