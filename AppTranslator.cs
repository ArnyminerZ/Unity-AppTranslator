using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script has been written by Arnyminer Z (https://github.com/ArnyminerZ/)
 * 
 * It is subject to GNU GPLv3 license, and distributed through Github at https://github.com/ArnyminerZ/Unity-AppTranslator
 */

[Serializable]
public class Translation
{
    public string key;
    public string value;

    public Translation(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
}

[Serializable]
public class Language
{
    public SystemLanguage language;
    public List<Translation> translations;

    public Language(SystemLanguage language, List<Translation> translations)
    {
        this.language = language;
        this.translations = translations;
    }
}

public class AppTranslator : MonoBehaviour
{
    [SerializeField] public List<Language> languages = new List<Language>();
    public SystemLanguage currentLanguage;
    public SystemLanguage fallbackLanguage;

    [SerializeField] private bool setCurrentLanguageToSystem = true;

    public Language GetCurrentLanguage()
    {
        foreach (var language in languages)
            if (language.language == currentLanguage)
                return language;

        foreach (var language in languages)
            if (language.language == fallbackLanguage)
                return language;

        return null;
    }

    public string GetTranslation(string findKey)
    {
        var language = GetCurrentLanguage();
        if (language == null) return null;

        foreach (var translation in language.translations)
        {
            if (translation.key.Equals(findKey, StringComparison.CurrentCultureIgnoreCase))
                return translation.value;
        }

        return null;
    }

    public void UpdateLanguage()
    {
        foreach (var text in FindObjectsOfType<Text>())
        {
            if (!text.text.StartsWith("/") || text.text.Length <= 0) continue;

            var translation = GetTranslation(text.text.Substring(1));
            if (translation != null)
                text.text = translation;
        }
    }

    private void Awake()
    {
        if (setCurrentLanguageToSystem)
            currentLanguage = Application.systemLanguage;
    }

    private void Start()
    {
        UpdateLanguage();
    }
}
