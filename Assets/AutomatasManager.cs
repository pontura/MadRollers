﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatasManager : MonoBehaviour
{
    CharactersManager charactersManager;
    int startingInLevel = 8;

    void Start()
    {
        charactersManager = Game.Instance.level.charactersManager;
        Invoke("CheckToAdd", 8);
        if (Data.Instance.videogamesData.actualID == 2)
            startingInLevel = 3;
        if (Data.Instance.videogamesData.actualID == 3)
            startingInLevel = 1;
    }
    void CheckToAdd()
    {
        int timeToCheck = 3;
        if(Game.Instance.state == Game.states.GAME_OVER)
            return;
        
        int missionID = Data.Instance.missions.MissionActiveID;

        if (missionID <= startingInLevel)
            return;

        int totalAutomatas = missionID - (startingInLevel/3);
        if (totalAutomatas < 1)
            totalAutomatas = 1;
        if (totalAutomatas > 4)
            totalAutomatas = 4;
        
        List<int> charactersInSceneID = new List<int>(4);
        foreach (CharacterBehavior cb in Game.Instance.level.charactersManager.characters)
            charactersInSceneID.Add(cb.player.id);

        if (Random.Range(0, 15) < totalAutomatas + 1)
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

        
        Invoke("CheckToAdd", timeToCheck);
        
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