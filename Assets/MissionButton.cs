using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class MissionButton : MonoBehaviour {

	public Animation anim;
    public Image background;
    public int id;
	public int videoGameID;

	public Image logo;
	public Image floppyCover;

	public VideogameData videogameData;

    public Text missionField;

    public Image avatarImage;
    public Text usernameField;
    public Text hiscoreField;

    public int missionActive;
    public LevelSelectorMobile levelSelectorMobile;

    // se usa tanto para mobile como para Standalone!

    public void Init (VideogameData videogameData, bool animate = true) {
		this.videogameData = videogameData;
		logo.sprite = videogameData.logo;
		floppyCover.sprite = videogameData.floppyCover;
        if (animate)
        {
            anim["MissionButtonOn"].normalizedTime = 0;
            anim.Play("MissionButtonOn");
        }
        missionField.text = videogameData.name;
        usernameField.text = "MISSION 0";
    }
    public void SetMobile(LevelSelectorMobile levelSelectorMobile)
    {
        this.levelSelectorMobile = levelSelectorMobile;
    }

    // solo version Mobile Android!
    public void OnClick()
    {
        List<VoicesManager.VoiceData> list = Data.Instance.voicesManager.videogames_names;
        Data.Instance.voicesManager.PlaySpecificClipFromList(list, videogameData.id);

        anim.Play ("videoGameButtonMobile");
        Data.Instance.videogamesData.actualID = videogameData.id;
        Data.Instance.missions.MissionActiveID = Data.Instance.missions.GetMissionsByVideoGame(videogameData.id).missionUnblockedID;
        Invoke("DelayedClick", 1);
        levelSelectorMobile.OnMissionButtonClicked(this);
    }

	public void SetOn()
	{
		anim.Play ("MissionTopSetActive");
	}
    public void SetMenuButtonOff()
    {
        anim.Play("MissionButtonOff");
    }
    void DelayedClick()
    {
        if(Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Data.Instance.LoadLevel("Game");
        else
            levelSelectorMobile.missionSelectorMobile.Init();
       // Data.Instance.LoadLevel("Game");
    }
    public void GetHiscore()
    {
        missionActive = Data.Instance.missions.GetMissionsByVideoGame(videogameData.id).missionUnblockedID;
        missionField.text = "MISION " + (missionActive + 1);
        usernameField.text = "<loading...>";
        UserData.Instance.hiscoresByMissions.LoadHiscore(videogameData.id, missionActive, OnLoaded);
    }
    void OnLoaded(HiscoresByMissions.MissionHiscoreData data)
    {
        if (data == null || data.all.Count == 0 || !isActiveAndEnabled)
            return;

        usernameField.text = data.all[0].username;
        hiscoreField.text = Utils.FormatNumbers(data.all[0].score);
        UserData.Instance.avatarImages.GetImageFor(data.all[0].userID, OnAvatarImageLoaded);
    }
    void OnAvatarImageLoaded(Texture2D texture2d)
    {
        if (avatarImage != null)
            avatarImage.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }

}
