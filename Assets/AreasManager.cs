using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasManager : MonoBehaviour
{
    public List<TextAsset> all;
    public List<string> names;
    public void Init()
    {
        names = new List<string>();
        all = new List<TextAsset>();

        //agrega las 2 por default:
        Add("continue_Multiplayer");
        Add("start_Multiplayer");
    }
    public void Add(string areaName)
    {
        if (IsNameUsed(areaName))
            return;

        print("_________________Add Area" + areaName);

        names.Add(areaName);
        TextAsset asset = Resources.Load("areas/" + areaName) as TextAsset;
        all.Add(asset);
    }
    public TextAsset GetArea(string areaName)
    {
        int id = 0;
        foreach (string n in names)
        {
            if (n == areaName)
            {
                return all[id];
            }
            id++;
        }
        Debug.LogError("No hay area: " + areaName + ", en MissionsManager > AreasManager");
        return null;
    }
    bool IsNameUsed(string name)
    {
        foreach (string n in names)
            if (n == name)
                return true;
         return false;
    }
    int GetIdByName(string name)
    {
        int id = 0;
        foreach(string n in names)
        {
            if (n == name)
                return id;
            id++;
        }
        return id;
    }
}
