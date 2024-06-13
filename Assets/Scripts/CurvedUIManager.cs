using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BNG;

public class CurvedUIManager : MonoBehaviour
{
    public GameObject canves, eventSystemObject;
    private void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        canves.GetComponent<GraphicRaycaster>().enabled = true;
        canves.GetComponent<MeshCollider>().enabled = false;
        eventSystemObject.GetComponent<VRUISystem>().enabled = true;
        eventSystemObject.GetComponent<CurvedUIInputModule>().enabled = false;
    }
}
