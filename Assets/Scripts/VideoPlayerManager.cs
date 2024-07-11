using UnityEngine;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class VideoPlayerManager : MonoBehaviour
{
    public GameObject hotspotButtonPrefab, hotspotButtonENV,hotspotObjectPrefab, mainEnvironment, innerSphere, hotspotSphere, mediaCanvas, continueButton;
    public string hotspotLabel;
    public MediaPlayer videoPlayer;
    public ApplyToMesh mainVideoPlayer;
    public int count;
    public string videoName;
    public List<GameObject> hotspotENV = new List<GameObject>();
    public List<GameObject> hotspotButtons;
    private void OnEnable()
    {
        ResetScene();
        videoPlayer = SceneManager.Instance.myMediaPlayer;
        mainVideoPlayer.Player = videoPlayer;
        videoPlayer.OpenMedia(new MediaPath(videoName + ".mp4", MediaPathType.RelativeToStreamingAssetsFolder), autoPlay: true);
        SetInactive();
        //this line of code is used to load videos from local storage of meta
        //videoPlayer.OpenMedia(new MediaPath("/sdcard/Movies/" + videoName + ".mp4", MediaPathType.AbsolutePathOrURL), autoPlay: true);

    }
    public void InstantiateHotspotObjects()
    {
        GameObject refObj = Instantiate(hotspotObjectPrefab, this.gameObject.transform);
       
        hotspotENV.Add(refObj);
        //hotspotSphere.GetComponent<EnableDisable>().hotspotButtons.Add(refObj);
        GameObject btnObj = Instantiate(hotspotButtonPrefab, hotspotButtonENV.transform);
        hotspotButtons.Add(btnObj);
        hotspotSphere.GetComponent<EnableDisable>().hotspotButtons.Add(btnObj);
        btnObj.transform.position = new Vector3(btnObj.transform.position.x, btnObj.transform.position.y + count, btnObj.transform.position.z);
        count += 70;
        HotspotButton btn = btnObj.GetComponent<HotspotButton>();
        btn.mainEnvironment = mainEnvironment;
        refObj.GetComponent<HotspotVideoPlayerManager>().mainOBJ = mainEnvironment;
        refObj.GetComponent<HotspotVideoPlayerManager>().StartSceneConfigurations();
        btn.buttonVideoObject = refObj;
        btn.myLabelText.text = hotspotLabel;
        btnObj.name = hotspotLabel + " 'Type: " + refObj.GetComponent<HotspotVideoPlayerManager>().hotspotType + "' Button";
        refObj.name = hotspotLabel +" 'Type: "+ refObj.GetComponent<HotspotVideoPlayerManager>().hotspotType+"' hotspot";
    }
    public void SetActive()
    {
        mediaCanvas.SetActive(false);
        continueButton.SetActive(true);
        foreach (GameObject g in hotspotButtons)
        {
            g.SetActive(true);
        }
    }
    public void SetInactive()
    {
        mediaCanvas.SetActive(true);
        continueButton.SetActive(false);
        foreach (GameObject g in hotspotButtons)
        {
            g.SetActive(false);
        }
    }
    private void ResetScene()
    {
        mainEnvironment.SetActive(true);
        innerSphere.SetActive(true);
        foreach (GameObject g in hotspotENV)
        {
            g.SetActive(false);
        }
    } 
}
