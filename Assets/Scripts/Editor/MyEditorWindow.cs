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
    public Texture2D lastFrame360Image;
}
// HotspotData class to hold hotspot-related data
[System.Serializable]
public class HotspotData
{
    public HotspotType type;
    public string hotspotName;
    public Texture2D imageAsset;
    public string imageDescription;
    public VideoClip videoAsset;
    public string textAsset;
    public GameObject assetModel;
    public string modelDescription;
}

// Enum for hotspot types
public enum HotspotType
{
    Picture,
    Video,
    Text,
    _3DModel,
}
public class MyEditorWindow : EditorWindow
{
    private string projectName = "";
    private string companyName = "";
    private string packageName = "";
    private string videoName = "";
    private string errorMessage;
    private Texture2D icon;
    private Texture2D homeImage;
    private AudioClip bgMusic;
    private Texture2D duplicatedTexture;
    private Cubemap cubemap;
    private int numberOfScenes = 0;
    private List<SceneData> sceneDataList = new List<SceneData>();
    private Vector2 scrollPosition;
    GameObject contentObject;
    private List<bool> hasErrors= new List<bool>(); 
    //new method to apply texture to 360 imagew

    [MenuItem("VR Plugin/Open My Editor Window")]
    public static void ShowWindow()
    {
        // Open the custom Editor window
        GetWindow<MyEditorWindow>("My Editor Window");
    }
    private void OnGUI()
    {
        hasErrors.Clear();
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

        GUILayout.Space(5);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Home Screen Settings", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //EditorGUILayout.EndVertical();
        GUILayout.Space(5);
        homeImage = (Texture2D)EditorGUILayout.ObjectField("360 Image", homeImage, typeof(Texture2D), false);
        //asset =
        bgMusic = (AudioClip)EditorGUILayout.ObjectField("Background Music", bgMusic, typeof(AudioClip), false);

        //through errors on invalid videos
        if (bgMusic != null)
        {
            string path = AssetDatabase.GetAssetPath(bgMusic);
            if (!(path.EndsWith(".mp3", System.StringComparison.OrdinalIgnoreCase)||
                path.EndsWith(".wav", System.StringComparison.OrdinalIgnoreCase)||
                path.EndsWith(".aiff", System.StringComparison.OrdinalIgnoreCase)||
                path.EndsWith(".ogg", System.StringComparison.OrdinalIgnoreCase)||
                path.EndsWith(".flac", System.StringComparison.OrdinalIgnoreCase)))
            {
                errorMessage = "Error: Only mp3,wav,aiff,ogg,flac audios are supported.";
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                hasErrors.Add(true);
            }
            else
            {
                hasErrors.Add(false);
            }
        }
        GUILayout.Label("*This field is optional", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        GUILayout.Space(5);
       
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Scene Settings", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
       
        
        numberOfScenes = EditorGUILayout.IntField("Number of Scenes:", numberOfScenes);

        numberOfScenes = Mathf.Clamp(numberOfScenes,0,8);
        if (numberOfScenes < 0)
        {
            numberOfScenes = 0;
        }

        // Check if the scene data list needs to be resized
        if (sceneDataList.Count != numberOfScenes)
        {
            ResizeSceneDataList();
        }
        GUILayout.Label("[Maximum number of Scenes allowed is 8]", EditorStyles.boldLabel);



        // Display scene data fields
        for (int i = 0; i < numberOfScenes; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Scene " + (i + 1), EditorStyles.boldLabel);

            //to place video file name automatically
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("360 Video File Name", GUILayout.Width(EditorGUIUtility.labelWidth));

            if (string.IsNullOrEmpty(projectName))
            {
                videoName = "Wyoming_Scene" + (i+1)+".mp4";
            }
            else
            {
                videoName = projectName.Replace(" ", "")+ "_Scene" +(i+1)+".mp4";
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(videoName);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            SceneData sceneData = sceneDataList[i];
           
            sceneData.tagline = EditorGUILayout.TextField("Video Label:", sceneData.tagline);
            if (sceneData.tagline != null)
            {
                if (sceneData.tagline.Length > 25)
                {
                    errorMessage = "Only 25 characters are allowed.";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    hasErrors.Add(true);
                }
                else
                {
                    hasErrors.Add(false);
                }
            }
            GUILayout.Label("[Max length is upto 25 characters]", EditorStyles.boldLabel);
            
            //sceneData.tagline = sceneData.tagline.Substring(0,25);
            sceneData.description = EditorGUILayout.TextField("Video Description:", sceneData.description);
            if (sceneData.description != null)
            {
                if (sceneData.description.Length > 40)
                {
                    errorMessage = "Only 40 characters are allowed.";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    hasErrors.Add(true);
                }
                else
                {
                    hasErrors.Add(false);
                }
            }
            GUILayout.Label("[Max length is upto 40 characters]", EditorStyles.boldLabel);
            
            GUILayout.Space(5);
            sceneData.video = (VideoClip)EditorGUILayout.ObjectField("360 Video:", sceneData.video, typeof(VideoClip), false);
            //through errors on invalid videos
            if (sceneData.video != null)
            { 
                string path = AssetDatabase.GetAssetPath(sceneData.video);
                if (!path.EndsWith(".mp4", System.StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = "Error: Only .mp4 360 videos are supported.";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    hasErrors.Add(true);
                }
                else
                {
                    hasErrors.Add(false);
                }
            }
           
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Thumbnail Image:");
            EditorGUILayout.Space();
          
            sceneData.thumbnail = (Texture2D)EditorGUILayout.ObjectField("", sceneData.thumbnail, typeof(Texture2D), false);
           
            GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("[Thumbnail Image must be 16:9 ratio]", EditorStyles.boldLabel);
            GUILayout.EndVertical();    
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Hotspot Settings ", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            if (sceneData.hotspots.Count >0)
            {
                GUILayout.Space(5);
                sceneData.lastFrame360Image = (Texture2D)EditorGUILayout.ObjectField("Last frame 360 Image", sceneData.lastFrame360Image, typeof(Texture2D), false);

            }

            //GUILayout.Label("Hotspot Settings ", EditorStyles.boldLabel);
            GUILayout.Label("[Maximum number of hotspots allowed is 10]", EditorStyles.boldLabel);
            for (int j = 0; j < sceneData.hotspots.Count; j++)
            {
                HotspotData hotspotData = sceneData.hotspots[j];

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
               
                hotspotData.hotspotName = EditorGUILayout.TextField("Hotspot Label:", hotspotData.hotspotName);
                if (hotspotData.hotspotName != null)
                {
                    if (hotspotData.hotspotName.Length > 25)
                    {
                        errorMessage = "Only 25 characters are allowed.";
                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                        hasErrors.Add(true);
                    }
                    else
                    {
                        hasErrors.Add(false);
                    }
                }
                GUILayout.Label("[Max length is upto 25 characters]", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Type:", GUILayout.Width(60));
                hotspotData.type = (HotspotType)EditorGUILayout.EnumPopup(hotspotData.type, GUILayout.Width(80));

                //GUILayout.Label("Asset:", GUILayout.Width(50));
                switch (hotspotData.type)
                {
                    case HotspotType.Picture:
                        GUILayout.Label("Asset:", GUILayout.Width(50));
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        hotspotData.imageAsset = (Texture2D)EditorGUILayout.ObjectField(hotspotData.imageAsset, typeof(Texture2D), false);
                        EditorGUILayout.EndVertical();
                        GUILayout.Label("[Picture must be 16:9 ratio]", EditorStyles.boldLabel);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("Picture description:", GUILayout.Width(110));
                        // Adjust the height as needed
                        hotspotData.imageDescription = EditorGUILayout.TextField(hotspotData.imageDescription, GUILayout.Height(20));
                        
                        //new line added for testing
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            sceneData.hotspots.RemoveAt(j);
                        }

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginVertical();
                        if (hotspotData.imageDescription != null)
                        {
                            if (hotspotData.imageDescription.Length > 300)
                            {
                                errorMessage = "Only 300 characters are allowed.";
                                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                                hasErrors.Add(true);
                            }
                            else
                            {
                                hasErrors.Add(false);
                            }
                        }
                        GUILayout.Label("[Max characters upto 300]", EditorStyles.boldLabel);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndVertical();

                        break;
                    case HotspotType.Video:
                        GUILayout.Label("Asset:", GUILayout.Width(50));
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        hotspotData.videoAsset = (VideoClip)EditorGUILayout.ObjectField(hotspotData.videoAsset, typeof(VideoClip), false);
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            sceneData.hotspots.RemoveAt(j);
                        }

                        EditorGUILayout.EndVertical();
                        GUILayout.Label("[Video must be 16:9 ratio and .mp4 format]", EditorStyles.boldLabel);
                        if (hotspotData.videoAsset != null)
                        {
                            string path = AssetDatabase.GetAssetPath(hotspotData.videoAsset);
                            if (!path.EndsWith(".mp4", System.StringComparison.OrdinalIgnoreCase))
                            {
                                errorMessage = "Error: Only .mp4 videos are supported.";
                                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                                hasErrors.Add(true);
                            }
                            else
                            {
                                hasErrors.Add(false);
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        break;
                    case HotspotType.Text:

                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Text:", GUILayout.Width(50));
                        hotspotData.textAsset = EditorGUILayout.TextField(hotspotData.textAsset);
                        
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            sceneData.hotspots.RemoveAt(j);
                        }

                        EditorGUILayout.EndHorizontal();
                        if (hotspotData.textAsset != null)
                        {
                            if (hotspotData.textAsset.Length > 800)
                            {
                                errorMessage = "Only 800 characters are allowed.";
                                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                                hasErrors.Add(true);
                            }
                            else
                            {
                                hasErrors.Add(false);
                            }
                        }
                        GUILayout.Label("[Max characters upto 800]", EditorStyles.boldLabel);
                        EditorGUILayout.EndVertical();

                        break;
                    case HotspotType._3DModel:
                        GUILayout.Label("Asset:", GUILayout.Width(50));
                        hotspotData.assetModel = (GameObject)EditorGUILayout.ObjectField(hotspotData.assetModel, typeof(GameObject), false);
                        //throw error on invalid data entry
                        if (hotspotData.assetModel != null)
                        {
                            string path = AssetDatabase.GetAssetPath(hotspotData.assetModel);
                            if (!(path.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase)  ||
                                 path.EndsWith(".gltf", System.StringComparison.OrdinalIgnoreCase)  ||
                                 path.EndsWith(".glb", System.StringComparison.OrdinalIgnoreCase)   ||
                                 path.EndsWith(".obj", System.StringComparison.OrdinalIgnoreCase)   ||
                                 path.EndsWith(".blend", System.StringComparison.OrdinalIgnoreCase)
                                ))
                            {
                                errorMessage = "Error: Only FBX,glTF,OBJ and Blender type models are supported.";
                                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                                hasErrors.Add(true);
                            }
                            else
                            {
                                hasErrors.Add(false);
                            }
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Model description:", GUILayout.Width(110));
                        hotspotData.modelDescription = EditorGUILayout.TextField(hotspotData.modelDescription, GUILayout.Height(20)); // Adjust the height as needed
                        
                        //new line added for testing
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            sceneData.hotspots.RemoveAt(j);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginVertical();
                        if (hotspotData.modelDescription != null)
                        {
                            if (hotspotData.modelDescription.Length > 300)
                            {
                                errorMessage = "Only 300 characters are allowed.";
                                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                                hasErrors.Add(true);
                            }
                            else
                            {
                                hasErrors.Add(false);
                            }
                        }
                        GUILayout.Label("[Max characters upto 300]", EditorStyles.boldLabel);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndVertical();
                        
                        break;
                }

                EditorGUILayout.EndHorizontal();
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
        CheckForDuplicateTaglines();
        CheckForDuplicateHotspotNames();
        EditorGUILayout.EndVertical();

       
        if (GUILayout.Button("Apply"))
        {
           
            bool alltrue = true;
            
            foreach (bool check in hasErrors)
            {
                if (check)
                {
                    alltrue = false;
                    break;
                }
            }
            if (alltrue)
            {
                ApplySettings();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Remove all errors first.", "OK");
            }
        }
        EditorGUILayout.EndScrollView();



        //onGUI method is going to end here


    }
    private void CheckForDuplicateTaglines()
    {
        var taglines = new HashSet<string>();
        bool hasDuplicate = false;

        foreach (var scene in sceneDataList)
        {
            if (!string.IsNullOrEmpty(scene.tagline))
            {
                if (taglines.Contains(scene.tagline))
                {
                    errorMessage = $"Error: Duplicate tagline detected: {scene.tagline}";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    hasDuplicate = true;
                }
                else
                {
                    taglines.Add(scene.tagline);
                }
            }
        }

        if (hasDuplicate)
        {
            hasErrors.Add(true);
        }
        else
        {
            hasErrors.Add(false);
        }
    }
    private void CheckForDuplicateHotspotNames()
    {
        var hotspotNames = new HashSet<string>();
        bool hasDuplicate = false;

        foreach (var scene in sceneDataList)
        {
            foreach (var hotspot in scene.hotspots)
            {
                if (!string.IsNullOrEmpty(hotspot.hotspotName))
                {
                    if (hotspotNames.Contains(hotspot.hotspotName))
                    {
                        errorMessage = $"Error: Duplicate hotspot name detected: {hotspot.hotspotName}";
                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                        hasDuplicate = true;
                    }
                    else
                    {
                        hotspotNames.Add(hotspot.hotspotName);
                    }
                }
            }
        }

        if (hasDuplicate)
        {
            hasErrors.Add(true);
        }
        else
        {
            hasErrors.Add(false);
        }
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
            videoObjectData.sceneNumber = i;
            myButton.myLabelText.text = sceneDataList[i].tagline;
            myButton.myDescriptionText.text = sceneDataList[i].description;
            // now project shifted from passing data into buttons to gameobjects video
            videoObjectData.videoName = sceneDataList[i].video.name;

            //changing material approach
            videoObjectData.imageBackground360 = sceneDataList[i].lastFrame360Image;
            //Material hotspotMaterial = videoObjectData.hotspotSphere.GetComponent<MeshRenderer>().sharedMaterial;

            //hotspotMaterial.mainTexture = sceneDataList[i].lastFrame360Image;
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
            videoObjectData.hotspotCount = sceneDataList[i].hotspots.Count;
            for (int k = 0; k < sceneDataList[i].hotspots.Count; k++)
            {
                HotspotData hotspotData = sceneDataList[i].hotspots[k];
                Debug.Log("hence the total number of hotspots are" + sceneDataList[i].hotspots[k].hotspotName);
                obj.hotspotType = hotspotData.type.ToString();

                videoObjectData.hotspotLabel = sceneDataList[i].hotspots[k].hotspotName;

                if (sceneDataList[i].hotspots[k].hotspotName.Length > 15)
                {
                    videoObjectData.hotspotLabel = sceneDataList[i].hotspots[k].hotspotName.Substring(0, 15);
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
                    case HotspotType.Picture:
                        obj.hotspotSprite = sceneDataList[i].hotspots[k].imageAsset;
                        obj.imageDescription.text = sceneDataList[i].hotspots[k].imageDescription;
                        break;
                    case HotspotType.Video:
                        obj.hotspotVideoName = sceneDataList[i].hotspots[k].videoAsset.name;
                        break;
                    case HotspotType._3DModel:
                        obj.assetModel = sceneDataList[i].hotspots[k].assetModel;
                        obj.modelDescription.text = sceneDataList[i].hotspots[k].modelDescription;
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
        for (int i=0; i< numberOfScenes; i++)
        {
           PlayerPrefsHandler.SetPlayHotspot(i,1);
        }
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
        if (bgMusic != null)
        {
            homeObject.GetComponent<AudioSource>().clip = bgMusic;
        }
        else
        {
            homeObject.GetComponent<AudioSource>().clip = null;
        }
        //Material material = homeObject.GetComponent<MeshRenderer>().material;
        material.mainTexture = homeImage;
        // Spawn VideoButton prefabs
        //texture material to hotspot image, 360 image in the background
       
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
