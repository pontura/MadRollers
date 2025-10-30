using UnityEngine;

public class TorneoCallToActionUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] GameObject panel;
    bool dontShowAgain;

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
        if (dontShowAgain) return;
        panel.SetActive(true);
        int torneoScore = UserData.Instance.hiscoresByMissions.GetTorneoScore();
        int torneoRank = UserData.Instance.hiscoresByMissions.torneoRank;
        if (id == 0)
        {
            if(torneoRank>0)
                field.text = "Rank " + torneoRank + ". " + torneoScore + " points. Wanna give it another shot?";
            else
                field.text =  Utils.FormatNumbers(torneoScore) + " points in the Competition. Wanna give it another shot?";
            return;
        } else if (torneoScore == 0)
        {
            field.text = "You're not that bad... Looks like you're ready for a real match";
            dontShowAgain = true;
        }
        else if (!MissionsManager.Instance.HasPlayedTorneoToday())
        {
            field.text = "No action today in the competition… yet";
            dontShowAgain = true;
        }
        else
            panel.SetActive(false);
    }
    public void GotoTorneo()
    {
        Close();
        MissionsManager.Instance.PlayTorneo();
    }
    public void Close()
    {
        panel.SetActive(false);
    }

}
