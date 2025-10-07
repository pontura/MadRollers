using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
    public List<CharacterBehavior> characters;
    public List<CharacterBehavior> deadCharacters;

    CharacterBehavior mainCharacter;

    private float separationX  = 4.5f;

    public float distance;
	private float MAX_SPEED = 19;
    private float speedRun = 0;
	private float acceleration = 10;
    private Missions missions;
    public List<int> playerPositions;
	bool canStartPlayers;
    public int totalCharacters;
    bool isAndroid;

    void Awake()
    {
        isAndroid = Data.Instance.isAndroid;
        distance = 0;
        Events.OnAlignAllCharacters += OnAlignAllCharacters;
        Events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
        Events.OnAvatarCrash += OnAvatarCrash;
        Events.OnAvatarFall += OnAvatarFall;
        Events.StartMultiplayerRace += StartMultiplayerRace;
        Events.FreezeCharacters += FreezeCharacters;
    }
    public void Continue()
    {
        print("Continue");
        int i = characters.Count;
        while(i>0)
        {
            i--;
            CharacterBehavior ch = characters[i];
            ch.Die();
        }
        characters = new List<CharacterBehavior>();
        Vector3 pos = new (0, 5, distance);
        addCharacter(pos, 0);

    }
    public virtual void Init()
    {
        print("CharactersManager Init");
        missions = Data.Instance.GetComponent<Missions>();
        StartCoroutine(AddCharactersInitials());

        if (Data.Instance.missions.MissionActiveID != 0)
            gameObject.AddComponent<AutomatasManager>();
    }

    public bool freezed;
	void FreezeCharacters(bool _freezed)
	{
		foreach (CharacterBehavior cb in characters) {
			if (!cb.player.IsDebbugerPlayer ()) {
				cb.GetComponent<Rigidbody> ().useGravity = !_freezed;
			}
		}
		freezed = _freezed;
	}
    void StartMultiplayerRace()
    {
		canStartPlayers = true;
		if (Data.Instance.isReplay) {
			speedRun = MAX_SPEED;
		}
    }
	void Update()
    {
        if (freezed)
			return;

        if (Game.Instance.level.waitingToStart) return;
        if (Game.Instance.state == Game.states.GAME_OVER) return;

        OnUpdate();

        if (speedRun >= MAX_SPEED)
			speedRun = MAX_SPEED;
		else
			speedRun += acceleration * Time.deltaTime;

        distance += speedRun * Time.deltaTime;
		
    }
	public virtual void OnUpdate(){ }
   
    IEnumerator AddCharactersInitials()
    {
		Vector3 pos;
		float _y = 4;

		if (Data.Instance.isReplay)// || isAndroid)
			_y = 30;
		else
			canStartPlayers = true;

		pos = new Vector3(0, _y, 0);

		int positionID = 0;

		totalCharacters = Data.Instance.multiplayerData.GetTotalCharacters ();

		if (totalCharacters == 0)
			yield return null;
		
		if (Data.Instance.multiplayerData.player1) { addCharacter(CalculateInitialPosition(pos, positionID), 0); playerPositions.Add(0); };
        //if (Data.Instance.multiplayerData.player2) { addCharacter(CalculateInitialPosition(pos, positionID+1), 1); playerPositions.Add(1); };
        //if (Data.Instance.multiplayerData.player3) { addCharacter(CalculateInitialPosition(pos, positionID+2), 2); playerPositions.Add(2); };
        //if (Data.Instance.multiplayerData.player4) { addCharacter(CalculateInitialPosition(pos, positionID+3), 3); playerPositions.Add(3); };

        if (Data.Instance.missions.MissionActiveID != 0)
            Add3Automatas(pos);
        yield return null;
	}
    void Add3Automatas(Vector3 pos)
    {
        for (int a = 0; a < 3; a++)
        {
            CharacterBehavior cb = addCharacter(CalculateInitialPosition(pos, a + 1), a + 1); playerPositions.Add(a + 1);
            cb.gameObject.AddComponent<Automata>();
            cb.GetComponent<Automata>().Init(cb, true);
        }
    }
    void OnDestroy()
    {
		Events.OnAvatarCrash -= OnAvatarCrash;
        Events.OnAvatarFall -= OnAvatarFall;
        Events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
        Events.StartMultiplayerRace -= StartMultiplayerRace;
        Events.OnAlignAllCharacters -= OnAlignAllCharacters;
		Events.FreezeCharacters -= FreezeCharacters;
    }
	
    void OnReorderAvatarsByPosition(List<int> playerPositions)
    {
        this.playerPositions = playerPositions;
    }
	public void OnAvatarFall(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior);
    }
    public void OnAvatarCrash(CharacterBehavior characterBehavior)
    {
#if UNITY_ANDROID
        if(characterBehavior.player.id == 0)
            Handheld.Vibrate();
#endif
        killCharacter(characterBehavior);
    }
    public bool existsPlayer(int id)
    {
        bool exists = false;
        characters.ForEach((cb) =>
        {
            if (cb.player != null && cb.player.id == id) exists = true;
        });
        return exists;
    }
    public CharacterBehavior AddAutomata(int id)
    {
        print("__ADD Auatomata " + id);
        CharacterBehavior cb = AddNewCharacter(id, true);
        if (cb != null)
        {
            cb.gameObject.AddComponent<Automata>();
            cb.GetComponent<Automata>().Init(cb, false);
            return cb;
        }
        else return null;
    }
    public CharacterBehavior AddNewCharacter(int id, bool isAutomata)
    {
        print("characters.Count " + characters.Count + "  gameCamera.state  " + Game.Instance.gameCamera.state + " isAutomata " + isAutomata  + " canStartPlayers " + canStartPlayers);
        if (!canStartPlayers)
            return null;
        if (characters.Count == 0 && Game.Instance.gameCamera.state != GameCamera.states.WAITING_TO_TRAVEL)
            return null;

        Events.OnSoundFX("coin");

		Vector3 pos = Vector3.zero;

		if(characters.Count >0)
			pos = characters[0].transform.position;
		
        pos.y += 3;
        pos.x = 0;

		if(distance<20)
             pos.x = (separationX * id) - ((separationX * 2) - separationX / 2);

        CharacterBehavior characterBeavior = addCharacter(pos, id);
        if (id ==0)
            mainCharacter = characterBeavior;

        //Events.ForceFrameRate(1);
        return characterBeavior;
    }
	public CharacterBehavior addCharacter(Vector3 pos, int id)
	{
        Events.OnAddNewPlayer(id);
        CharacterBehavior newCharacter = null;
		foreach (CharacterBehavior cb in deadCharacters)
		{
			if (cb.player.id == id)
				newCharacter = cb;
		}
		if (newCharacter == null)
        {
            newCharacter = Instantiate(character, Vector3.zero, Quaternion.identity) as CharacterBehavior;
            if (id == 0) mainCharacter = newCharacter;
        }
		else
			deadCharacters.Remove(newCharacter);
       
        Player player = newCharacter.GetComponent<Player> ();
		player.Init(id);

        player.id = id;
		newCharacter.Revive();
		characters.Add(newCharacter);
        totalCharacters = characters.Count;

       // if (isAndroid) pos.x = 0;

        newCharacter.transform.position = pos;
		Events.OnCharacterInit (id);
        player.SetInvensible(3);
        return newCharacter;
	}
	int automaticIdPosition = 0;
	
	//public CharacterBehavior AddChildPlayer(CharacterBehavior parentPlayer)
	//{
	//	int id = parentPlayer.controls.childs.Count + 4;
	//	CharacterBehavior newCharacter = addCharacter(parentPlayer.transform.position, id);
	//	parentPlayer.controls.AddNewChild( newCharacter );
	//	return newCharacter;
	//}
	float separationOnReplay = 1f;
	Vector3 CalculateInitialPosition(Vector3 pos, int positionID)
	{		
		float _x;
        //if (isAndroid)
        //    _x = 0;
        //else 
        if (Data.Instance.isReplay)
			_x = ((float)positionID * separationOnReplay)  - (((((float)totalCharacters-1))/2)*separationOnReplay);
		else
			_x = (separationX * positionID+1) - ((separationX*2)- separationX/2);

		return new Vector3(_x-1.75f,pos.y);
	}

	public void KillAllCharacters()
	{
		foreach (CharacterBehavior cb in characters)
			cb.Die ();
	}
    public void killCharacter(CharacterBehavior characterBehavior)
    {
        if (Game.Instance.state == Game.states.GAME_OVER)
            return;

        print("DIE: " + characters.Count);

        if (characterBehavior.player.id == 0) 
            mainCharacter = null;

        characters.Remove(characterBehavior);
        totalCharacters = characters.Count;
        deadCharacters.Add(characterBehavior);
        Events.OnAvatarDie(characterBehavior);

        if (characters.Count == 0)
            StartCoroutine(GameOver(characterBehavior));
        else
        {
            bool stillPlayingRealCharacters = false;
            foreach(CharacterBehavior cb in characters)
            {
               // if (cb.GetComponent<Automata>() == null)
                    stillPlayingRealCharacters = true;
            }
            print("DIE: stillPlayingRealCharacters " + stillPlayingRealCharacters);
            if (!stillPlayingRealCharacters)
            {                   
                StartCoroutine(GameOver(characterBehavior));
            }
        }

    }
    IEnumerator GameOver(CharacterBehavior cb)
    {
        Events.AllDead();
		Events.OnSoundFX("deathFX");
        Game.Instance.GameOver();
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        Events.OnGameOver(false);
        yield return new WaitForSeconds(1.32f);
    }
    public float GetCharacterRot()
    {
        if (getMainCharacter() == null) return 0;
        else
            return  getMainCharacter().rotationY;
    }
    public CharacterBehavior getMainCharacter()
    {
        if (mainCharacter != null) return mainCharacter;
        if (getTotalCharacters() <= 0)
        {
            Debug.LogError("[ERROR] No hay más characters y sigue pidiendo...");
          //  print("[ERROR] No hay más characters y sigue pidiendo...");
            return null;
        }
        return characters[0];
    }
    public Vector3 getPositionMainCharacter()
    {
        return getMainCharacter().transform.position;
    }
	public virtual Vector3 getCameraPosition()
    {
        ///////retomar
        int totalCharacters = getTotalCharacters();
        if (mainCharacter == null)
        {
            Vector3 normalPosition = Vector3.zero;
            totalCharacters = 0;
            foreach (CharacterBehavior cbs in characters)
            {
                totalCharacters++;
                normalPosition += cbs.transform.localPosition;
            }
            if (totalCharacters > 0)
            {
                normalPosition /= totalCharacters;
                normalPosition.y += 0.35f + (totalCharacters / 3f);
                normalPosition.z = distance - 3.2f - (totalCharacters / 2f);
            }
            return normalPosition;
        }
        else
        {
            Vector3 p;
            if (totalCharacters == 0)
                p = Vector3.zero;
            else
                p = mainCharacter.transform.position;

            p.y += 0.5f;
            p.z = distance - 1.9f;
            return p;
        }
    }
    public int getTotalCharacters()
    {
        return totalCharacters;
    }
    public float getDistance()
    {
        return distance;
    }
    void OnAlignAllCharacters()
    {
        foreach (CharacterBehavior cb in characters)
        {
            Vector3 pos = cb.transform.localPosition;
            pos.x = 0;
            pos.y = 1;
            cb.transform.localPosition = pos;
        }
    }
	public void ResetJumps(){
		foreach (CharacterBehavior cb in characters) {
			cb.ResetJump ();
		}
	}
	public void OnLevelComplete()
	{
		foreach (CharacterBehavior cb in characters) {
			//cb.SuperJump (2200);
			cb.player.SetInvensible (6);
		}
	}
	public virtual Vector3 getPositionByTeam(int id) {
		return Vector3.zero;
	}
}
