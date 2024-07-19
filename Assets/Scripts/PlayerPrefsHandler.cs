using UnityEngine;
public static class PlayerPrefsHandler
{
    public static string playHotpost = "PlayHotspot";
    public static int PlayHotspot
    {
        set
        {
            PlayerPrefs.SetInt(playHotpost, value);
        }
        get
        {
            return PlayerPrefs.GetInt(playHotpost);
        }
    } 
}
