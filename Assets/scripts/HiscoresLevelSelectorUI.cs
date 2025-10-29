using UnityEngine;

public class HiscoresLevelSelectorUI : MonoBehaviour
{
    [SerializeField] HiscoresMobile hiscoresMobile;
    [SerializeField] GameObject panel;
    private void Start()
    {
        Close();
    }
    public void Init()
    {
        panel.SetActive(true);
        hiscoresMobile.Init(1,MissionsManager.Instance.MissionTorneo, null);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
