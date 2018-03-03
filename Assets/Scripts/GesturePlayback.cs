using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GesturePlayback : MonoBehaviour {
    public Gesture gesture;

    private int curIndex = 0;
    private Rigidbody rb;
    private bool playing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
        gesture.LoadData();
	}
	
	// Update is called once per frame
	void Update () {
        handleInput();
        if (playing)
        {
            applyNextForce();
        }
	}

    public void Reset()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        curIndex = 0;
    }

    private void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
            playing = true;
        }
    }

    private void applyNextForce()
    {
        // If we reached the end stop playing and return
        if (curIndex >= gesture.data.Count)
        {
            playing = false;
            return;
        }

        rb.AddForce(gesture.data[curIndex].RAccelerometerData);
        curIndex++;
    }
}
