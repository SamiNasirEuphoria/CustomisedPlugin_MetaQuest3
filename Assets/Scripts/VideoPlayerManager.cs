using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class VideoPlayerManager : MonoBehaviour
{
    public GameObject hotspotButtonPrefab, hotspotButtonENV,hotspotObjectPrefab, mainEnvironment, innerSphere, hotspotSphere, mediaCanvas, continueButton;
    public string hotspotLabel;
    public MediaPlayer videoPlayer;
    public ApplyToMesh mainVideoPlayer;
    public int count, hotspotCount, sceneNumber;
    public string videoName;
    public List<GameObject> hotspotENV = new List<GameObject>();
    public List<GameObject> hotspotButtons;
    public Texture imageBackground360;
    private void OnEnable()
    {
        ResetScene();
        videoPlayer = SceneManager.Instance.myMediaPlayer;
        mainVideoPlayer.Player = videoPlayer;
        videoPlayer.OpenMedia(new MediaPath(videoName + ".mp4", MediaPathType.RelativeToStreamingAssetsFolder), autoPlay: true);
        SetInactive();
#if UNITY_EDITOR
        if (PlayerPrefsHandler.GetPlayHotspot(sceneNumber) == 1)
        {
            if (hotspotCount>0)
            {
                StartCoroutine(Wait());
            }
            PlayerPrefsHandler.SetPlayHotspot(sceneNumber, 0);
        }
#endif
        //this line of code is used to load videos from local storage of meta
        //videoPlayer.OpenMedia(new MediaPath("/sdcard/Movies/" + videoName + ".mp4", MediaPathType.AbsolutePathOrURL), autoPlay: true);

    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
        hotspotSphere.SetActive(true);
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
        //assign the material to hotspot sphere


        // Create a new material
        Material newMaterial = new Material(Shader.Find("Standard"));
        // Assign the texture to the material
        newMaterial.mainTexture = imageBackground360;
        // Apply the material to the GameObject's renderer
        hotspotSphere.GetComponent<MeshRenderer>().material = newMaterial;
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
