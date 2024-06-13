using UnityEngine;
using System.IO;

[System.Serializable]
public class TransformData
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
}

public class HotspotButtonPositionManager : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "HotspotButtonTransform.json");
        Debug.Log("peristant path" + Application.persistentDataPath);
        LoadTransform();
    }

    private void OnApplicationQuit()
    {
        SaveTransform();
    }

    void SaveTransform()
    {
        TransformData data = new TransformData
        {
            objectName = gameObject.name,
            position = this.transform.localPosition,
            rotation = this.transform.localRotation,
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    void LoadTransform()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TransformData data = JsonUtility.FromJson<TransformData>(json);

            if (data.objectName == gameObject.name)
            {
                this.transform.localPosition = data.position;
                this.transform.localRotation = data.rotation;
            }
        }
    }

}
