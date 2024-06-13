using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

[System.Serializable]
public class TransformData
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class TransformDataList
{
    public List<TransformData> transforms = new List<TransformData>();
}

public class HotspotButtonPositionManager : MonoBehaviour
{
    private string filePath;

    private void OnEnable()
    {
        filePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");
        Debug.Log("OnEnable is called");
        LoadTransform();
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable is called");
        SaveTransform();
    }

    void SaveTransform()
    {
        TransformDataList dataList = new TransformDataList();

        // Check if the file exists and load existing data
        if (File.Exists(filePath))
        {
            try
            {
                string existingJson = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(existingJson))
                {
                    dataList = JsonUtility.FromJson<TransformDataList>(existingJson);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to read existing transform data: " + e.Message);
                dataList = new TransformDataList(); // Initialize to avoid null reference
            }
        }

        TransformData data = new TransformData
        {
            objectName = gameObject.name,
            position = transform.localPosition,
            rotation = transform.localRotation
        };

        if (dataList.transforms == null)
        {
            dataList.transforms = new List<TransformData>();
        }

        // Check if the gameObject name already exists in the list
        bool dataExists = false;
        for (int i = 0; i < dataList.transforms.Count; i++)
        {
            if (dataList.transforms[i].objectName == data.objectName)
            {
                // Override the existing data
                dataList.transforms[i] = data;
                dataExists = true;
                break;
            }
        }

        // If the data does not exist, add it to the list
        if (!dataExists)
        {
            dataList.transforms.Add(data);
        }

        try
        {
            string json = JsonUtility.ToJson(dataList, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Transform saved successfully to " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save transform data: " + e.Message);
        }
    }

    void LoadTransform()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                TransformDataList dataList = JsonUtility.FromJson<TransformDataList>(json);

                foreach (TransformData data in dataList.transforms)
                {
                    if (data.objectName == gameObject.name)
                    {
                        transform.localPosition = data.position;
                        transform.localRotation = data.rotation;
                        Debug.Log("Transform loaded successfully for " + gameObject.name);
                        return;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No saved transform data found for " + gameObject.name);
        }
    }
}
