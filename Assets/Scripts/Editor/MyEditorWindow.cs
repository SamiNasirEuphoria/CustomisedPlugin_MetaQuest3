using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
// SceneData class to hold scene-related data
[System.Serializable]
public class SceneData
{
    public string tagline;
    public string description;
    public VideoClip video;
    public List<HotspotData> hotspots = new List<HotspotData>();
    public Texture2D thumbnail;
}
// HotspotData class to hold hotspot-related data
[System.Serializable]
public class HotspotData
{
    public HotspotType type;
    public string hotspotName;
    public Texture2D imageAsset;
    public VideoClip videoAsset;
    public string textAsset;
}

// Enum for hotspot types
public enum HotspotType
{
    Image,
    Video,
    //Video360,
    Text
}
public class MyEditorWindow : EditorWindow
{
    private string projectName = "";
    private string companyName = "";
    private string packageName = "";
    private Texture2D icon;
    private Texture2D homeImage;
    private Texture2D duplicatedTexture;
    private Cubemap cubemap;
    private int numberOfScenes = 0;
    private List<SceneData> sceneDataList = new List<SceneData>();
    private Vector2 scrollPosition;
    GameObject contentObject;
    //new method to apply texture to 360 imagew

    [MenuItem("VR Plugin/Open My Editor Window")]
    public static void ShowWindow()
    {
        // Open the custom Editor window
        GetWindow<MyEditorWindow>("My Editor Window");
    }
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Company Name", GUILayout.Width(EditorGUIUtility.labelWidth));
        companyName = EditorGUILayout.TextField(RemoveSpecialCharactersAndNumbers(companyName));
        EditorGUILayout.EndHorizontal();
        PlayerSettings.companyName = companyName;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Project Name", GUILayout.Width(EditorGUIUtility.labelWidth));
        projectName = EditorGUILayout.TextField(RemoveSpecialCharactersAndNumbers(projectName));
        EditorGUILayout.EndHorizontal();
        PlayerSettings.productName = projectName;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Package Name", GUILayout.Width(EditorGUIUtility.labelWidth));
       
        if (string.IsNullOrEmpty(companyName))
        {
            packageName = "com." +  projectName.Replace(" ", "").ToLower();
        }
        else if (string.IsNullOrEmpty(projectName))
        {
            packageName = "com." + companyName.Replace(" ", "").ToLower();
        }
        else
        {
            packageName = "com." + companyName.Replace(" ", "").ToLower() + "." + projectName.Replace(" ", "").ToLower();
        }


        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(packageName);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        
        icon = (Texture2D)EditorGUILayout.ObjectField("Icon", icon, typeof(Texture2D), false);


        homeImage = (Texture2D)EditorGUILayout.ObjectField("Background 360 Image", homeImage, typeof(Texture2D), false);

        GUILayout.Space(20);
        GUILayout.Label("(Add scenes upto max no. 10)", EditorStyles.boldLabel);
        numberOfScenes = EditorGUILayout.IntField("Number of Scenes:", numberOfScenes);

        numberOfScenes = Mathf.Clamp(numberOfScenes,0,10);
        if (numberOfScenes < 0)
        {
            numberOfScenes = 0;
        }

        // Check if the scene data list needs to be resized
        if (sceneDataList.Count != numberOfScenes)
        {
            ResizeSceneDataList();
        }

        // Display scene data fields
        for (int i = 0; i < numberOfScenes; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Scene " + (i + 1), EditorStyles.boldLabel);

            SceneData sceneData = sceneDataList[i];
            GUILayout.Label("[Max lenght is upto 25 characters]",EditorStyles.boldLabel );
            sceneData.tagline = EditorGUILayout.TextField("Video Label:", sceneData.tagline);
            GUILayout.Label("[Max lenght is upto 40 characters]", EditorStyles.boldLabel);
            sceneData.description = EditorGUILayout.TextField("Video Description:", sceneData.description);
            GUILayout.Space(5);
            sceneData.video = (VideoClip)EditorGUILayout.ObjectField("360 Video:", sceneData.video, typeof(VideoClip), false);
            sceneData.thumbnail = (Texture2D)EditorGUILayout.ObjectField("Video Thumbnail:", sceneData.thumbnail, typeof(Texture2D), false);
            //store the video names here
            //string name = sceneData.video.name;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Hotspots", EditorStyles.boldLabel);
            GUILayout.Label("[Max lenght is upto 10 characters]", EditorStyles.boldLabel);
            for (int j = 0; j < sceneData.hotspots.Count; j++)
            {
                HotspotData hotspotData = sceneData.hotspots[j];

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                hotspotData.hotspotName = EditorGUILayout.TextField("Hotspot Label:", hotspotData.hotspotName);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Type:", GUILayout.Width(60));
                hotspotData.type = (HotspotType)EditorGUILayout.EnumPopup(hotspotData.type, GUILayout.Width(80));
                
                GUILayout.Label("Asset:", GUILayout.Width(50));
                switch (hotspotData.type)
                {
                    case HotspotType.Image:
                        hotspotData.imageAsset = (Texture2D)EditorGUILayout.ObjectField(hotspotData.imageAsset, typeof(Texture2D), false);
                        break;
                    case HotspotType.Video:
                        hotspotData.videoAsset = (VideoClip)EditorGUILayout.ObjectField(hotspotData.videoAsset, typeof(VideoClip), false);
                        break;
                    //case HotspotType.Video360:
                    //    hotspotData.videoAsset = (VideoClip)EditorGUILayout.ObjectField(hotspotData.videoAsset, typeof(VideoClip), false);
                    //    break;
                    case HotspotType.Text:
                        hotspotData.textAsset = EditorGUILayout.TextField(hotspotData.textAsset);
                        break;
                }

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    sceneData.hotspots.RemoveAt(j);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Hotspot"))
            {
                if (sceneData.hotspots.Count < 10)
                {
                    sceneData.hotspots.Add(new HotspotData());
                }
              
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Apply"))
        {
            //ApplyTexture(textureApplier);
            ApplySettings();
        }
        
        EditorGUILayout.EndScrollView();
    }

   private void ResizeSceneDataList()
   {
       if (sceneDataList.Count < numberOfScenes)
       {
           while (sceneDataList.Count < numberOfScenes)
           {
               sceneDataList.Add(new SceneData());
           }
       }
       else
       {
           while (sceneDataList.Count > numberOfScenes)
           {
               sceneDataList.RemoveAt(sceneDataList.Count - 1);
           }
       }
   }


    private string RemoveSpecialCharactersAndNumbers(string input)
    {
        // Define the pattern to match any character that is not a letter
        string pattern = "[^a-zA-Z]";
        // Use Regex.Replace to remove all matches of the pattern
        return Regex.Replace(input, pattern, "");
    }
    private string RemoveSpecialCharAndAlphabets(string input)
    {
        string pattern = "^(10|[1-9])$";
        return Regex.Replace(input, pattern, "");
    }
    private void SpawnVideoButtons()
    {
        contentObject = GameObject.Find("Content");
        GameObject videoButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/VideoButtonNew.prefab");
        GameObject videoObjectHolder = GameObject.Find("VideoPlayerObject");
        GameObject videoObjectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/VideoPlayerObject.prefab");
        if (contentObject == null)
        {
            Debug.LogError("Content object not found.");
            return;
        }

        for (int i = 0; i < numberOfScenes; i++)
        {
            
            if (videoButtonPrefab == null)
            {
                Debug.LogError("VideoButton prefab not found.");
                return;
            }

            GameObject videoButtonInstance = Instantiate(videoButtonPrefab, contentObject.transform);
            videoButtonInstance.name = sceneDataList[i].tagline;
            GameObject videoObject = Instantiate(videoObjectPrefab, videoObjectHolder.transform);
            videoObject.name = sceneDataList[i].tagline + " scene";
            //new line of code added
            VideoButton myButton = videoButtonInstance.GetComponent<VideoButton>();
            myButton.buttonVideoObject = videoObject;

            Sprite sprite = Sprite.Create(sceneDataList[i].thumbnail, new Rect(0, 0, sceneDataList[i].thumbnail.width, sceneDataList[i].thumbnail.height), Vector2.one * 0.5f);

            myButton.buttonImage.sprite = sprite;

            VideoPlayerManager videoObjectData = videoObject.GetComponent<VideoPlayerManager>();
            myButton.myLabelText.text = sceneDataList[i].tagline;
            myButton.myDescriptionText.text = sceneDataList[i].description;
            // now project shifted from passing data into buttons to gameobjects video
            videoObjectData.videoName = sceneDataList[i].video.name;

            HotspotVideoPlayerManager obj = videoObjectData.hotspotObjectPrefab.GetComponent<HotspotVideoPlayerManager>();
            obj.videoName = sceneDataList[i].video.name;
            obj.sceneTagline = sceneDataList[i].tagline;
            if (obj.sceneTagline.Length > 10)
            {
                obj.sceneTagline = obj.sceneTagline.Substring(0, 10);
            }
            else
            {
                obj.sceneTagline = obj.sceneTagline;
            }
            
            obj.hotspotLenght = sceneDataList[i].hotspots.Count;
            

            videoObjectData.count = 0;
            for (int k = 0; k < sceneDataList[i].hotspots.Count; k++)
            {
                HotspotData hotspotData = sceneDataList[i].hotspots[k];
                Debug.Log("hence the total number of hotspots are" + sceneDataList[i].hotspots[k].hotspotName);
                obj.hotspotType = hotspotData.type.ToString();

                videoObjectData.hotspotLabel = sceneDataList[i].hotspots[k].hotspotName;

                if (sceneDataList[i].hotspots[k].hotspotName.Length > 10)
                {
                    videoObjectData.hotspotLabel = sceneDataList[i].hotspots[k].hotspotName.Substring(0, 10);
                }
                else
                {
                    videoObjectData.hotspotLabel = sceneDataList[i].hotspots[k].hotspotName;
                }
                switch (hotspotData.type)
                {
                    case HotspotType.Text:
                        obj.hotspotText = sceneDataList[i].hotspots[k].textAsset;
                        break;
                    case HotspotType.Image:
                        obj.hotspotSprite = sceneDataList[i].hotspots[k].imageAsset;
                        break;
                    case HotspotType.Video:
                        obj.hotspotVideoName = sceneDataList[i].hotspots[k].videoAsset.name;
                        break;
                }
                videoObjectData.InstantiateHotspotObjects();
            }
            if (videoButtonInstance != null)
            {
               
                    // Set thumbnail image
                    if (i < sceneDataList.Count)
                    {
                        SceneData sceneData = sceneDataList[i];
                        if (sceneData.thumbnail != null)
                        {
                            Image imageComponent = videoButtonInstance.GetComponent<Image>();
                            if (imageComponent != null)
                            {
                                imageComponent.sprite = Sprite.Create(sceneData.thumbnail, new Rect(0, 0, sceneData.thumbnail.width, sceneData.thumbnail.height), new Vector2(0.5f, 0.5f));
                            }
                        }
                    }
                    
            }
            else
            {
                Debug.LogError("Failed to instantiate VideoButton prefab.");
            }
            videoObject.SetActive(false);
        }
    }
    
    private void DestroyOldVideoButtons()
    {
        GameObject contentObject = GameObject.Find("Content");
        GameObject videoObject = GameObject.Find("VideoPlayerObject");
        if (contentObject != null)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>();

            // Add references to objects to destroy
            foreach (Transform child in contentObject.transform)
            {
                if (child.CompareTag("VideoButton"))
                {
                    objectsToDestroy.Add(child.gameObject);

                }
            }
            foreach (Transform child in videoObject.transform)
            {
                if (child.CompareTag("VideoObject"))
                {
                    objectsToDestroy.Add(child.gameObject);
                }
            }

            // Destroy the objects outside of the loop
            foreach (var obj in objectsToDestroy)
            {
                DestroyImmediate(obj);
            }
        }
    }


    private void ApplySettings()
    {
        PlayerSettings.applicationIdentifier = packageName;
        // Destroy existing VideoButton prefabs
        DestroyOldVideoButtons();
        if (icon != null)
        {
            // Set icon for Android platform
            Texture2D[] iconArray = new Texture2D[]
            {
                icon, icon, icon, icon, icon
            };
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, iconArray);
        }
        // Get the "Home" GameObject from the scene hierarchy
        GameObject homeObject = GameObject.Find("Home");
        Material material = homeObject.GetComponent<MeshRenderer>().sharedMaterial;
        //Material material = homeObject.GetComponent<MeshRenderer>().material;

        material.mainTexture = homeImage;
        // Spawn VideoButton prefabs
        SpawnVideoButtons();
        //add area to content size
        RectTransform myTransform = contentObject.GetComponent<RectTransform>();
        Vector2 sizeModified = myTransform.sizeDelta;
        sizeModified.x = 0;
        myTransform.sizeDelta = sizeModified;
        for (int i=0; i< numberOfScenes; i++)
        {
            if (i % 3 == 0)
            {
                sizeModified.x += 1920;
            }
        }
        myTransform.sizeDelta = sizeModified;
        // EditorApplication.isPlaying = true;
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
#endif
