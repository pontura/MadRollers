using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatasManager : MonoBehaviour
{
    CharactersManager charactersManager;

    void Start()
    {
        charactersManager = Game.Instance.level.charactersManager;
        Invoke("CheckToAdd", 5);
    }
    void CheckToAdd()
    {
        if(Game.Instance.state == Game.states.GAME_OVER)
            return;
        Invoke("CheckToAdd", 3);
        int missionID = Data.Instance.missions.MissionActiveID;

        if (missionID <= 2)
            return;

        int totalAutomatas = missionID - 3;
        if (totalAutomatas < 1)
            totalAutomatas = 1;
        if (totalAutomatas > 4)
            totalAutomatas = 4;
        
        List<int> charactersInSceneID = new List<int>(4);
        foreach (CharacterBehavior cb in Game.Instance.level.charactersManager.characters)
            charactersInSceneID.Add(cb.player.id);

        if (Random.Range(0, 15) < totalAutomatas + 1)
            return;
        int rand = Random.Range(1, totalAutomatas);
        foreach (int i in charactersInSceneID)
            if (i == rand)
                return;

        AddAutomata(rand);
        
    }
    void AddAutomata(int avatarID)
    {
        CharacterBehavior cb =  Game.Instance.level.charactersManager.AddAutomata(avatarID);
        if (cb == null)
            return;
        cb.gameObject.AddComponent<Automata>();
        cb.GetComponent<Automata>().Init(cb);
    }   
}
