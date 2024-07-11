using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotspotButtonHower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image fillImage;
    public float fillSpeed = 0.5f;
    public float decreaseSpeed = 0.2f;
    private float fillAmount;
    private Coroutine fillCoroutine;
    public HotspotButton button;

    public void Testing()
    {
        if (fillCoroutine == null)
        {
            fillCoroutine = StartCoroutine(FillImageCoroutine());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fillCoroutine == null)
        {
            fillCoroutine = StartCoroutine(FillImageCoroutine());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(DecreaseFillAmountCoroutine());
        }
    }
    IEnumerator FillImageCoroutine()
    {
        fillAmount = 0f;
        while (fillAmount < 1f)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
            yield return null;
        }
        button.OpenVideoObject();
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
