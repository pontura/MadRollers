#if UNITY_EDITOR
    using Firebase;
    using Firebase.Auth;
    using Firebase.Extensions;
    using Firebase.Firestore;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;

public class ArcadeRanking : MonoBehaviour
{
    public string path;
    [Serializable]
    public class Hiscore
    {
        public string uid;
        public string username;
        public int hiscore;
    }
    public List<Hiscore> all;

#if UNITY_WEBGL && !UNITY_EDITOR  
   
    [DllImport("__Internal")]
    private static extern void SignInAnonymously();

    [DllImport("__Internal")]
    private static extern void SubmitScore(string userId, int score, string username);


    [DllImport("__Internal")]
    private static extern void GetHighScores();

    private string userId;

    void Start()
    {
        Data.Instance.events.RefreshHiscores += RefreshHiscores;
        // Intentar iniciar sesión anónimamente
        SignInAnonymously(); 
        GetHighScores();  // Llamar para obtener los puntajes más altos
    }
    private void OnDestroy()
    {
        Data.Instance.events.RefreshHiscores -= RefreshHiscores;
    }
    void RefreshHiscores()
	{
        GetHighScores();
    }
    public void Save(string username, int score)
    {
        // Asegúrate de que el userId se haya obtenido de la sesión de Firebase
        SubmitScore(userId, score, username);
    }

    // Este método debe ser llamado desde JavaScript para devolver el userId
    public void SetUserId(string id)
    {
        userId = id;
    }
    // Este método será llamado desde JavaScript para recibir los puntajes
    public void ReceiveHighScores(string highScoresJson)
    {
        // Parseamos el JSON recibido de JavaScript (deberás convertirlo desde el formato de JavaScript a C#)
        List<Hiscore> highScores = JsonUtility.FromJson<List<Hiscore>>(highScoresJson);

        foreach (var score in highScores)
        {
            Debug.Log("UID: " + score.uid + ", Score: " + score.hiscore + ", username: " + score.username);
        }
    }
#else


    FirebaseAuth auth;
    FirebaseFirestore db;
    FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;

            SignInAnonymously();
        });
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                user = task.Result.User;
                Debug.Log("Signed in anonymously as: " + user.UserId);
                GetTopScores(); // opcional
            }
        });
    }

    public void Save(string username, int score)
    {
        print("Save " + username + " score: " + score);

        DocumentReference docRef = db.Collection("leaderboard").Document();
        Dictionary<string, object> entry = new Dictionary<string, object>
        {
            { "uid", user.UserId },
            { "username", username },
            { "score", score },
            { "timestamp", Timestamp.GetCurrentTimestamp() }
        };

        docRef.SetAsync(entry).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Puntaje enviado!");
            }
            else
            {
                Debug.LogError("Error al enviar puntaje: " + task.Exception);
            }
        });
    }

    public void GetTopScores()
    {
        Query query = db.Collection("leaderboard")
                        .OrderByDescending("score")
                        .Limit(10);

        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                foreach (DocumentSnapshot doc in task.Result.Documents)
                {
                    Hiscore hiscore = new Hiscore();
                    hiscore.username = doc.GetValue<string>("username");
                    hiscore.hiscore = doc.GetValue<int>("score");
                    all.Add(hiscore);
                    Debug.Log("Score: " + doc.GetValue<int>("score"));
                }
            }
        });
    }
#endif
}



//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using System;
//using System.IO;
//using System.Linq;
//using UnityEngine.Networking;
//using LootLocker.Requests;

//public class ArcadeRanking : MonoBehaviour {

//	//public string path = "C:\\tumbagames\\hiscores\\MadRollers.txt";
//	public string path;
//	public List<Hiscore> all;

//	[Serializable]
//	public class Hiscore
//	{
//		public string username;
//		public int hiscore;       
//	}
//	//void Start () {
// //       if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
// //           return;
// //       Data.Instance.events.RefreshHiscores += RefreshHiscores;
//	//	path = Application.streamingAssetsPath + "/hiscores.txt";
// //       //LoadHiscores(path);
// //       //StartCoroutine(LoadHiscoresForWeb());
// //       GetTopScores();
// //   }
//    void RefreshHiscores()
//	{
//        //LoadHiscores (path);
//        // StartCoroutine(LoadHiscoresForWeb());
//        GetTopScores();
//    }
//    void Start()
//    {
//        StartCoroutine(StartSession());
//    }
//    IEnumerator StartSession()
//    {
//        bool done = false;

//        LootLockerSDKManager.StartGuestSession((response) =>
//        {
//            if (response.success)
//            {
//                Debug.Log("Session started");
//                GetTopScores();
//            }
//            else
//            {
//                Debug.Log("Session failed");
//            }
//            done = true;
//        });

//        yield return new WaitUntil(() => done);
//    }


//    string leaderboardKey = "partymode";
//    //
//    public void GetTopScores()
//    {


//        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
//        {
//            if (response.success)
//            {
//                foreach (var item in response.items)
//                {
//                   Debug.Log(item.member_id + " metadata: " +  item.metadata + " : " + item.score);
//                    Hiscore hiscore = new Hiscore();
//                    hiscore.username = item.metadata;
//                    hiscore.hiscore = item.score;
//                    all.Add(hiscore);
//                }
//            }
//            else
//            {
//                Debug.Log("Failed to get scores.");
//            }
//        });
//    }
//    public void Save(string username, int score)
//    {
//        Debug.Log("Save score");
//        LootLockerSDKManager.SubmitScore(username, score, leaderboardKey, username , (response) =>
//        {
//            if (response.success)
//            {
//                Debug.Log("Score submitted!");
//            }
//            else
//            {
//                Debug.Log("Failed to submit score.");
//            }
//        });
//    }





//    IEnumerator LoadHiscoresForWeb()
//    {
//        string path = Application.streamingAssetsPath + "/hiscores.txt";
//        UnityWebRequest www = UnityWebRequest.Get(path);
//        yield return www.SendWebRequest();

//        if (www.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError("Error al cargar el archivo: " + www.error);
//        }
//        else
//        {
//            string text = www.downloadHandler.text;
//            string[] lineas = text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
//            all.Clear();
//            foreach (string line in lineas)
//            {
//                string[] lines = line.Split("_"[0]);
//                Hiscore hiscore = new Hiscore();
//                hiscore.username = lines[0];
//                hiscore.hiscore = int.Parse(lines[1]);
//                all.Add(hiscore);
//            }
//        }
//    }
//    void LoadHiscores(string fileName)
//	{
//		String[] arrLines = File.ReadAllLines(fileName);
//		all.Clear ();
//		foreach (string line in arrLines)
//		{
//			string[] lines = line.Split("_"[0]);
//			Hiscore hiscore = new Hiscore();
//			hiscore.username = lines[0];
//			hiscore.hiscore = int.Parse(lines[1]);
//			all.Add(hiscore);
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
		//} 
	//}
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
//}
