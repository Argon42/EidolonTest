using UnityEngine;

public class PlayerPrefsRepository : IRepository
{
    public void SaveText(string key, string value) => PlayerPrefs.SetString(key, value);

    public string LoadText(string key) => PlayerPrefs.GetString(key);
}