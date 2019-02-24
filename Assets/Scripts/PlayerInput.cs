using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerInput : MonoBehaviour {

	PlayerControls controls;

    // Gesture recording variables
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    // Use this for initialization
    void Start () {
		controls = GetComponent<PlayerControls> ();

        // Grab joycons from JoyconManager
        foreach (Joycon jc in JoyconManager.Instance.j)
        {
            if (jc.isLeft) leftJoycon = jc;
            if (!jc.isLeft) rightJoycon = jc;
        }

        if (leftJoycon == null || rightJoycon == null)
        {
            Debug.LogError("Unable to find left and right joycon!");
        }
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			controls.input.command = Command.Jump;
		} else if ((Input.GetKeyDown (KeyCode.Q) && Input.GetKey (KeyCode.LeftShift)) || GestureRecognitionSystem.GetGestureRecognized (GestureType.LeftBend)) {
			controls.input.command = Command.LeftBend;
		} else if ((Input.GetKeyDown (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) || GestureRecognitionSystem.GetGestureRecognized (GestureType.UpBend)) {
			controls.input.command = Command.UpBend;
		} else if ((Input.GetKeyDown (KeyCode.E) && Input.GetKey (KeyCode.LeftShift)) || GestureRecognitionSystem.GetGestureRecognized (GestureType.RightBend)) {
			controls.input.command = Command.RightBend;
		} else if (Input.GetKeyDown (KeyCode.Q) || GestureRecognitionSystem.GetGestureRecognized (GestureType.LeftPunch)) {
			controls.input.command = Command.LeftPunch;
		} else if (Input.GetKeyDown (KeyCode.W) || GestureRecognitionSystem.GetGestureRecognized (GestureType.UpperCut)) {
			controls.input.command = Command.UpPunch;
		} else if (Input.GetKeyDown (KeyCode.E) || GestureRecognitionSystem.GetGestureRecognized (GestureType.RightPunch)) {
			controls.input.command = Command.RightPunch;
		} else if (Input.GetKey (KeyCode.A) || (leftJoycon != null && leftJoycon.GetButton (Joycon.Button.SHOULDER_1))) {
			controls.input.command = Command.Left;
		} else if (Input.GetKey (KeyCode.D) || (rightJoycon != null && rightJoycon.GetButton (Joycon.Button.SHOULDER_1))) {
			controls.input.command = Command.Right;
		} else if (Input.GetKey (KeyCode.R) || rightJoycon != null && rightJoycon.GetButton (Joycon.Button.HOME)) {
			controls.input.command = Command.Reset;
		}
	}
}
