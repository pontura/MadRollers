using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Linq;

public class ArcadeRanking : MonoBehaviour {

	//public string path = "C:\\tumbagames\\hiscores\\MadRollers.txt";
	public string path;
	public List<Hiscore> all;

	[Serializable]
	public class Hiscore
	{
		public string username;
		public int hiscore;       
	}
	void Start () {
		Data.Instance.events.RefreshHiscores += RefreshHiscores;
		path = Application.streamingAssetsPath + "/hiscores.txt";
		LoadHiscores(path);
	}
	void RefreshHiscores()
	{
		LoadHiscores (path);
	}
	void LoadHiscores(string fileName)
	{
		String[] arrLines = File.ReadAllLines(fileName);
		all.Clear ();
		foreach (string line in arrLines)
		{
			string[] lines = line.Split("_"[0]);
			Hiscore hiscore = new Hiscore();
			hiscore.username = lines[0];
			hiscore.hiscore = int.Parse(lines[1]);
			all.Add(hiscore);
//
//			if (hiscore.hiscore < _hiscore && !yaAgrego)
//			{
//				yaAgrego = true;
//				puesto = num;
//				if (num < 16)
//				{
//					ScoreLine newScoreLine = Instantiate(scoreLineNewHiscore);
//					newScoreLine.Init(num, "XXXXX", _hiscore);
//					newScoreLine.transform.SetParent(container);
//					newScoreLine.transform.localScale = Vector3.one;
//					num++;
//				}                    
//			}
//
//			if(num<16)
//			{
//				ScoreLine newScoreLine = Instantiate(scoreLine);                
//				newScoreLine.Init(num, hiscore.username, hiscore.hiscore);               
//				newScoreLine.transform.SetParent(container);
//				newScoreLine.transform.localScale = Vector3.one;
//			}               

			//num++;
		} 
	}
//    public int newHiscore;
//    private int totalHiscores = 5;
//
//    [Serializable]
//    public class RankingData
//    {
//        public int score;
//        public Texture2D texture;
//    }
//	public List<RankingData> all;
//
//    public void OnAddHiscore(Texture2D texture,  int _hiscore)
//    {
//        RankingData data = new RankingData();
//        data.score = _hiscore;
//        data.texture = texture;
//        all.Add(data);
//        Reorder();
//    }
//    public bool CheckIfEnterHiscore(int score)
//    {
//        if (score>50 && all.Count < totalHiscores) return true;
//
//        if (score > all[totalHiscores-1].score)
//            return true;
//
//        return false;
//    }
//    void Start () {
//		Data.Instance.events.OnHiscore += OnHiscore;
//	}
//	void OnHiscore(Texture2D texture, int _hiscore)
//	{
//        RankingData data = new RankingData();
//        data.score = _hiscore;
//        data.texture = texture;
//        all.Add(data);
//        Reorder();
//        if (all.Count > totalHiscores)
//            all.Remove(all[all.Count - 1]);
//    }
//    void Reorder()
//    {
//        all = all.OrderBy(w => w.score).ToList();
//        all.Reverse();
//    }
}
