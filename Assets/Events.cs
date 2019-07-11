using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Events : MonoBehaviour {

    //public System.Action<string> OnFacebookNewUserLogged = delegate { };
    //public System.Action OnFacebookIdAdded = delegate { };
    //public System.Action<string, string> OnFacebookUserLoaded = delegate { };
    //public System.Action<string, int, int, bool> OnSetUserData = delegate { };
    public System.Action<string> OnChangeScene = delegate { };
    public System.Action<bool> SetHamburguerButton = delegate { };
    public System.Action RefreshHiscores = delegate { };

    public System.Action<Texture2D, int> OnHiscore = delegate { };   

	public System.Action AddNewCredit = delegate { }; 

	public System.Action<int> OnCharacterInit = delegate { };
	public System.Action<int, string> OnDrawScore = delegate { };
	public System.Action<bool> OnGameOver = delegate { };
    public System.Action<string> VoiceFromResources = delegate { };
    public System.Action<string, int> OnSoundFX = delegate { };
	public System.Action<MadRollersSFX.types, int> OnMadRollerFX = delegate { };
    public System.Action<float> SetVolume = delegate { };
    public System.Action<bool> OnFadeALittle = delegate { };
    public System.Action OnInterfacesStart = delegate { };
    public System.Action OnGameStart = delegate { };
    public System.Action OnIntro = delegate { };
    public System.Action<bool> OnGamePaused = delegate { };
    public System.Action<bool> SetSettingsButtonStatus = delegate { };
	public System.Action<string> OnDestroySceneObject = delegate { };


    public System.Action<int, int> OnSetStarsToMission = delegate { };
    
    public void MissionStart(int levelID) { OnMissionStart(levelID); }
    public System.Action<int> OnMissionStart = delegate { };

	public System.Action OnMissionProgress = delegate { };
    public System.Action<int> OnMissionComplete = delegate { };

    //lo llama level:
	public void MissionComplete() { OnMissionComplete(Data.Instance.missions.MissionActiveID); }    

	public System.Action NewMissionStart = delegate { };

	public System.Action<string> ShowNotification = delegate { };
	public System.Action<ListenerDispatcher.myEnum> OnListenerDispatcher = delegate { };
	public void ListenerDispatcher(ListenerDispatcher.myEnum message) { OnListenerDispatcher(message); }

   // public System.Action<int, Vector3, int> OnSetFinalScore = delegate { };
	public System.Action<int, Vector3, int, ScoresManager.types> OnScoreOn = delegate { };
   // public void ScoreSignalOn(int playerID, Vector3 position, int score) { OnScoreOn(playerID, position, score); }
    public System.Action<int> OnChangeMood = delegate { };

    public System.Action<int> OnAddNewPlayer = delegate { };

	public System.Action<string, Vector3> OnAddSpecificPowerUp = delegate { };
    public System.Action<Vector3> OnAddPowerUp = delegate { };
    public System.Action OnCreateBonusArea = delegate { };
    public System.Action<Vector3, Color> OnAddExplotion = delegate { };
    public System.Action OnAlignAllCharacters = delegate { };
	public System.Action OnResetScores = delegate { };
    public System.Action<List<int>> OnReorderAvatarsByPosition = delegate { };
    public void AddExplotion(Vector3 position, Color color) { OnAddExplotion(position, color); }

    public System.Action<Vector3, int> OnAddObjectExplotion = delegate { };
	public System.Action<Vector3, Material[], Vector3[]> OnAddHeartsByBreaking = delegate { };
    
    public System.Action<Vector3, string, string> OnAddTumba = delegate { };  

    public System.Action<Vector3, Color> OnAddWallExplotion = delegate { };
    public void AddWallExplotion(Vector3 position, Color color) { OnAddWallExplotion(position, color); }
    
    public System.Action OnOpenMainMenu = delegate { };
    public System.Action OnCloseMainmenu = delegate { };

    public System.Action OnResetLevel = delegate { };
    public System.Action StartMultiplayerRace = delegate { };
    public System.Action<float> ChangeCurvedWorldX = delegate { };
    public System.Action SetVictoryArea = delegate { };

    public System.Action<string> OnChangeBackgroundSide = delegate { };
    public System.Action<int, Weapon.types> OnChangeWeapon = delegate { };
    public System.Action<int, Powerup.types> OnAvatarGetItem = delegate { };
    public System.Action<Player.fxStates> OnAvatarChangeFX = delegate { };
    public System.Action<CharacterBehavior> OnAvatarCrash = delegate { };
    public System.Action<CharacterBehavior> OnAvatarFall = delegate { };
    public System.Action<CharacterBehavior> OnAvatarDie = delegate { };
    public System.Action OnAvatarProgressBarEmpty = delegate { };
    
    public System.Action OncharacterCheer = delegate { };

	public System.Action<int> OnAvatarJump= delegate { };

    public System.Action<int> OnAvatarShoot = delegate { };

    public System.Action OnCompetitionMissionComplete = delegate { };

	public System.Action<int> OnShowTutorial = delegate { };

    public System.Action<int> OnSetNewAreaSet = delegate { };
    public System.Action<int> OnUseHearts = delegate { };
    

    public System.Action OnGrabHeart = delegate { };
    public System.Action<string> AdvisesOn = delegate { };

	public System.Action OnFireUI = delegate { };

	public System.Action<string> OnGenericUIText = delegate { };

	public System.Action OnJoystickUp = delegate { };
	public System.Action OnJoystickDown = delegate { };
	public System.Action OnJoystickRight = delegate { };
	public System.Action OnJoystickLeft = delegate { };
	public System.Action OnJoystickClick = delegate { };
	public System.Action OnJoystickBack= delegate { };
	public System.Action<bool> OnTalk = delegate { };
	public System.Action<CharacterBehavior> OnAutomataCharacterDie = delegate { };
	public System.Action<float, float> RalentaTo = delegate { };
	public System.Action<float> ForceFrameRate = delegate { };
	public System.Action<int> OnVersusTeamWon= delegate { };

	public System.Action<bool> OnMusicStatus= delegate { };
	public System.Action<bool> OnSFXStatus= delegate { };
	public System.Action<bool> OnVoicesStatus= delegate { };
	public System.Action<bool> OnMadRollersSFXStatus= delegate { };

	public System.Action<bool> OnBossActive = delegate { };
	public System.Action OnBossDropBomb = delegate { };
	public System.Action<int> OnBossDropRay = delegate { };
    public System.Action<int> OnBossSpecial = delegate { };
    public System.Action<int> OnBossInit = delegate { };
	public System.Action<string> OnBossSetNewAsset = delegate { };
	public System.Action<int> OnBossSetTimer = delegate { };
	public System.Action<float> OnBossHitsUpdate = delegate { };
    public System.Action ResetMissionsBlocked = delegate { };

    public System.Action<Vector3> OnProjectilStartSnappingTarget = delegate { };
	public System.Action<Vector3> OnCameraZoomTo = delegate { };
	public System.Action<bool> FreezeCharacters = delegate { };
    public System.Action<int> ChangePlayer = delegate { };
    public System.Action<CameraChromaManager.types> OnCameraChroma = delegate { };
}
