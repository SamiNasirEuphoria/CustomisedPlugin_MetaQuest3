using UnityEngine;

public class TextureApplier : MonoBehaviour
{
    public GameObject targetObject; // Reference to the GameObject with MeshRenderer
    public Texture2D textureToApply; // The Texture2D you want to apply

    public void ApplyTexture()
    {
        if (targetObject == null || textureToApply == null)
        {
            Debug.LogWarning("Target Object or Texture to Apply is null.");
            return;
        }

        MeshRenderer meshRenderer = targetObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.sharedMaterial;
            material.mainTexture = textureToApply;
        }
        else
        {
            Debug.LogWarning("Target object does not have a MeshRenderer component.");
        }
    }
}
