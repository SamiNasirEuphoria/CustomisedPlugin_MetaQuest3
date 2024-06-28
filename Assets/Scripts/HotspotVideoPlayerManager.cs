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
    public GameObject assetModel;
    [Header("[Game Objects references]")]
    public GameObject hotspotImage;
    public GameObject videoPlayerObject;
    public GameObject textObject;
    public GameObject modelReference;
    public GameObject positionReference;
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

    private void Start()
    {
        StartSceneConfigurations();
        backFromHotspot.onClick.AddListener(BackFromHotspot);
        hotspotMediaPlayer = SceneManager.Instance.hotspotMediaPlayer;
        myCanvesVideo.CurrentMediaPlayer = hotspotMediaPlayer;
        OpenHotspotPanel();
    }
    private void OnEnable()
    {
        if (hotspotType == "Video")
        {
            hotspotMediaPlayer.Rewind(true);
            hotspotMediaPlayer.Play();
        }
    }
    public void StartSceneConfigurations()
    {
        hotspotImage.SetActive(false);
        videoPlayerObject.SetActive(false);
        textObject.SetActive(false);
        positionReference.SetActive(false);
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
    public void Play()
    {
        hotspotMediaPlayer.Play();
    }
    public void Pause()
    {
        hotspotMediaPlayer.Pause();
    }
    public void Rewind()
    {
        hotspotMediaPlayer.Rewind(true);
        hotspotMediaPlayer.Play();
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
            case "GameObject":
                Debug.Log("You did it");
                positionReference.SetActive(true);
                GameObject sample = Instantiate(assetModel);
                sample.transform.position = modelReference.transform.position;
                //GameObject sample = Instantiate(assetModel);
                ResizeModelToReference(sample, modelReference.transform.localScale);
                CenterAndPivotAtPoint(sample, positionReference.transform.position);
                sample.AddComponent<Rotation>();
                sample.transform.SetParent(positionReference.transform);
                
                break;
        }
    }
    public Vector3 GetTotalBoundsSize(GameObject obj)
    {
        // Get all Renderer and SkinnedMeshRenderer components in the hierarchy
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        SkinnedMeshRenderer[] skinnedRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

        // Check if there are any renderers
        if (renderers.Length == 0 && skinnedRenderers.Length == 0)
        {
            Debug.LogWarning($"No Renderer or SkinnedMeshRenderer components found in {obj.name}.");
            return Vector3.zero;
        }

        // Combine the bounds of all renderers
        Bounds combinedBounds = new Bounds();
        bool hasBounds = false;

        foreach (Renderer renderer in renderers)
        {
            if (hasBounds)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
            else
            {
                combinedBounds = renderer.bounds;
                hasBounds = true;
            }
        }

        foreach (SkinnedMeshRenderer skinnedRenderer in skinnedRenderers)
        {
            if (hasBounds)
            {
                combinedBounds.Encapsulate(skinnedRenderer.bounds);
            }
            else
            {
                combinedBounds = skinnedRenderer.bounds;
                hasBounds = true;
            }
        }

        return combinedBounds.size;
    }
    public static GameObject CenterAndPivotAtPoint(GameObject targetObject, Vector3 pivotPoint)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not set!");
            return null;
        }

        // Get all Renderer and SkinnedMeshRenderer components in the hierarchy
        Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
        SkinnedMeshRenderer[] skinnedRenderers = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        // Check if there are any renderers
        if (renderers.Length == 0 && skinnedRenderers.Length == 0)
        {
            Debug.LogWarning($"No Renderer or SkinnedMeshRenderer components found in {targetObject.name}.");
            return null;
        }

        // Combine the bounds of all renderers to find the center
        Bounds combinedBounds = new Bounds();
        bool hasBounds = false;

        foreach (Renderer renderer in renderers)
        {
            if (hasBounds)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
            else
            {
                combinedBounds = renderer.bounds;
                hasBounds = true;
            }
        }

        foreach (SkinnedMeshRenderer skinnedRenderer in skinnedRenderers)
        {
            if (hasBounds)
            {
                combinedBounds.Encapsulate(skinnedRenderer.bounds);
            }
            else
            {
                combinedBounds = skinnedRenderer.bounds;
                hasBounds = true;
            }
        }

        // Calculate the center point of the combined bounds
        Vector3 center = combinedBounds.center;

        // Create a new parent GameObject at the desired pivot point
        GameObject parent = new GameObject(targetObject.name + "_CenteredPivot");
        parent.transform.position = pivotPoint;

        // Calculate the offset from the pivot point to the center of the original object
        Vector3 offset = center - pivotPoint;

        // Reparent the original object to the new parent
        targetObject.transform.SetParent(parent.transform);

        // Adjust the position of the original object to align the center with the pivot point
        targetObject.transform.localPosition = new Vector3(-offset.x, -offset.y, offset.z);

        return parent;
    }
    public void ResizeModelToReference(GameObject model, Vector3 referenceSize)
    {
        if (model == null)
        {
            Debug.LogError("Model is null!");
            return;
        }

        // Get the size of the model's bounding box
        // Vector3 modelSize = modelRenderer.bounds.size;
        Vector3 modelSize = GetTotalBoundsSize(model);

        if(modelSize == Vector3.zero)
        {
            Debug.Log("There's no renderer found");
        }
        // Calculate the scale factor to match the reference size
        Vector3 scaleFactor = new Vector3(
            referenceSize.x / modelSize.x,
            referenceSize.y / modelSize.y,
            referenceSize.z / modelSize.z
        );

        // Determine the overall scaling factor to maintain the model's proportions
        float uniformScaleFactor = Mathf.Min(scaleFactor.x, Mathf.Min(scaleFactor.y, scaleFactor.z));

        // Apply the uniform scaling factor to the model's transform
        model.transform.localScale = model.transform.localScale * uniformScaleFactor;
    }
}
