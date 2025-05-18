using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class Hiscores : MonoBehaviour {

    public List<LevelHiscore> levels;

    [Serializable]
    public class LevelHiscore
    {
        [SerializeField]
        public List<Hiscore> hiscore;
        public int id;
        public int myScore;
    }

    [Serializable]
    public class Hiscore
    {
        [SerializeField]
        public int id;
        public string username;
        public string facebookID;
        public int score;
        public Texture2D profilePicture;
    }

    void Start()
    {
//        
//        SocialEvents.OnFinalDistance += OnFinalDistance;
//        SocialEvents.OnFacebookImageLoaded += OnFacebookImageLoaded;
//        SocialEvents.LoadMyHiscoreIfNotExistesInRanking += LoadMyHiscoreIfNotExistesInRanking;
//        loadLocalSavedScores();
    }
    public Texture2D GetPicture(string facebookID)
    {
		
        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (facebookID == hiscore.facebookID && hiscore.profilePicture)
                return hiscore.profilePicture;
        }
        return null;
    }
    void OnFacebookImageLoaded(string facebookID, Texture2D texture2d)
    {
		return;
        foreach(Hiscore hiscore in levels[0].hiscore)
        {
            if (facebookID == hiscore.facebookID)
                hiscore.profilePicture = texture2d;
        }
    }
    void loadLocalSavedScores()
    {
		return;
        for (int a = 1; a < levels.Count+1; a++)
        {
            int myScore = PlayerPrefs.GetInt("scoreLevel_" + a);
            levels[a-1].myScore = myScore;
        }
    }
    void OnFinalDistance(float score)
    {
		return;
      //  if (Data.Instance.playMode == Data.PlayModes.STORY) return;
//        int competitionID = Data.Instance.competitions.GetCurrentCompetition();
//        checkToSaveHiscore(competitionID, score);
    }
    void checkToSaveHiscore(int competitionID, float score)
    {
		return;
    }
    void OnHiscoresLoaded(string receivedData)
    {
		return;
    }
    void LoadMyHiscoreIfNotExistesInRanking(string facebookID, int score)
    {
		return;
    }
    public void ArrengeHiscoresByScore()
    {
		return;
    }
    public Hiscore GetMyNextGoal()
    {
        string facebookID = "";// = Data.Instance.userData.facebookId;

        if (levels[0].hiscore.Count == 0) return null;

        Hiscore lastHiscore = levels[0].hiscore[0];

        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (levels[0].myScore > hiscore.score || facebookID == hiscore.facebookID)
                return lastHiscore;
            lastHiscore = hiscore;
        }
        return lastHiscore;
    }
    //mientras corres le ganas a un contrincante y te graba tu score provisorio
    public void SetMyScoreWhenPlaying(int newScore)
    {
		return;
    }
    public int GetMyScore()
    {
        return levels[0].myScore;
    }
    public void Reset()
    {
        levels[0].myScore = 0;
    }
}
