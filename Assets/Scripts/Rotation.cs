using UnityEngine;

public enum RotationAxis
{
    x_Axis,
    y_Axis,
    z_Axis
}

public class Rotation : MonoBehaviour
{
    public RotationAxis rotationAxis = RotationAxis.y_Axis; // Corrected variable name
    public float speed = 50f;
    private Vector3 axis;
    private void FixedUpdate()
    {
        // Assign the correct axis based on the enum value
        switch (rotationAxis)
        {
            case RotationAxis.x_Axis:
                axis = Vector3.right;
                break;
            case RotationAxis.y_Axis:
                axis = Vector3.up;
                break;
            case RotationAxis.z_Axis:
                axis = Vector3.forward;
                break;
        }

        // Apply rotation using Time.fixedDeltaTime for FixedUpdate
        transform.Rotate(axis * speed * Time.fixedDeltaTime);
    }
}
