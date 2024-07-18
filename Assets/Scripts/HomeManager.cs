using UnityEngine;

public class HomeManager : MonoBehaviour
{
    bool check;
    public delegate void HotspotButtons();
    public static HotspotButtons hotspotButton;
    
    private void OnEnable()
    {
        if (check)
        {
            hotspotButton?.Invoke();
            check = false;
        }
    }
    private void OnDisable()
    {
        check = true;   
    }

}
