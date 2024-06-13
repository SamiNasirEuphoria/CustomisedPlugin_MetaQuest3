using UnityEngine;
using RenderHeads.Media.AVProVideo;
using BNG;
using System.Collections;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour
{
    public ApplyToMesh meshMedia;
    public MediaPlayer videoPlayer;
    public Animator fadeScreenAnimator;
    public UnityEngine.UI.Button exit, play,pause, rewind;
    public UnityEngine.UI.Slider videoSlider;
    public UnityEvent buttonResetState;
    private bool check;
    private void OnEnable()
    {
       videoPlayer = SceneManager.Instance.myMediaPlayer;
    }
    private void Start()
    {
        play.onClick.AddListener(Play);
        pause.onClick.AddListener(Pause);
        rewind.onClick.AddListener(Rewind);
        exit.onClick.AddListener(Exit);
        StartCoroutine(Wait());
    }
    public void ButtonsState(bool check)
    {
        videoSlider.interactable = check;
        exit.interactable = check;
        play.interactable = check;
        rewind.interactable = check;
        pause.interactable = check; 
    }
    IEnumerator ExitScreen()
    {
        if (check)
        {
            fadeScreenAnimator.SetTrigger("FadeIn");
            check = false;
        }
        yield return new WaitForSeconds(2.0f);
        buttonResetState.Invoke();
        SceneManager.Instance.myMediaPlayer.Stop();
        SceneManager.Instance.mainCanvasObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Exit()
    {
        check = true;
        StartCoroutine(ExitScreen());
    }
    public void Play()
    {
        videoPlayer.Play();
    }
    public void Pause()
    {
        videoPlayer.Pause();
    }
    public void Rewind()
    {
        videoPlayer.Rewind(true);
        videoPlayer.Play();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.25f);
        yield return new WaitForEndOfFrame();
    }
}
