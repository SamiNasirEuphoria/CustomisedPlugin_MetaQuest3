using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

using RenderHeads.Media.AVProVideo;

public class HotspotVideoPlayerManager : MonoBehaviour
{

    [Space(20)]
    public Text buttonTagline;
    public string sceneTagline;
    private Button button;
    [Header("[Scene Element]")]
    public string videoName;
    public ApplyToMesh mainVideoPlayer;
    public MediaPlayer videoPlayer;

    [Header("[Hotspot Element]")]
    public int hotspotLenght;
    //= new List<string>();
    public string hotspotType;
    public string hotspotVideoName;
    public string hotspotText;
    public Texture2D hotspotSprite;
    [Header("[Game Objects references]")]
    public GameObject hotspotImage;
    public GameObject videoPlayerObject;
    public GameObject textObject;
    public Animator myAnimator;
    //public Text hotspotLabelObject;
    [Space(2)]
    [Header("[Reference Container to hold data]")]
    public Image myImage;
    public DisplayUGUI myCanvesVideo;
    public MediaPlayer hotspotMediaPlayer;
    public TMP_Text myText;

    [Space(5)]
    [Header("Video Player Object Management Assets")]
    public GameObject mainOBJ;
    //public GameObject hotspotOBJ;
    public Button backFromHotspot; //hotspotButton, backtoMainButton,

    private void OnEnable()
    {
        backFromHotspot.onClick.AddListener(BackFromHotspot);
        hotspotMediaPlayer = SceneManager.Instance.hotspotMediaPlayer;
        myCanvesVideo.CurrentMediaPlayer = hotspotMediaPlayer;
        OpenHotspotPanel();
    }
    public void StartSceneConfigurations()
    {
        hotspotImage.SetActive(false);
        videoPlayerObject.SetActive(false);
        textObject.SetActive(false);
    }
    public void BackFromHotspot()
    {
        myAnimator.SetTrigger("FadeIn");
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        mainOBJ.SetActive(true);
        SceneManager.Instance.hotspotMediaPlayer.Stop();
        this.gameObject.SetActive(false);
        SceneManager.Instance.myMediaPlayer.Play();
    }
    public void OpenHotspotPanel()
    {
        this.gameObject.SetActive(true);
        switch (hotspotType)
        {
            case "Image":
                hotspotImage.SetActive(true);
                Sprite sprite = Sprite.Create(hotspotSprite, new Rect(0, 0, hotspotSprite.width, hotspotSprite.height), Vector2.one * 0.5f);
                myImage.sprite = sprite;
                break;

            case "Video":
                videoPlayerObject.SetActive(true);
                hotspotMediaPlayer.OpenMedia(new MediaPath(hotspotVideoName + ".mp4", MediaPathType.RelativeToStreamingAssetsFolder), autoPlay: true);
                break;

            case "Text":
                textObject.SetActive(true);
                myText.text = hotspotText;
                break;
        }
    }
}
