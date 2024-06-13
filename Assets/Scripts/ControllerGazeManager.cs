using UnityEngine;

public class ControllerGazeManager : MonoBehaviour
{
    void Update()
    {
        // Define the origin of the raycast
        Vector3 origin = this.transform.position;

        // Define the direction of the raycast (local X direction)
        Vector3 direction = this.transform.right;

        // Perform the raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("HotspotButton"))
            {
                Debug.Log("Hit: " + hitInfo.collider.name);
            }
        }
        // For debugging purposes, draw the ray in the Scene view
        Debug.DrawRay(origin, direction * 1000, Color.red);
    }
}
