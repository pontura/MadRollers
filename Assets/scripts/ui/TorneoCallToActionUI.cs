using UnityEngine;

public class TorneoCallToActionUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] GameObject panel;

    private void Start()
    {
        Close();
        Events.OpenTorneoCallToAction += OpenTorneoCallToAction;
    }
    private void OnDestroy()
    {
        Events.OpenTorneoCallToAction -= OpenTorneoCallToAction;
    }
    public void OpenTorneoCallToAction(int id = 1)
    { 
        panel.SetActive(true);
        int torneoScore = UserData.Instance.hiscoresByMissions.GetTorneoScore();
        int torneoRank = UserData.Instance.hiscoresByMissions.torneoRank;
        if (id == 0)
        {
            if(torneoRank>0)
                field.text = "Rank " + torneoRank + ". " + torneoScore + " points. Wanna give it another shot?";
            else
                field.text =  Utils.FormatNumbers(torneoScore) + " points in the Tournament. Wanna give it another shot?";
            return;
        } else if (torneoScore == 0)
            field.text = "You're not that bad... Looks like you're ready for a real match";
        else if (!MissionsManager.Instance.HasPlayedTorneoToday())
            field.text = "No action from you in today’s tournament… yet";
        else
            panel.SetActive(false);
    }
    public void GotoTorneo()
    {
        MissionsManager.Instance.PlayTorneo();
    }
    public void Close()
    {
        panel.SetActive(false);
    }

}
