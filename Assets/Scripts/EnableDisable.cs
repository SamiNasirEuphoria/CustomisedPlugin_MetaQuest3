using UnityEngine;
using System.Collections.Generic;

public class EnableDisable : MonoBehaviour
{
    public GameObject videoSphere, mediaCanvesObject;
    public List<GameObject> hotspotButtons;

    //methods have been updated
    //this code will convert hotspot buttons
    //into pink color and vise versa
    private void OnEnable()
    {
        this.gameObject.SetActive(true);
        videoSphere.SetActive(false);
        mediaCanvesObject.SetActive(false);
        SceneManager.Instance.myMediaPlayer.Pause();
        //foreach (GameObject g in hotspotButtons)
        //{
        //    g.GetComponent<HotspotButton>().FadeInColor();
        //}
        foreach (GameObject g in hotspotButtons)
        {
            g.SetActive(true);
        }
    }
    private void OnDisable()
    {
        this.gameObject.SetActive(false);
        videoSphere.SetActive(true);
        mediaCanvesObject.SetActive(true);
        SceneManager.Instance.myMediaPlayer.Play();
        //foreach (GameObject g in hotspotButtons)
        //{
        //    g.GetComponent<HotspotButton>().FadeOutColor();
        //}
        foreach (GameObject g in hotspotButtons)
        {
            g.SetActive(false);
        }
    }
}
