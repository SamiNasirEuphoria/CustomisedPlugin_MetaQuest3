using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(Button))]
public class VideoButton : MonoBehaviour
{
    public GameObject buttonVideoObject;
    public TMP_Text myLabelText;
    public TMP_Text myDescriptionText;
    private Button myButton;
    public Image buttonImage;
    private void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OpenVideoObject);
    }
    public void OpenVideoObject()
    {
        buttonVideoObject.SetActive(true);
        SceneManager.Instance.mainCanvasObject.SetActive(false);
    }
}
