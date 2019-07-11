using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SummaryMultiplayer : MonoBehaviour {

    public GameObject title;
    public GameObject winnersTitle;

    public SummaryArcadePlayerUI player1;
    public SummaryArcadePlayerUI player2;
    public SummaryArcadePlayerUI player3;
    public SummaryArcadePlayerUI player4;

    private MultiplayerData multiplayerData;
	private int totalScore;

    public states state;
    public MeshRenderer rawimageRanking;
    public GameObject rankingScore;
  //  public Light lightInScene;

    public enum states
    {
        INTRO,
        READY,
        READY_RANKING,
        DONE
    }

    private IEnumerator nextRoutine;

	void Start () {
        //Data.Instance.GetComponent<MusicManager>().stopAllSounds();
//        int hiscore = Data.Instance.GetComponent<ArcadeRanking>().all[0].score;
//
//        rawimageRanking.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[0].texture;
//
//        foreach (Text field in winnersTitle.GetComponentsInChildren<Text>())
//            field.text = "1er PUESTO - " + Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition;
//
//        foreach ( Text field in rankingScore.GetComponentsInChildren<Text>())
//            field.text = "con " + hiscore + " PUNTOS.";
//
//        Time.timeScale = 1;
//        multiplayerData = Data.Instance.multiplayerData;
//
//        MultiplayerSummaryTexts texts = Data.Instance.GetComponent<MultiplayerSummaryTexts>();
//
//        for (int playerID = 0; playerID < 4; playerID++)
//            GetPosition(playerID);
//
//        int player_position_1 = multiplayerData.players[0];
//        int player_position_2 = multiplayerData.players[1];
//        int player_position_3 = multiplayerData.players[2];
//        int player_position_4 = multiplayerData.players[3];
//
//        int score1 = multiplayerData.GetScore(player_position_1);
//        int score2 = multiplayerData.GetScore(player_position_2);
//        int score3 = multiplayerData.GetScore(player_position_3);
//        int score4 = multiplayerData.GetScore(player_position_4);
//
//        string title_1 = texts.GetText(1, score1);
//        string title_2 = texts.GetText(2, score2);
//        string title_3 = texts.GetText(3, score3);
//        string title_4 = texts.GetText(4, score4);
//        totalScore = 1+ score1 + score2 + score3 + score4;
//        
//		if (totalScore>1000 && Data.Instance.GetComponent<ArcadeRanking>().CheckIfEnterHiscore(totalScore))
//        {
//            Data.Instance.GetComponent<ArcadeRanking>().newHiscore = totalScore;
//            SceneManager.LoadScene("NewHiscoreMultiplayer");
//            return;
//        }
//
//        foreach (Text field in title.GetComponentsInChildren<Text>())
//            field.text = "Puntos = " + totalScore; // + "x en " + (int)multiplayerData.distance  + " mts.";
//
//        player1.Init(multiplayerData.colors[player_position_1], title_1, score1, (score1 * 100) / totalScore, 1);
//        player2.Init(multiplayerData.colors[player_position_2], title_2, score2, (score2 * 100) / totalScore, 2);
//        player3.Init(multiplayerData.colors[player_position_3], title_3, score3, (score3 * 100) / totalScore, 3);
//        player4.Init(multiplayerData.colors[player_position_4], title_4, score4, (score4 * 100) / totalScore, 4);
//
//        nextRoutine = Next();
//        StartCoroutine(nextRoutine);
	}
    IEnumerator Next()
    {
        yield return new WaitForSeconds(3);   
        state = states.READY;
        yield return new WaitForSeconds(14);
        if(state == states.READY)
            ShowRanking();
    }
    int n = 0;
    void Update()
    {
        n++;
        if (n > 5)
        {
            //lightInScene.intensity = (float)Random.Range(70, 100) / 100;
            n = 0;
        }
                
		if (
            Data.Instance.inputManager.GetButtonDown(0, InputAction.action1) 
            || Data.Instance.inputManager.GetButtonDown(1, InputAction.action1)
            || Data.Instance.inputManager.GetButtonDown(2, InputAction.action1)
            || Data.Instance.inputManager.GetButtonDown(3, InputAction.action1)
            )
        {
            if (state == states.READY)
            {
                ShowRanking();
            }
            else if (state == states.READY_RANKING)
            {
                Ready();
            }
        }
    }
    void Reset()
    {
        StopCoroutine(nextRoutine);
    }
    void ShowRanking()
    {
        Reset();
        GetComponent<Animation>().Play("summaryMultiplayerRanking");
        Invoke("ReadyRanking", 2);
    }
    void ReadyRanking()
    {        
        state = states.READY_RANKING;
        Invoke("Ready", 2.5f);
    }
    void Ready()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
    //si no está registrado lo agrega a la lista:
    private int GetPosition(int _playerID)
    {
        int id = 1;
        foreach(int playerID in multiplayerData.players)
        {
            if (playerID == _playerID)
                return id;
        }
        multiplayerData.players.Add(_playerID);
        return multiplayerData.players.Count;
    }
	
}
