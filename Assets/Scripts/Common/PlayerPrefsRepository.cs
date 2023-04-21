using UnityEngine;

namespace Eidolon.Common.Common
{
    public class PlayerPrefsRepository : IRepository
    {
        public string LoadText(string key) => PlayerPrefs.GetString(key);
        public void SaveText(string key, string value) => PlayerPrefs.SetString(key, value);
    }
}