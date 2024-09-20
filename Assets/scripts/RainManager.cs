using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RainManager : MonoBehaviour {

//    private CharactersManager charactersManager;
//    private bool isCompetition;
//    private float offset = 300;//400;
//    private float restaOffset = 30; //20;
//    private float min_offset = 20; //150;
//    private float distanceToAdd = 300; //700;
//
// 	  public void Init()
//    {
//		if (Data.Instance.playMode == Data.PlayModes.COMPETITION )
//        {
//            isCompetition = true;
//            charactersManager = GetComponent<CharactersManager>();
//        }
//    }
//    void Update()
//    {
//        if (!isCompetition) return;
//
//        if (charactersManager.distance > distanceToAdd)
//        {
//            int dificultLevel = 0;
//            switch (Game.Instance.level.Dificulty)
//            {
//                case Level.Dificult.HARD: dificultLevel = 120;  break;
//                case Level.Dificult.MEDIUM: dificultLevel = 40; break;
//            }
//            distanceToAdd = charactersManager.distance + (offset * 2) - dificultLevel;  
//            offset -= restaOffset;
//            if (offset < min_offset) offset = min_offset;
//			Data.Instance.events.OnBossDropBomb ();
//            if(Random.Range(0,10)<5)
//                AddSceneObject(new Vector3(0, 0, charactersManager.distance + 100), "Bomb1_real");
//         //   else
//             //   AddSceneObject(new Vector3(0, 0, charactersManager.distance + 100), "BombTeledirigida_real");
//        }
//    }
//    public void AddSceneObject(Vector3 position, string sceneObjectName)
//    {
//        Vector3 newPos = position;
//        newPos.x = Random.Range(-6, 6);
//        SceneObject obj = ObjectPool.instance.GetObjectForType(sceneObjectName, true);
//        if (obj)
//        {
//			Game.Instance.sceneObjectsManager.AddSceneObject(obj, newPos);
//            //obj.Restart(newPos);
//        }
//    }
//
}

