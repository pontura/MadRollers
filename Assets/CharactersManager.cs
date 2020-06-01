using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
    public List<CharacterBehavior> characters;
    public List<CharacterBehavior> deadCharacters;

    private float separationX  = 4.5f;

    public float distance;
	private float MAX_SPEED = 19;
    private float speedRun = 0;
	private float acceleration = 10;
    private Missions missions;
    public List<int> playerPositions;
	bool canStartPlayers;
    private IEnumerator RalentaCoroutine;
    public int totalCharacters;
    bool isAndroid;

    void Awake()
    {
        isAndroid = Data.Instance.isAndroid;
        distance = 0;
        Data.Instance.events.OnAlignAllCharacters += OnAlignAllCharacters;
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnAutomataCharacterDie += OnAutomataCharacterDie;
        Data.Instance.events.FreezeCharacters += FreezeCharacters;
    }
    public virtual void Init()
    {
        missions = Data.Instance.GetComponent<Missions>();
        gameObject.AddComponent<AutomatasManager>();
        Data.Instance.inputSavedAutomaticPlay.Init(this);
        StartCoroutine(AddCharactersInitials());
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
        print("______________StartMultiplayerRace ");
		canStartPlayers = true;
		if (Data.Instance.isReplay) {
			speedRun = MAX_SPEED;
		}
        if (!isAndroid)
            Loop ();
    }
	void Loop()	{
		foreach (CharacterBehavior cb in characters)
			cb.characterMovement.SetCharacterScorePosition ();
		Invoke ("Loop", 1);
	}
	void LateUpdate()
    {
		if (freezed)
			return;
		
//		if(Input.GetKeyDown(KeyCode.M))
//			AddChildPlayer( getMainCharacter() );
		
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
		if (Data.Instance.multiplayerData.player2) { addCharacter(CalculateInitialPosition(pos, positionID+1), 1); playerPositions.Add(1); };
		if (Data.Instance.multiplayerData.player3) { addCharacter(CalculateInitialPosition(pos, positionID+2), 2); playerPositions.Add(2); };
		if (Data.Instance.multiplayerData.player4) { addCharacter(CalculateInitialPosition(pos, positionID+3), 3); playerPositions.Add(3); };

		yield return null;
	}
    void OnDestroy()
    {
		Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarFall;
        Data.Instance.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnAlignAllCharacters -= OnAlignAllCharacters;
		Data.Instance.events.OnAutomataCharacterDie -= OnAutomataCharacterDie;
		Data.Instance.events.FreezeCharacters -= FreezeCharacters;
    }
	void OnAutomataCharacterDie(CharacterBehavior automataCharacter)
	{
		CharacterBehavior parentCharacter = null;
		CharacterBehavior childCharacter = null;
		foreach (CharacterBehavior cb1 in characters) {
			foreach (CharacterBehavior cb2 in cb1.controls.childs) {
				if (cb2 == automataCharacter) {					
					parentCharacter = cb1;
					childCharacter = cb2; 
					parentCharacter.controls.StopAllCoroutines ();
					childCharacter.GetComponent<CharacterAutomata> ().Reset ();
				}					
			}
		}
		if(childCharacter != null)
			parentCharacter.controls.RemoveChild (childCharacter);
		
		deadCharacters.Remove (automataCharacter);
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
            cb.controls.isAutomata = true;
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
        
        Data.Instance.events.OnSoundFX("coin", id);
		Vector3 pos = Vector3.zero;

		if(characters.Count >0)
			pos = characters[0].transform.position;
		
        pos.y += 3;
        pos.x = 0;

		if(distance<20)
             pos.x = (separationX * id) - ((separationX * 2) - separationX / 2);

        CharacterBehavior cb = addCharacter(pos, id);

        if (isAndroid || isAutomata)
            cb.controls.isAutomata = true;
        else
           Data.Instance.multiplayerData.AddNewCharacter(id);

        Data.Instance.events.ForceFrameRate(1);
        return cb;
    }
	public CharacterBehavior addCharacter(Vector3 pos, int id)
	{
        Data.Instance.events.OnAddNewPlayer(id);
		CharacterBehavior newCharacter = null;
		foreach (CharacterBehavior cb in deadCharacters)
		{
			if (cb.player.id == id)
				newCharacter = cb;
		}
		if (newCharacter == null)
			newCharacter = Instantiate(character, Vector3.zero, Quaternion.identity) as CharacterBehavior;
		else
			deadCharacters.Remove(newCharacter);
       
        Player player = newCharacter.GetComponent<Player> ();
		player.Init(id);

        player.id = id;
		newCharacter.Revive();
		characters.Add(newCharacter);
        totalCharacters = characters.Count;

        newCharacter.transform.position = pos;
		Data.Instance.events.OnCharacterInit (id);
        player.SetInvensible(3);
        return newCharacter;
	}
	int automaticIdPosition = 0;
	public CharacterBehavior AddAutomaticPlayer(int id)
	{
		CharacterBehavior newCharacter = addCharacter(CalculateInitialPosition(new Vector3(1,1,1), automaticIdPosition), id);
		newCharacter.controls.isAutomata = true;
		automaticIdPosition++;
		return newCharacter;
	}
	public CharacterBehavior AddChildPlayer(CharacterBehavior parentPlayer)
	{
		int id = parentPlayer.controls.childs.Count + 4;
		CharacterBehavior newCharacter = addCharacter(parentPlayer.transform.position, id);
		newCharacter.controls.isAutomata = true;
		parentPlayer.controls.AddNewChild( newCharacter );
		newCharacter.GetComponent<CharacterAutomata> ().Init ();
		return newCharacter;
	}
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

		return new Vector3(_x,pos.y);
	}

	public void KillAllCharacters()
	{
		foreach (CharacterBehavior cb in characters)
			cb.Die ();
	}
    public void killCharacter(CharacterBehavior characterBehavior)
    {
        print("killCharacter characters.Count " + characters.Count + " id:" + characterBehavior.player.id + " automata: " + characterBehavior.controls.isAutomata);
        if (Game.Instance.state == Game.states.GAME_OVER)
            return;
        
        characters.Remove(characterBehavior);
        totalCharacters = characters.Count;
        deadCharacters.Add(characterBehavior);
        Data.Instance.events.OnAvatarDie(characterBehavior);

        if (characters.Count == 0)
            StartCoroutine(restart(characterBehavior));
        else
        {
            bool stillPlayingRealCharacters = false;
            foreach(CharacterBehavior cb in characters)
            {
                if (!cb.controls.isAutomata)
                    stillPlayingRealCharacters = true;
            }
            if (!stillPlayingRealCharacters)
            {                   
                StartCoroutine(restart(characterBehavior));
            }
        }

    }
    IEnumerator restart(CharacterBehavior cb)
    {
        print("_______________ RESTART");
		Data.Instance.events.OnCameraChroma (CameraChromaManager.types.RED);
		Data.Instance.events.OnSoundFX("dead", -1);

        Game.Instance.GameOver();
        yield return new WaitForSeconds(0.05f);
        Data.Instance.events.OnGameOver(false);
        yield return new WaitForSeconds(1.32f);
    }
    public CharacterBehavior getMainCharacter()
    {
        if(isAndroid)
            return characters[0];

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
        if (totalCharacters > 1 && !isAndroid)
        {
            Vector3 normalPosition = Vector3.zero;
            totalCharacters = 0;
            foreach (CharacterBehavior cb in characters)
            {
                if (!cb.controls.isAutomata)
                {
                    totalCharacters++;
                    normalPosition += cb.transform.localPosition;
                }
            }
            if (totalCharacters > 0)
            {
                normalPosition /= totalCharacters;
                normalPosition.y += 0.15f + (totalCharacters / 3f);
                normalPosition.z = distance - 2.3f - (totalCharacters / 2.5f);
            }
            return normalPosition;
        }
        else
        {
            Vector3 p;
            if (totalCharacters == 0)
                p = Vector3.zero;
            else
                p = characters[0].transform.position;

            p.y += 0.5f;
            p.z = distance - 2f;
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
