using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class StoneBendBlock : MonoBehaviour {

	public GameObject bendPrefab;
	public float launchPower = 20;

	void Start () {
		Interactable bendable = GetComponent<Interactable> ();
		bendable.OnBend += OnBend;
	}
	
	void OnBend (GameObject player, Direction direction) {
		GameObject block = Instantiate (bendPrefab);
		switch (direction) {
		case Direction.Up:
			// Bend upwards
			player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, launchPower);
			block.transform.position = player.transform.position + Vector3.down * 1.05f;
			StartCoroutine (MoveBlock(block, player.transform.position, Vector3.down));
			break;
		case Direction.Down:
			// Bend downwards
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -launchPower);
			block.transform.position = player.transform.position + Vector3.up * 1.05f;
			StartCoroutine (MoveBlock(block, player.transform.position, Vector3.up));
			break;
		case Direction.Left:
			// Bend right
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(-launchPower, 5);
			block.transform.position = player.transform.position + Vector3.right * 1.05f;
			block.transform.rotation = Quaternion.Euler (0, 0, -90);
			StartCoroutine (MoveBlock(block, player.transform.position, Vector3.right));
			break;
		case Direction.Right:
			// Bend left
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(launchPower, 5);
			block.transform.position = player.transform.position + Vector3.left * 1.05f;
			block.transform.rotation = Quaternion.Euler (0, 0, 90);
			StartCoroutine (MoveBlock(block, player.transform.position, Vector3.left));
			break;
		}
	}

	IEnumerator MoveBlock(GameObject block, Vector3 playerPosition, Vector3 direction) {
		block.GetComponent<BoxCollider2D> ().enabled = false;
		for (int i = 5; i >= 0 ; i--) {
			block.transform.position = playerPosition + (direction * (float)i / 5f);
			yield return null;
		}
		block.GetComponent<BoxCollider2D> ().enabled = true;
	}
}
