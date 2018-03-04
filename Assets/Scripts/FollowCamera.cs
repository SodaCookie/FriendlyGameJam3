using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public float border = 0f;
	private GameObject playArea;
	private Camera cam;

	// Use this for initialization
	void Start () {
		playArea = GameObject.Find ("[Play Area]");
		Debug.Assert (playArea != null);

		cam = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find ("[Game System]").GetComponent<GameSystem> ().player;
		if (player != null) {
			Vector3 relPosition = player.transform.position - playArea.transform.position;
			float halfHeight = cam.orthographicSize;
			float halfWidth = cam.aspect * halfHeight;
			if (halfWidth + border - playArea.transform.localScale.x / 2<= relPosition.x && relPosition.x <= playArea.transform.localScale.x / 2 - halfWidth - border) {
				transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
			}
			if (halfHeight + border - playArea.transform.localScale.y / 2<= relPosition.y && relPosition.y <= playArea.transform.localScale.y / 2 - halfHeight - border) {
				transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
			}
		}
	}
}
