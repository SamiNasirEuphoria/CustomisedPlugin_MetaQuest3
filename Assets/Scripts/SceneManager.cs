using UnityEngine;
using UnityEngine.UI;

using RenderHeads.Media.AVProVideo;
public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance
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
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        mainCanvasObject.SetActive(true);
    }
    public GameObject mainCanvasObject;
    public MediaPlayer myMediaPlayer, hotspotMediaPlayer;
    
    
}
