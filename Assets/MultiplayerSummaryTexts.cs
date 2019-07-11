using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class MultiplayerSummaryTexts : MonoBehaviour {

    int levelScore = 1000;

    public List<Condition> conditions;
    [Serializable]
    public class Condition
    {
        public int position;
        public int SCORE_TO_ACHIEVE;
        public List<string> puesto1;
        public List<string> puesto2;
        public List<string> puesto3;
        public List<string> puesto4;
    }
    
    string json_multiplayerResults_Url = "multiplayerResults";
    
    void Start()
    {
        Encoding utf8 = Encoding.UTF8;

        TextAsset file = Resources.Load(json_multiplayerResults_Url) as TextAsset;
        LoadDataromServer(file.text);
    }
    public void LoadDataromServer(string json_data)
    {
        var Json = SimpleJSON.JSON.Parse(json_data);
        
        for (int level = 0; level < 3; level++)
        {
            Condition condition = new Condition();
            condition.SCORE_TO_ACHIEVE = level * 2000;
            conditions.Add(condition);

            condition.puesto1 = new List<string>();
            condition.puesto2 = new List<string>();
            condition.puesto3 = new List<string>();
            condition.puesto4 = new List<string>();

            fillArray(condition.puesto1, Json["puesto1_" + level]);
            fillArray(condition.puesto2, Json["puesto2_" + level]);
            fillArray(condition.puesto3, Json["puesto3_" + level]);
            fillArray(condition.puesto4, Json["puesto4_" + level]);            
        }
    }
    private void fillArray(List<string> arr, JSONNode content)
    {
        for (int a = 0; a < content.Count; a++)
        {
            string text = content[a];
            arr.Add(text);
        }
    }
    public string GetText(int position, int score)
    {
        if (score == 0)
            return "";

        Condition condition = conditions[0];
        foreach(Condition _condition in conditions)
        {
            if (score > _condition.SCORE_TO_ACHIEVE)
               condition = _condition;
        }
        switch (position)
        {
            case 1:     return condition.puesto1[UnityEngine.Random.Range(0, condition.puesto1.Count)];
            case 2:     return condition.puesto2[UnityEngine.Random.Range(0, condition.puesto2.Count)];
            case 3:     return condition.puesto3[UnityEngine.Random.Range(0, condition.puesto3.Count)];
            default:    return condition.puesto4[UnityEngine.Random.Range(0, condition.puesto4.Count)];
        }
    }
}
