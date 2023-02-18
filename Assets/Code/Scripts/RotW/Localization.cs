using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Localization {
    // Start is called before the first frame update
    private static Dictionary<string, string> local = new Dictionary<string, string>();
    [NonSerialized]
    public static string currentLanguage = null;

    public string Language;
    public string Token;

    public static string Localize(string token) {
        if (local.ContainsKey(token))
            return local[token];
        return token;
    }

    public static void LoadLocalization(string lang) {
        if (currentLanguage == lang)
            return;
        TextAsset asset = Resources.Load("Locale/" + lang, typeof(TextAsset)) as TextAsset;
        if (asset == null) {
            Debug.LogError("Can't find localization: " + lang + ".");
            return;
        }
        local.Clear();
        Localization loc = JsonUtility.FromJson<Localization>(asset.text);

        //local = JsonSerializer.Deserialize<Dictionary<string, string>>(loc.Token);
    }
}
