using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]

public class ProjectManagerData
{
    public static string videoName;
    public static string hotspotType;
    public static Texture2D hotspotImage;
    public static string hotspotVideoName;
    public static string hotspotText;
}
public class ProjectManager : MonoBehaviour
{
    private static ProjectManager instance;
    public static ProjectManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }
    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }
}
