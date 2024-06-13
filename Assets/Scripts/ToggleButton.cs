using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonPause;

    public Sprite normalSpritePlay;
    public Sprite normalSpritePause;
    public Sprite selectedSpritePlay;
    public Sprite selectedSpritePause;

    void Start()
    {
        StartScene();
    }
    void StartScene()
    {
        buttonPlay.onClick.AddListener(ToggleButton1);
        buttonPause.onClick.AddListener(ToggleButton2);
        buttonPlay.GetComponent<ButtonPopupAnimation>().enabled = true;
        buttonPause.GetComponent<ButtonPopupAnimation>().enabled = true;
        buttonPlay.image.sprite = selectedSpritePlay;
        buttonPause.image.sprite = normalSpritePause;
    }
    public void ToggleButton1()
    {
        buttonPlay.image.sprite = selectedSpritePlay;
        buttonPause.image.sprite = normalSpritePause;
    }

    void ToggleButton2()
    {
        buttonPlay.image.sprite = normalSpritePlay;
        buttonPause.image.sprite = selectedSpritePause;
    }
    public void ResetScene()
    {
        buttonPlay.image.sprite = normalSpritePlay;
        buttonPause.image.sprite = normalSpritePause;
        buttonPlay.GetComponent<ButtonPopupAnimation>().enabled = false;
        buttonPause.GetComponent<ButtonPopupAnimation>().enabled = false;
    }
}
