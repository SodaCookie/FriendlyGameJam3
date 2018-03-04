using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Gesture")]
public class Gesture : ScriptableObject {
    public string gestureID;
    public string dataLoadPath;

    [HideInInspector]
    public List<List<DataPoint>> data = new List<List<DataPoint>>();

    public int indexChosen;

    // Loads data from path relative to StreamingAssetsPath
    public void LoadData()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, dataLoadPath);

        if (File.Exists(fullPath))
        {
            string loadedJson = File.ReadAllText(fullPath);
            DataSerializationContainer loadedData = JsonUtility.FromJson<DataSerializationContainer>(loadedJson);
            data = new List<List<DataPoint>>();
            foreach (DataPointListSerializationContainer dataSeries in loadedData.dataSeriesList)
            {
                data.Add(new List<DataPoint>(dataSeries.dataSeries));
            }
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
        string fullPath = Application.streamingAssetsPath + "/" + dataLoadPath;

        string jsonData = JsonUtility.ToJson(new DataSerializationContainer(data));
        File.WriteAllText(fullPath, jsonData);
        Debug.Log("Successfully saved gesture " + gestureID + " to " + fullPath);
    }

    // Takes the average of distance to each data series
    public float GetDistanceTo(List<DataPoint> other)
    {
        float distance = float.MaxValue;

        int count;
        float tempDistance;
        int curInd = 0;
        // Distance to each data series
        foreach (List<DataPoint> dataSeries in data)
        {
            count = Mathf.Min(dataSeries.Count, other.Count);

            tempDistance = 0;

            for (int i = 0; i < count; i++)
            {
                tempDistance += dataSeries[i].distanceTo(other[i]);
            }

            tempDistance = tempDistance / count;

            //distance += tempDistance;
            if (tempDistance < distance)
            {
                indexChosen = curInd;
            }
            distance = Mathf.Min(distance, tempDistance);
            curInd++;
        }

        //distance = distance / data.Count;

        return distance;
    }

    public string GetStatus()
    {
        return "Number of data entries: " + data.Count;
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

        public float distanceTo(DataPoint other)
        {
            float toRet = 0;

            toRet += Mathf.Pow(other.RAccelerometerData.x - RAccelerometerData.x, 2);
            toRet += Mathf.Pow(other.RAccelerometerData.y - RAccelerometerData.y, 2);
            toRet += Mathf.Pow(other.RAccelerometerData.z - RAccelerometerData.z, 2);

            toRet += Mathf.Pow(other.LAccelerometerData.x - LAccelerometerData.x, 2);
            toRet += Mathf.Pow(other.LAccelerometerData.y - LAccelerometerData.y, 2);
            toRet += Mathf.Pow(other.LAccelerometerData.z - LAccelerometerData.z, 2);

            toRet += Mathf.Pow(other.RGyroscopeData.x - RGyroscopeData.x, 2);
            toRet += Mathf.Pow(other.RGyroscopeData.y - RGyroscopeData.y, 2);
            toRet += Mathf.Pow(other.RGyroscopeData.z - RGyroscopeData.z, 2);

            toRet += Mathf.Pow(other.LGyroscopeData.x - LGyroscopeData.x, 2);
            toRet += Mathf.Pow(other.LGyroscopeData.y - LGyroscopeData.y, 2);
            toRet += Mathf.Pow(other.LGyroscopeData.z - LGyroscopeData.z, 2);

            toRet += Mathf.Pow(other.time - time, 2);

            toRet = Mathf.Sqrt(toRet);

            return toRet;
        }
    }

    [System.Serializable]
    struct DataSerializationContainer
    {
        public DataPointListSerializationContainer[] dataSeriesList;

        public DataSerializationContainer(List<List<DataPoint>> pDataList)
        {
            dataSeriesList = new DataPointListSerializationContainer[pDataList.Count];
            for (int i = 0; i < pDataList.Count; i++)
            {
                dataSeriesList[i] = new DataPointListSerializationContainer(pDataList[i].ToArray());
            }
        }
    }

    [System.Serializable]
    struct DataPointListSerializationContainer
    {
        public DataPoint[] dataSeries;

        public DataPointListSerializationContainer(DataPoint[] pDataSerries)
        {
            dataSeries = pDataSerries;
        }
    }
}
