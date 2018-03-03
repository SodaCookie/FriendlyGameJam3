using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerInput : MonoBehaviour {

	PlayerControls controls;

	// Use this for initialization
	void Start () {
		controls = GetComponent<PlayerControls> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			controls.input.command = Command.Jump;
		} else if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)) {
			controls.input.command = Command.LeftBend;
		} else if (Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) {
			controls.input.command = Command.UpBend;
		} else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift)) {
			controls.input.command = Command.RightBend;
		}  else if (Input.GetKeyDown(KeyCode.Q)) {
			controls.input.command = Command.LeftPunch;
		} else if (Input.GetKeyDown(KeyCode.W)) {
			controls.input.command = Command.UpPunch;
		} else if (Input.GetKeyDown(KeyCode.E)) {
			controls.input.command = Command.RightPunch;
		} else if (Input.GetKey(KeyCode.A)) {
			controls.input.command = Command.Left;
		} else if (Input.GetKey(KeyCode.D)) {
			controls.input.command = Command.Right;
		}
	}
}
