using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
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
    private float fillAmount;
    private Coroutine fillCoroutine;
    private void Start()
    {
        parentImage = myButton.image;
        //myButton.interactable = false;
        //myButton.onClick.AddListener(OpenVideoObject);



        myLabelText.gameObject.SetActive(false);
        parentImage.color = new Color(parentImage.color.a, parentImage.color.g, parentImage.color.b, 0);
    }
    public void OpenVideoObject()
    {
        buttonVideoObject.SetActive(true);
        SceneManager.Instance.myMediaPlayer.Pause();
        mainEnvironment.SetActive(false);
    }
    

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (fillCoroutine == null)
    //    {
    //        fillCoroutine = StartCoroutine(FillImageCoroutine());
    //    }
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (fillCoroutine != null)
    //    {
    //        StopCoroutine(fillCoroutine);
    //        fillCoroutine = StartCoroutine(DecreaseFillAmountCoroutine());
    //    }
    //}
    IEnumerator FillImageCoroutine()
    {
        fillAmount = 0f;
        while (fillAmount < 1f)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
            yield return null;
        }
        OpenVideoObject();
        //myButton.interactable = true;
        fillCoroutine = null;
        fillImage.fillAmount = 0f;
    }
    IEnumerator DecreaseFillAmountCoroutine()
    {
        fillAmount = fillImage.fillAmount;
        while (fillAmount > 0f)
        {
            fillAmount -= decreaseSpeed * Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
            yield return null;
        }
        fillCoroutine = null;
        fillImage.fillAmount = 0f;
    }
}
