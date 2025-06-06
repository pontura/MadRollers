using System.Collections.Generic;
using UnityEngine;

public class AutomatasManager : MonoBehaviour
{
    CharactersManager charactersManager;
    int timeToCheck = 3;
    int totalAutomatas = 4;

#if UNITY_STANDALONE || UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            AddAutomata(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddAutomata(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddAutomata(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddAutomata(3);
    }
#endif

    void Start()
    {
        charactersManager = Game.Instance.level.charactersManager;
        Invoke("CheckToAdd", 8);
        //if (Data.Instance.videogamesData.actualID == 2)
        //    startingInLevel = 6;
        //if (Data.Instance.videogamesData.actualID == 3)
        //    startingInLevel = 3;
    }
    void CheckToAdd()
    {
        Invoke("CheckToAdd", timeToCheck);

        if (Game.Instance.state == Game.states.GAME_OVER)
            return;

        List<int> charactersInSceneID = new List<int>(4);

        foreach (CharacterBehavior cb in Game.Instance.level.charactersManager.characters)
            charactersInSceneID.Add(cb.player.id);

        if (Random.Range(0, 6) < totalAutomatas + 1)
        {
            bool characterExists = false;
            int rand = Random.Range(1, 4);
            foreach (int i in charactersInSceneID)
                if (i == rand)
                    characterExists = true;
            if (!characterExists)
            {
                AddAutomata(rand);
                timeToCheck += 3;
            }
        }

    }
    void AddAutomata(int avatarID)
    {
        Game.Instance.level.charactersManager.AddAutomata(avatarID);        
    }
}
