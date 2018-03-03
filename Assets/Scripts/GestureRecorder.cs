using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GestureRecorder : MonoBehaviour {
    public Gesture gesture;

    // Grabbed automatically from Joycon Manager
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    private float recordingStartTime = 0;
    private bool recording = false;
    private List<Gesture.DataPoint> recordingBuffer = new List<Gesture.DataPoint>();

	// Use this for initialization
	void Start () {
		// Grab joycons from JoyconManager
        foreach (Joycon jc in JoyconManager.Instance.j)
        {
            if (jc.isLeft) leftJoycon = jc;
            if (!jc.isLeft) rightJoycon = jc;
        }

        if (leftJoycon == null || rightJoycon == null)
        {
            Debug.LogError("Unable to find left and right joycon!");
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (recording) RecordCurrentInputs();
        handleInput();
	}

    public void BeginRecording()
    {
        recordingBuffer.Clear();
        recordingStartTime = Time.time;
        recording = true;
    }

    public void EndRecording()
    {
        recording = false;
        gesture.data = recordingBuffer;
        gesture.SaveData();
    }

    public void RecordCurrentInputs()
    {
        Vector3 rA = rightJoycon.GetAccel();
        Vector3 lA = leftJoycon.GetAccel();
        Vector3 rG = rightJoycon.GetGyro();
        Vector3 lG = leftJoycon.GetGyro();
        float time = Time.time - recordingStartTime;

        Gesture.DataPoint dp = new Gesture.DataPoint(lA, rA, lG, rG, time);

        recordingBuffer.Add(dp);
    }

    private void handleInput()
    {
        // If both buttons are pressed
        if (rightJoycon.GetButton(Joycon.Button.SR) && leftJoycon.GetButton(Joycon.Button.SL))
        {
            // Begin recording
            if (!recording)
            {
                BeginRecording();
                Debug.Log("Start Recording");
            }
        }
        // If the buttons are let go
        else
        {
            // Stop recording
            if (recording)
            {
                EndRecording();
                Debug.Log("End Recording");
            }
        }
    }
}
