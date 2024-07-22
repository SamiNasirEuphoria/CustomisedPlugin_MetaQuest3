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


    private static string playHotspotKeyPrefix = "PlayHotspot_";

    // Method to set a value for a specific PlayHotspot index
    public static void SetPlayHotspot(int index, int value)
    {
        string key = playHotspotKeyPrefix + index;
        PlayerPrefs.SetInt(key, value);
    }

    // Method to get a value for a specific PlayHotspot index
    public static int GetPlayHotspot(int index)
    {
        string key = playHotspotKeyPrefix + index;
        return PlayerPrefs.GetInt(key, 0); // Default value is 0 if the key does not exist
    }

}
