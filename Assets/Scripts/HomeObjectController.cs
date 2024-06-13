using UnityEngine;

public class HomeObjectController : MonoBehaviour
{
    public GameObject innerSphere;
    private void OnEnable()
    {
        innerSphere.SetActive(true);
    }
}
