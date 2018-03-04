using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRecognitionSystem : MonoBehaviour {

    public static GestureRecognitionSystem instance;

    [Tooltip("All the gestures to look for when recognizing.")]
    public Gesture[] gestures;

    // Whether this class should record inputs for gestures, or just be used as utility for recognition
    public bool live = true;
    // Whether to be verbose about recognition
    public bool verbose;
    // What distance is considered too far to be a match?
    public float recognitionThreshold = 8;

    // Gesture recording variables
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    private float recordingStartTime = 0;
    private bool recording = false;
    private List<Gesture.DataPoint> recordingBuffer = new List<Gesture.DataPoint>();

    // Queriable Input System vars
    Gesture gestureRecognized;
    Gesture lastGestureRecognized;

    public static bool GetGestureRecognized(string gesture_id)
    {
        if (instance.gestureRecognized == null) return false;
        return instance.gestureRecognized.gestureID == gesture_id;
    }

    public static bool GetLastGesture(string gesture_id)
    {
        if (instance.lastGestureRecognized == null) return false;
        return instance.lastGestureRecognized.gestureID == gesture_id;
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        // Load all the gestures from disk
		foreach (Gesture gesture in gestures)
        {
            gesture.LoadData();
        }

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
	
    /// <summary>
    /// Match an input data for a gesture with the gesture library
    /// </summary>
    /// <param name="inputGesture"></param>
    /// <returns></returns>
	public Gesture Recognize(List<Gesture.DataPoint> inputData)
    {
        float curBestDistance = float.MaxValue;
        Gesture currentBestGesture = null;

        float dist;
        foreach (Gesture gesture in gestures)
        {
            dist = gesture.GetDistanceTo(inputData);
            if (dist < curBestDistance)
            {
                curBestDistance = dist;
                currentBestGesture = gesture;
            }
        }

        if (curBestDistance <= recognitionThreshold)
        {
            if (verbose) Debug.Log("Recognized as " + currentBestGesture.gestureID + " with distance " + curBestDistance + " and index " + currentBestGesture.indexChosen);
        }
        else
        {
            if (verbose) Debug.Log("System did not recognize gesture");
            currentBestGesture = null;
        }


        return currentBestGesture;
    }

    // Update is called once per frame
    void Update()
    {
        if (live)
        {
            gestureRecognized = null;
            if (recording) RecordCurrentInputs();
            handleInput();
        }
    }

    private void BeginRecording()
    {
        recordingBuffer.Clear();
        recordingStartTime = Time.time;
        recording = true;
    }

    private void EndRecording()
    {
        recording = false;
    }

    private void RecordCurrentInputs()
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
            }
        }
        // If the buttons are let go
        else
        {
            // Stop recording
            if (recording)
            {
                EndRecording();
                Gesture recognized = Recognize(recordingBuffer);
                if (recognized != null)
                {
                    lastGestureRecognized = recognized;
                    gestureRecognized = recognized;
                }
            }
        }
    }
}

public class GestureType
{
    public const string LeftPunch = "left_punch";
    public const string RightPunch = "right_punch";
    public const string UpperCut = "upper_cut";
    public const string UpBend = "up_bend";
    public const string RightBend = "right_bend";
    public const string LeftBend = "left_bend";
}