using UnityEngine;
using System.Collections; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonPopupAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private RectTransform buttonTransform;
    private bool check, howerButton;
    public bool isPlayPause, mainScreenButton;
    void Start()
    {
       
        button = GetComponent<Button>();
        button.interactable = true;
        buttonTransform = button.GetComponent<RectTransform>();
        button.onClick.AddListener(Clicked);
        if (mainScreenButton)
        {
            button.interactable = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mainScreenButton)
        {
            button.interactable = false;
            howerButton = true;
            StartCoroutine(WaitCoroutine());
        }else
        {
            buttonTransform.DOScale(Vector3.one * 1.15f, 0.75f);
        }
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       
        buttonTransform.DOScale(Vector3.one, 0.75f);
        if (mainScreenButton)
        {
            howerButton = false;
            button.interactable = false;
        }
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(0.55f);
        if (howerButton)
        {
            buttonTransform.DOScale(Vector3.one * 1.15f, 0.75f);
            button.interactable = true;
        }
       
    }
    public void Clicked()
    {
        SoundManager.Instance.ButtonClick();
        if (!isPlayPause)
        {
            buttonTransform.DOScale(Vector3.one * 1.25f, 0.75f);
            buttonTransform.DOScale(Vector3.one, 0.75f);
            // button.interactable = false;
            // StartCoroutine(Wait());
            check = true;
        }
        else if (howerButton)
        {
            howerButton = false;
            button.interactable = false;
        }
        else
        {
            buttonTransform.DOScale(Vector3.one * 1.25f, 0.75f);
            buttonTransform.DOScale(Vector3.one, 0.75f);
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        buttonTransform.DOScale(Vector3.one, 0.75f);
    }
}
