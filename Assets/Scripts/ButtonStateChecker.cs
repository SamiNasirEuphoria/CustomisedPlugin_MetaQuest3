using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStateChecker : MonoBehaviour
{
    private Image image;

    EventTrigger trigger;
    public bool isClicked,Hovered;

    public bool  noInteractable;
    public bool isColorTint;
    public Sprite Hover, Pressed,Normal;
    public Color HoverColor, PressedColor,NormalColor;
    //public Button button;
    // public Selectable AnySelectable;
    // private PropertyInfo _selectableStateInfo = null;

    private void Awake()
    {
       
        image = GetComponent<Image>();

        if (noInteractable)
        {
            image.sprite = Pressed;
        }
        else
        {
            trigger = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnClick(); });
            trigger.triggers.Add(entry);

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerEnter;
            entry1.callback.AddListener((eventData) => { OnEnter(); });
            trigger.triggers.Add(entry1);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((eventData) => { OnExit(); });
            trigger.triggers.Add(entry2);
        }



        // _selectableStateInfo = typeof(Selectable).GetProperty("currentSelectionState", BindingFlags.NonPublic | BindingFlags.Instance);
    }
    

    public void MakeInteractable()
    {
        trigger = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;

        noInteractable = false;
        image.sprite = Normal;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnClick(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((eventData) => { OnEnter(); });
        trigger.triggers.Add(entry1);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((eventData) => { OnExit(); });
        trigger.triggers.Add(entry2);
    }

    private void Update()
    {
        if (noInteractable)
        {
            image.sprite = Pressed;
            return;
        }

        if (isClicked==true)
        {
            Clicked();
        }
        else
        {
            if (Hovered==false)
            {
                if (isColorTint)
                {
                    image.color = NormalColor;
                }
                else
                {
                    image.sprite = Normal;
                }
                Hovered = true;
            }
        }
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
       // var sprite = Resources.Load<Sprite>("Sprites/sprite01");
    }
    public void Clicked()
    {
        if (isColorTint)
        {
            image.color = PressedColor;

        }
        else
        {
            image.sprite = Pressed;
        }

    }
    private void Start()
    {
        image.sprite = Normal;

    }
    public void OnEnter()
    {
        Debug.Log(image.name + "_Hover");

        if (isColorTint)
        {
            image.color = HoverColor;

        }
        else
        {
            image.sprite = Hover;

        }

        //image.color = new Color32(118, 118, 118, 255);


    }
    public void OnExit()
    {
        if (isColorTint)
        {
            image.color = NormalColor;
        }
        else
        {
            image.sprite = Normal;
        }

       
        //image.color = new Color32(71, 71, 71, 229);
    }
    public void OnClick()
    {
        Debug.Log(image.name + "_Pressed");
        if (isColorTint)
        {
            image.color = PressedColor;

        }
        else
        {
            image.sprite = Pressed;
        }
        //image.color = new Color32(200, 200, 200, 255);
        Invoke(nameof(SetNormalColor), 0.08f);
        Debug.Log("Clickeed");
    }
    private void SetNormalColor()
    {
        if (isColorTint)
        {
            image.color = NormalColor;
        }
        else
        {
            image.sprite = Normal;
        }
        // image.color = new Color32(71, 71, 71, 229);
    }

}
