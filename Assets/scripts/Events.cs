using UnityEngine;
using System.Collections.Generic;

public static class Events
{
    public static System.Action OnStartGameScene = delegate { };
    public static System.Action<string> OnChangeScene = delegate { };
    public static System.Action<bool> SetHamburguerButton = delegate { };
    public static System.Action RefreshHiscores = delegate { };

    public static System.Action<Texture2D, int> OnHiscore = delegate { };   

	public static System.Action AddNewCredit = delegate { };

    public static System.Action IAPInit = delegate { };
    public static System.Action<System.Action<bool>> BuyIAP = delegate { };

    public static System.Action<int> OnCharacterInit = delegate { };
	public static System.Action<int, string> OnDrawScore = delegate { };
    public static System.Action ResetHandwritingText = delegate { };
    public static System.Action AllDead = delegate { };
    public static System.Action Respawn = delegate { };
    public static System.Action<bool> OnGameOver = delegate { };
    public static System.Action OnContinue = delegate { };
    public static System.Action<string> VoiceFromResources = delegate { };
    public static System.Action<string> OnSoundFX = delegate { };
	public static System.Action<MadRollersSFX.types, int> OnMadRollerFX = delegate { };
    public static System.Action<bool> OnFadeALittle = delegate { };
    public static System.Action OnInterfacesStart = delegate { };
    public static System.Action OnGameStart = delegate { };
    public static System.Action OnIntro = delegate { };
    public static System.Action<bool> OnGamePaused = delegate { };
    public static System.Action<bool> SetSettingsButtonStatus = delegate { };
	public static System.Action<string> OnDestroySceneObject = delegate { };


    public static System.Action<int, int> OnSetStarsToMission = delegate { };    
    public static void MissionStart(int levelID) { OnMissionStart(levelID); }
    public static System.Action<int> OnMissionStart = delegate { };
	public static System.Action OnMissionProgress = delegate { };
    public static System.Action<int> OnMissionComplete = delegate { };
	public static void MissionComplete() { OnMissionComplete(Data.Instance.missions.MissionActiveID); }
	public static System.Action NewMissionStart = delegate { };
	public static System.Action<string> ShowNotification = delegate { };
	public static System.Action<ListenerDispatcher.myEnum> OnListenerDispatcher = delegate { };
	public static void ListenerDispatcher(ListenerDispatcher.myEnum message) { OnListenerDispatcher(message); }
	public static System.Action<int, Vector3, int, ScoresManager.types> OnScoreOn = delegate { };
    public static System.Action<int> OnChangeMood = delegate { };
    public static System.Action<int> OnAddNewPlayer = delegate { };
	public static System.Action<string, Vector3> OnAddSpecificPowerUp = delegate { };
    public static System.Action<Vector3> OnAddPowerUp = delegate { };
    public static System.Action OnCreateBonusArea = delegate { };
    public static System.Action<Vector3, Color> OnAddExplotion = delegate { };    
    public static System.Action OnAlignAllCharacters = delegate { };
	public static System.Action OnResetScores = delegate { };
    public static System.Action OnResetMultiplayerData = delegate { };
    public static System.Action<List<int>> OnReorderAvatarsByPosition = delegate { };
    public static void AddExplotion(Vector3 position, Color color) { OnAddExplotion(position, color); }
    public static System.Action<Vector3, int> OnAddObjectExplotion = delegate { };
	public static System.Action<Vector3, Material[], Vector3[]> OnAddHeartsByBreaking = delegate { };    
    public static System.Action<Vector3, string, string> OnAddTumba = delegate { };  
    public static System.Action<Vector3, Color> OnAddWallExplotion = delegate { };
    public static void AddWallExplotion(Vector3 position, Color color) { OnAddWallExplotion(position, color); }    
    public static System.Action OnOpenMainMenu = delegate { };
    public static System.Action OnCloseMainmenu = delegate { };
    public static System.Action OnResetLevel = delegate { };
    public static System.Action StartMultiplayerRace = delegate { };
    public static System.Action<float> ChangeCurvedWorldX = delegate { };
    public static System.Action SetVictoryArea = delegate { };
    public static System.Action<string> OnAlertSignal = delegate { };
    public static System.Action<string> OnChangeBackgroundSide = delegate { };
    public static System.Action<int, Weapon.types> OnChangeWeapon = delegate { };
    public static System.Action<int, Powerup.types> OnAvatarGetItem = delegate { };
    public static System.Action<Player.fxStates> OnAvatarChangeFX = delegate { };
    public static System.Action<CharacterBehavior> OnAvatarCrash = delegate { };
    public static System.Action<CharacterBehavior> OnAvatarFall = delegate { };
    public static System.Action<CharacterBehavior> OnAvatarDie = delegate { };
    public static System.Action OnAvatarProgressBarEmpty = delegate { };    
    public static System.Action OncharacterCheer = delegate { };
	public static System.Action<int> OnAvatarJump= delegate { };
    public static System.Action<int> OnAvatarShoot = delegate { };
    public static System.Action OnCompetitionMissionComplete = delegate { };
    public static System.Action<int> OnSetNewAreaSet = delegate { };
    public static System.Action<int> OnUseHearts = delegate { }; 
    public static System.Action OnGrabHeart = delegate { };
    public static System.Action<string> AdvisesOn = delegate { };
	public static System.Action OnFireUI = delegate { };
	public static System.Action<string> OnGenericUIText = delegate { };
	public static System.Action OnJoystickUp = delegate { };
	public static System.Action OnJoystickDown = delegate { };
	public static System.Action OnJoystickRight = delegate { };
	public static System.Action OnJoystickLeft = delegate { };
	public static System.Action OnJoystickClick = delegate { };
	public static System.Action OnJoystickBack= delegate { };
    public static System.Action OnSaveScore = delegate { };
    public static System.Action<int> OnPayPixeles = delegate { };
    public static System.Action<bool> OnTalk = delegate { };
	public static System.Action<float, float> RalentaTo = delegate { };
	public static System.Action<float> ForceFrameRate = delegate { };
	public static System.Action<int> OnVersusTeamWon= delegate { };
	public static System.Action<bool> OnMusicStatus= delegate { };
	public static System.Action<bool> OnSFXStatus= delegate { };
	public static System.Action<bool> OnVoicesStatus= delegate { };
	public static System.Action<bool> OnMadRollersSFXStatus= delegate { };
    public static System.Action<bool> OnBossActive = delegate { };
	public static System.Action OnBossDropBomb = delegate { };
	public static System.Action<int> OnBossDropRay = delegate { };
    public static System.Action<int> OnBossSpecial = delegate { };
    public static System.Action<int> OnBossInit = delegate { };
	public static System.Action<string> OnBossSetNewAsset = delegate { };
	public static System.Action<int> OnBossSetTimer = delegate { };
	public static System.Action<float> OnBossHitsUpdate = delegate { };
    public static System.Action ResetMissionsBlocked = delegate { };
	public static System.Action<bool> FreezeCharacters = delegate { };
    public static System.Action<int> ChangePlayer = delegate { };
    public static System.Action<CameraChromaManager.types> OnCameraChroma = delegate { };


    public static System.Action<float> SetVolume = delegate { };
    public static System.Action<float> SetSoundsVolume = delegate { };
    public static System.Action<bool> MuteMusic = delegate { };
    public static System.Action<bool> MuteSounds = delegate { };
}
