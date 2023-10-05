using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsExt : PlayerPrefs
{
    public static void SetDictionary(string key, Dictionary<string, string> dictionary)
    {
        // Serialize the dictionary to JSON
        string json = JsonUtility.ToJson(new SerializableDictionary(dictionary));

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString(key, json);
    }
    public static Dictionary<string, string> GetDictionary(string key, Dictionary<string, string> defaultValue = null)
    {
        // Load the JSON string from PlayerPrefs
        string json = PlayerPrefs.GetString(key, "{}");

        // Deserialize the JSON string to a dictionary
        SerializableDictionary serializableDict = JsonUtility.FromJson<SerializableDictionary>(json);
        Dictionary<string, string> dictionary = serializableDict.ToDictionary();
        return dictionary;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        return GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }

}

[System.Serializable]
public class SerializableDictionary
{
    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<string> values = new List<string>();

    public SerializableDictionary(Dictionary<string, string> dict)
    {
        keys = new List<string>();
        values = new List<string>();
        foreach (KeyValuePair<string, string> kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict.Add(keys[i], values[i]);
        }
        return dict;
    }
}