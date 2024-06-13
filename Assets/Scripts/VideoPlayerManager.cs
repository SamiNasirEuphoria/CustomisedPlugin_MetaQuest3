using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class VideoPlayerManager : MonoBehaviour
{
    public GameObject hotspotButtonPrefab, hotspotObjectPrefab, mainEnvironment, innerSphere;
    public string hotspotLabel;
    public MediaPlayer videoPlayer;
    public ApplyToMesh mainVideoPlayer;
    public int count;
    public string videoName;
    public List<GameObject> hotspotENV = new List<GameObject>();
    private void OnEnable()
    {
        ResetScene();
        videoPlayer = SceneManager.Instance.myMediaPlayer;
        mainVideoPlayer.Player = videoPlayer;
        videoPlayer.OpenMedia(new MediaPath(videoName + ".mp4", MediaPathType.RelativeToStreamingAssetsFolder), autoPlay: true);
    }
    public void InstantiateHotspotObjects()
    {
        GameObject refObj = Instantiate(hotspotObjectPrefab, this.gameObject.transform);
       
        hotspotENV.Add(refObj);
        //GameObject btnObj = Instantiate(hotspotButtonPrefab, contentObjectOfHotspot.transform);
        GameObject btnObj = Instantiate(hotspotButtonPrefab, mainEnvironment.transform);
       
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
