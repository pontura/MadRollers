using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresInGame : MonoBehaviour
{
    [SerializeField] ScoreInGame scoreInGame;
    List< ScoreInGame> all;
    [SerializeField] Transform container;
    Camera cam;

    void Start()
    {
        cam = Game.Instance.gameCamera.cam;
        Init();
        Data.Instance.events.OnScoreOn += OnScoreOn;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
    }
    private void Init()
    {
        all = new List<ScoreInGame>();
        for(int a = 0; a<15; a++)
        {
            ScoreInGame s = Instantiate(scoreInGame, container);
            s.SetActive(false);
            all.Add(s);
        }
    }
    private void OnScoreOn(int arg1, Vector3 pos, int score, ScoresManager.types arg4)
    {
        if (pos == Vector3.zero) return;
        ScoreInGame s = GetScoreAvailable();
        if(s != null)
        {
            s.SetActive(true, score);
            Vector3 _pos = Data.Instance.curvedWorldManager.curvedWorld_Controller.TransformPoint(pos, VacuumShaders.CurvedWorld.BEND_TYPE.ClassicRunner);
            Vector2 viewportPosition = cam.WorldToViewportPoint(_pos);
            viewportPosition.x *= Screen.width;
            viewportPosition.y *= Screen.height;
            s.transform.position = viewportPosition;
        }
    }
    ScoreInGame GetScoreAvailable()
    {
        foreach (ScoreInGame s in all)
        {
            if (!s.IsActive)
                return s;
        }
        return null;
    }
}
