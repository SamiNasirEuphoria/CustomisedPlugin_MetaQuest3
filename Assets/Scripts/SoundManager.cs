using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public AudioClip buttonSound, hoverSound;
    public AudioSource myAudio;
    private static SoundManager instance;
    public static SoundManager Instance
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
        else if (instance !=this)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void ButtonClick()
    {
        myAudio.clip = buttonSound;
        myAudio.volume = 0.45f;
        myAudio.Play();
    }
    public void ButtonHover()
    {
        myAudio.clip = hoverSound;
        myAudio.volume = 0.25f;
        myAudio.Play();
    }
}
