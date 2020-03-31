using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatasManager : MonoBehaviour
{
    void Start()
    {        
        Invoke("CheckToAdd", 5);
    }
    void CheckToAdd()
    {
        Invoke("CheckToAdd", 3);
        int missionID = Data.Instance.missions.MissionActiveID;

        if (missionID <= 0)
            return;

        if (missionID > 4)
            missionID = 4;
        
        List<int> charactersInSceneID = new List<int>(4);
        foreach (CharacterBehavior cb in Game.Instance.level.charactersManager.characters)
            charactersInSceneID.Add(cb.player.id);
        if (Random.Range(0, 10) < missionID+1)
            return;
        int rand = Random.Range(1, missionID);
        foreach (int i in charactersInSceneID)
            if (i == rand)
                return;

        AddAutomata(rand);
        
    }
    void AddAutomata(int avatarID)
    {
        CharacterBehavior cb =  Game.Instance.level.charactersManager.AddAutomata(avatarID);
        cb.gameObject.AddComponent<Automata>();
        cb.GetComponent<Automata>().Init(cb);
    }   
}
