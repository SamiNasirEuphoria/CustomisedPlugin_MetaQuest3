using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotspotButton : MonoBehaviour
{
    public GameObject buttonVideoObject;
    [HideInInspector]

    public GameObject mainEnvironment;
    public TMP_Text myLabelText;
    public Button myButton;
    private Image parentImage;
    public Image fillImage;
    public float fillSpeed = 0.5f;
    public float decreaseSpeed = 0.2f;

    private void Start()
    {
        parentImage = myButton.image;
        //these lines of code used to disappear buttons on start of scene
        //myLabelText.gameObject.SetActive(false);
        //parentImage.color = new Color(parentImage.color.a, parentImage.color.g, parentImage.color.b, 0);
    }
    public void OpenVideoObject()
    {
        buttonVideoObject.SetActive(true);
        SceneManager.Instance.myMediaPlayer.Pause();
        mainEnvironment.SetActive(false);
    }
    public void FadeInColor()
    {
        parentImage.color = new Color(parentImage.color.a, parentImage.color.g, parentImage.color.b, 1);
    }
    public void FadeOutColor()
    {
        parentImage.color = new Color(parentImage.color.a, parentImage.color.g, parentImage.color.b, 0);
    }
}
