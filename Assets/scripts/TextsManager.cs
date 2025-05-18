using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.IO;

public class TextsManager : MonoBehaviour {

    public string lang = "es";
	public List<TextsData> all;
    public static TextsManager mInstance;

    public static TextsManager Instance
    {
        get
        {
            if (mInstance == null)
                Debug.LogError("Algo llama a TextsManager y no existe");
            return mInstance;
        }
    }
    void Awake()
    {
        if (!mInstance)  mInstance = this; DontDestroyOnLoad(this);
    }
    private void Start()
    {
        lang = PlayerPrefs.GetString("lang", "");
        if (lang != "")
            Debug.Log("Lang: " + lang);
        else if (Application.systemLanguage == SystemLanguage.Spanish)
            lang = "es";
        else
            lang = "en";
    }
    public void NextLang()
    {
        if (lang == "en")
            lang = "es";
        else
            lang = "en";
        SetLang(lang);
    }
    public void SetLang(string _lang)
    {
        this.lang = _lang;
        PlayerPrefs.SetString("lang", lang);
    }
    [Serializable]
	public class TextsData
	{
		public string en;
        public string es;
    }
    public string GetText(string originalText)
    {
        foreach(TextsData textData in all)
        {
            if (textData.en.ToLower() == originalText.ToLower())
                return GetTranslatedText(textData);
        }
        return originalText;
    }
    string GetTranslatedText(TextsData textData)
    {
        switch(lang)
        {
            case "en":  return textData.en;
            default:    return textData.es;
        }
    }
}
