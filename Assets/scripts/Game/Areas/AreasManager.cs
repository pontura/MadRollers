using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasManager : MonoBehaviour
{
    public List<TextAsset> all;
    Dictionary<string, AreaData> data;

    public void LoadData()
    {
        //print("AreasManager Start ");
        print("AreasManager Start " + all + " Count: " + all.Count);
        data = new Dictionary<string, AreaData>();
        // all = new List<TextAsset>();
        int i = all.Count;

        while (i>0)
        {
            TextAsset t  = all[i-1];
            if (t != null)
            {
               // print(t.name + " num: " + i);
                if (t.text.Length > 10)
                {
                    AreaData a = JsonUtility.FromJson<AreaData>(t.text);
                    if (a != null)
                        data.Add(t.name, a);
                }
            }
            i--;
        }
    }
    public void Init()
    {
        data = new Dictionary<string, AreaData>();
        all = new List<TextAsset>();

        Add("continue_Multiplayer");
        Add("start_Multiplayer");

    }
    public void Add(string areaName)
    {
        if (IsNameUsed(areaName))
            return;


        print("AreasManager Add " + areaName);

        TextAsset asset = Resources.Load("areas/" + areaName) as TextAsset;
        all.Add(asset);
        data.Add(areaName, JsonUtility.FromJson<AreaData>(asset.text));
    }
    public AreaData GetArea(string areaName)
    {
        int id = 0;
        if (data.ContainsKey(areaName))
            return data[areaName];
        Debug.LogError("No hay area: " + areaName + ", en MissionsManager > AreasManager");
        return null;
    }
    bool IsNameUsed(string areaName)
    {
        if (data.ContainsKey(areaName))
            return true;
         return false;
    }
}
