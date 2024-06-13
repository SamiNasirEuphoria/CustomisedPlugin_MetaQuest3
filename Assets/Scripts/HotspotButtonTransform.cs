using UnityEngine;
using UnityEditor;

public class HotspotButtonTransform : MonoBehaviour
{
    public Vector3 position;
    public Quaternion anglesRotation;
    private void Start()
    {
        LoadTransform();
    }
    private void Update()
    {
        position = this.transform.localPosition;
        anglesRotation = this.transform.localRotation;
    }
    private void OnApplicationQuit()
    {
        SaveTransform();
    }
    void SaveTransform()
    {
        // Save the values in EditorPrefs for persistence
        PlayerPrefs.SetFloat(gameObject.name + "_positionX", position.x);
        PlayerPrefs.SetFloat(gameObject.name + "_positionY", position.y);
        PlayerPrefs.SetFloat(gameObject.name + "_positionZ", position.z);
       
        PlayerPrefs.SetFloat(gameObject.name + "_rotationX", anglesRotation.x);
        PlayerPrefs.SetFloat(gameObject.name + "_rotationY", anglesRotation.y);
        PlayerPrefs.SetFloat(gameObject.name + "_rotationZ", anglesRotation.z);
        PlayerPrefs.SetFloat(gameObject.name + "_rotationW", anglesRotation.w);
    }

    void LoadTransform()
    {
        // Load the saved transform values from EditorPrefs
        if (PlayerPrefs.HasKey(gameObject.name + "_positionX"))
        {
            float x = PlayerPrefs.GetFloat(gameObject.name + "_positionX");
            float y = PlayerPrefs.GetFloat(gameObject.name + "_positionY");
            float z = PlayerPrefs.GetFloat(gameObject.name + "_positionZ");
            position = new Vector3(x, y, z);

            float rotX = PlayerPrefs.GetFloat(gameObject.name + "_rotationX");
            float rotY = PlayerPrefs.GetFloat(gameObject.name + "_rotationY");
            float rotZ = PlayerPrefs.GetFloat(gameObject.name + "_rotationZ");
            float rotW = PlayerPrefs.GetFloat(gameObject.name + "_rotationW");
            anglesRotation = new Quaternion(rotX, rotY, rotZ, rotW);

            // Apply the loaded values to the transform
            transform.localPosition = position;
            transform.localRotation = anglesRotation;
        }
    }
}
