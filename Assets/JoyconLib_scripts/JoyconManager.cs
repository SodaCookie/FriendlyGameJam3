﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class JoyconManager: MonoBehaviour
{

    // Settings accessible via Unity
    public bool EnableIMU = true;
    public bool EnableLocalize = true;

	// Different operating systems either do or don't like the trailing zero
	private const ushort vendor_id = 0x57e;
	private const ushort vendor_id_ = 0x057e;
	private const ushort product_l = 0x2006;
	private const ushort product_r = 0x2007;

    public List<Joycon> j; // Array of all connected Joy-Cons
    static JoyconManager instance;

    public static JoyconManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneExit;
        Debug.Log("Checkpoint");
        if (instance != null) Destroy(gameObject);
        instance = this;
		int i = 0;
        Debug.Log("Checkpoint");

        j = new List<Joycon>();
		bool isLeft = false;
        Debug.Log("Checkpoint");

        HIDapi.hid_init();
        Debug.Log("Checkpoint");

        IntPtr ptr = HIDapi.hid_enumerate(vendor_id, 0x0);
        Debug.Log("Checkpoint");

        IntPtr top_ptr = ptr;
        Debug.Log("Checkpoint");

        if (ptr == IntPtr.Zero)
		{
            Debug.Log("SubCheckpoint");

            ptr = HIDapi.hid_enumerate(vendor_id_, 0x0);
			if (ptr == IntPtr.Zero)
			{
                Debug.Log("SubCheckpoint");

                HIDapi.hid_free_enumeration(ptr);
				Debug.Log ("No Joy-Cons found!");
			}
		}
        Debug.Log("Checkpoint");

        hid_device_info enumerate;
		while (ptr != IntPtr.Zero) {
            Debug.Log("Checkpoint");

            enumerate = (hid_device_info)Marshal.PtrToStructure (ptr, typeof(hid_device_info));

			Debug.Log (enumerate.product_id);
				if (enumerate.product_id == product_l || enumerate.product_id == product_r) {
					if (enumerate.product_id == product_l) {
						isLeft = true;
						Debug.Log ("Left Joy-Con connected.");
					} else if (enumerate.product_id == product_r) {
						isLeft = false;
						Debug.Log ("Right Joy-Con connected.");
					} else {
						Debug.Log ("Non Joy-Con input device skipped.");
					}
					IntPtr handle = HIDapi.hid_open_path (enumerate.path);
					HIDapi.hid_set_nonblocking (handle, 1);
					j.Add (new Joycon (handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft));
					++i;
				}
				ptr = enumerate.next;
			}
		HIDapi.hid_free_enumeration (top_ptr);
    }

    void Start()
    {
		for (int i = 0; i < j.Count; ++i)
		{
			Debug.Log (i);
			Joycon jc = j [i];
			byte LEDs = 0x0;
			LEDs |= (byte)(0x1 << i/2);
			jc.Attach (leds_: LEDs);
			jc.Begin ();
		}
    }

    void Update()
    {
		for (int i = 0; i < j.Count; ++i)
		{
			j[i].Update();
		}
    }

    void OnApplicationQuit()
    {
        ExitProtocol();
    }

    public void ExitProtocol()
    {
        for (int i = 0; i < j.Count; ++i)
        {
            j[i].Detach();
        }
        HIDapi.hid_exit();
    }

    public void OnSceneExit(Scene scene)
    {
        ExitProtocol();
    }
}
