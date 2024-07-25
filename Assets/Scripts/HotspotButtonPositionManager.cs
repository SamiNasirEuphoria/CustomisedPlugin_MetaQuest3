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
#if UNITY_EDITOR
        Debug.Log("OnDisable is called");
        SaveTransform();
#endif
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





//using UnityEngine;
//using System.IO;
//using System.Collections.Generic;
//using System;

//[System.Serializable]
//public class TransformData
//{
//    public string objectName;
//    public Vector3 position;
//    public Quaternion rotation;
//}

//[System.Serializable]
//public class TransformDataList
//{
//    public List<TransformData> transforms = new List<TransformData>();
//}

//public class HotspotButtonPositionManager : MonoBehaviour
//{
//    private string filePath;// = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");

//    private void OnEnable()
//    {
//        filePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");
//        //filePath = "Assets/Resources/HotspotButtonTransforms.json";
//        LoadTransform();
//    }
//    private void OnDisable()
//    {

//#if UNITY_EDITOR
//        SaveTransform();
//#endif

//    }
//    void SaveTransform()
//    {
//        TransformDataList dataList = new TransformDataList();

//        // Check if the file exists and load existing data
//        if (File.Exists(filePath))
//        {
//            try
//            {
//                string existingJson = File.ReadAllText(filePath);
//                if (!string.IsNullOrEmpty(existingJson))
//                {
//                    dataList = JsonUtility.FromJson<TransformDataList>(existingJson);
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Failed to read existing transform data: " + e.Message);
//                dataList = new TransformDataList(); // Initialize to avoid null reference
//            }
//        }

//        TransformData data = new TransformData
//        {
//            objectName = gameObject.name,
//            position = transform.localPosition,
//            rotation = transform.localRotation
//        };

//        if (dataList.transforms == null)
//        {
//            dataList.transforms = new List<TransformData>();
//        }

//        // Check if the gameObject name already exists in the list
//        bool dataExists = false;
//        for (int i = 0; i < dataList.transforms.Count; i++)
//        {
//            if (dataList.transforms[i].objectName == data.objectName)
//            {
//                // Override the existing data
//                dataList.transforms[i] = data;
//                dataExists = true;
//                break;
//            }
//        }

//        // If the data does not exist, add it to the list
//        if (!dataExists)
//        {
//            dataList.transforms.Add(data);
//        }

//        try
//        {
//            string json = JsonUtility.ToJson(dataList, true);
//            File.WriteAllText(filePath, json);
//            Debug.Log("Transform saved successfully to " + filePath);
//        }
//        catch (Exception e)
//        {
//            Debug.LogError("Failed to save transform data: " + e.Message);
//        }
//    }

//    void LoadTransform()
//    {
//        if (File.Exists(filePath))
//        {
//            string json = File.ReadAllText(filePath);
//            if (!string.IsNullOrEmpty(json))
//            {
//                TransformDataList dataList = JsonUtility.FromJson<TransformDataList>(json);

//                foreach (TransformData data in dataList.transforms)
//                {
//                    if (data.objectName == gameObject.name)
//                    {
//                        transform.localPosition = data.position;
//                        transform.localRotation = data.rotation;
//                        Debug.Log($"Transform loaded successfully for {gameObject.name} and the position is {transform.position}");
//                        return;
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("No saved transform data found for " + gameObject.name+"file path is"+ filePath);
//        }
//    }
//}




//using UnityEngine;
//using System.IO;
//using System.Collections.Generic;
//using System;

//[System.Serializable]
//public class TransformData
//{
//    public string objectName;
//    public Vector3 position;
//    public Quaternion rotation;
//}

//[System.Serializable]
//public class TransformDataList
//{
//    public List<TransformData> transforms = new List<TransformData>();
//}

//public class HotspotButtonPositionManager : MonoBehaviour
//{
//    private string persistentFilePath;
//    private string streamingAssetsFilePath;
//    private void OnEnable()
//    {
//        persistentFilePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransforms.json");
//        streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, "HotspotButtonTransforms.json");
//#if UNITY_EDITOR
//        // Copy StreamingAssets from persistentDataPath on first launch
//        if (File.Exists(persistentFilePath))
//        {
//            Debug.Log("Calling for copy data");
//            CopyInitialData();
//        }
//#endif
//        LoadTransform();
//    }

//    private void OnDisable()
//    {
//#if UNITY_EDITOR
//        SaveTransform();
//#endif
//    }

//    private void CopyInitialData()
//    {
//        // Copy JSON from persistentDataPath to StreamingAssets
//        if (File.Exists(persistentFilePath))
//        {
//            try
//            {
//                string jsonContent = File.ReadAllText(persistentFilePath);
//                File.WriteAllText(streamingAssetsFilePath, jsonContent);
//                Debug.Log("Data copied from persistentDataPath to StreamingAssets.");
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Failed to copy data to StreamingAssets: " + e.Message);
//            }
//        }
//        else
//        {
//            Debug.LogError("No data found in persistentDataPath.");
//        }
//    }

//    void SaveTransform()
//    {
//        TransformDataList dataList = new TransformDataList();

//        // Check if the file exists in persistentDataPath and load existing data
//        if (File.Exists(persistentFilePath))
//        {
//            try
//            {
//                string existingJson = File.ReadAllText(persistentFilePath);
//                if (!string.IsNullOrEmpty(existingJson))
//                {
//                    dataList = JsonUtility.FromJson<TransformDataList>(existingJson);
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Failed to read existing transform data: " + e.Message);
//                dataList = new TransformDataList(); // Initialize to avoid null reference
//            }
//        }

//        TransformData data = new TransformData
//        {
//            objectName = gameObject.name,
//            position = transform.localPosition,
//            rotation = transform.localRotation
//        };

//        if (dataList.transforms == null)
//        {
//            dataList.transforms = new List<TransformData>();
//        }

//        // Check if the gameObject name already exists in the list
//        bool dataExists = false;
//        for (int i = 0; i < dataList.transforms.Count; i++)
//        {
//            if (dataList.transforms[i].objectName == data.objectName)
//            {
//                // Override the existing data
//                dataList.transforms[i] = data;
//                dataExists = true;
//                break;
//            }
//        }

//        // If the data does not exist, add it to the list
//        if (!dataExists)
//        {
//            dataList.transforms.Add(data);
//        }

//        try
//        {
//            // Save to persistentDataPath
//            string json = JsonUtility.ToJson(dataList, true);
//            File.WriteAllText(persistentFilePath, json);
//            Debug.Log("Transform saved successfully to " + persistentFilePath);

//            // Also save to StreamingAssets
//            File.WriteAllText(streamingAssetsFilePath, json);
//            Debug.Log("Transform also saved to " + streamingAssetsFilePath);
//        }
//        catch (Exception e)
//        {
//            Debug.LogError("Failed to save transform data: " + e.Message);
//        }
//    }

//    void LoadTransform()
//    {
//        if (File.Exists(streamingAssetsFilePath))
//        {
//            string json = File.ReadAllText(streamingAssetsFilePath);
//            Debug.Log("Loaded JSON from StreamingAssets: " + json); // Log JSON data for debugging

//            if (!string.IsNullOrEmpty(json))
//            {
//                TransformDataList dataList = JsonUtility.FromJson<TransformDataList>(json);

//                foreach (TransformData data in dataList.transforms)
//                {
//                    if (data.objectName == gameObject.name)
//                    {
//                        transform.localPosition = data.position;
//                        transform.localRotation = data.rotation;
//                        Debug.Log("Transform loaded successfully for " + gameObject.name);
//                        return;
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogError("Empty JSON data.");
//            }
//        }
//        else
//        {
//            Debug.Log("No saved transform data found in StreamingAssets for " + gameObject.name);
//        }
//    }
//}
