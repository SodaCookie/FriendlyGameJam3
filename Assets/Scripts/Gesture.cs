using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Gesture")]
public class Gesture : ScriptableObject {
    public string gestureID;
    public string dataLoadPath;

    [HideInInspector]
    public List<DataPoint> data;

    // Loads data from path relative to StreamingAssetsPath
    public void LoadData()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, dataLoadPath);

        if (File.Exists(fullPath))
        {
            string loadedJson = File.ReadAllText(fullPath);
            DataSerializationContainer loadedData = JsonUtility.FromJson<DataSerializationContainer>(loadedJson);
            data = new List<DataPoint>(loadedData.dataArray);
            Debug.Log("Successfully loaded gesture " + gestureID);

        }
        else
        {
            Debug.LogError("Failed to load gesture data for " + gestureID);
        }

    }

    // Saves the data to path relative to StreamingAssetsPath
    public void SaveData()
    {
        //string fullPath = Path.Combine(Application.streamingAssetsPath, dataLoadPath);
        string fullPath = Application.streamingAssetsPath + "/" + dataLoadPath;

        string jsonData = JsonUtility.ToJson(new DataSerializationContainer(data));
        File.WriteAllText(fullPath, jsonData);
        Debug.Log("Successfully saved gesture " + gestureID + " to " + fullPath);
    }

    // Helper Struct
    [System.Serializable]
    public struct DataPoint
    {
        public Vector3 RAccelerometerData;
        public Vector3 LAccelerometerData;
        public Vector3 RGyroscopeData;
        public Vector3 LGyroscopeData;
        public float time;

        public DataPoint(Vector3 AccelDataLeft, Vector3 AccelDataRight, Vector3 GyroDataLeft, Vector3 GyroDataRight, float t)
        {
            LAccelerometerData = AccelDataLeft;
            RAccelerometerData = AccelDataRight;

            LGyroscopeData = GyroDataLeft;
            RGyroscopeData = GyroDataRight;

            time = t;
        }
    }

    [System.Serializable]
    struct DataSerializationContainer
    {
        public DataPoint[] dataArray;

        public DataSerializationContainer(DataPoint[] pDataArray)
        {
            dataArray = pDataArray;
        }

        public DataSerializationContainer(List<DataPoint> pDataList)
        {
            dataArray = pDataList.ToArray();
        }
    }
}
