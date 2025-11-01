using System;
using System.Collections;
using UnityEngine;

public class ToturialSteps : MonoBehaviour
{
    [SerializeField] GameObject panel;

    [SerializeField] GameObject[] hideInToturial;
    [SerializeField] GameObject moveGO;
    [SerializeField] GameObject jumpGO;
    [SerializeField] GameObject shootGO;

    [SerializeField] ToturialStep step_move;
    [SerializeField] ToturialStep step_jump;
    [SerializeField] ToturialStep step_shot;
    [SerializeField] ToturialStep step_generic;
    CharactersManager charactersManager;

    [SerializeField] MobileInputs inputs;

    OnBoardingSteps[] onboardingSteps;

    class OnBoardingSteps
    {
        public int step = 0;
        public string[] texts;
        public int[] panelsID;
    }

    int stepID = 0;
    float timer;
    int missionActiveID;
    int last_missionActiveID;
    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            Destroy(panel.gameObject);
            Destroy(this);
        }

        onboardingSteps = new OnBoardingSteps[1];
        OnBoardingSteps ons = new OnBoardingSteps();
        ons.texts = new string[3];
        ons.panelsID = new int[3];
        ons.texts[0] = "You’re down! But the show goes on!";        ons.panelsID[0] = 0;
        ons.texts[1] = "As long as a teammate’s still up";          ons.panelsID[1] = 0;
        ons.texts[2] = "Mash this button to respawn faster!";       ons.panelsID[2] = 1;
        onboardingSteps[0] = ons;

        missionActiveID = Data.Instance.missions.MissionActiveID;

        last_missionActiveID = PlayerPrefs.GetInt("last_missionActiveID", -1);
        int last_stepID = PlayerPrefs.GetInt("last_stepID", 0);

        if(last_missionActiveID == missionActiveID)
            stepID = last_stepID;
        else
        {
            PlayerPrefs.SetInt("last_missionActiveID", missionActiveID);
            PlayerPrefs.SetInt("last_stepID", 0);
        }
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
        int onboardingStepsDone = PlayerPrefs.GetInt("onboarding", 0);
        print("onboardingStepsDone " + onboardingStepsDone);
        if(onboardingStepsDone<1)
        {
            Events.OnAvatarDie += OnAvatarDie;
        }
        if (missionActiveID > 2 || onboardingStepsDone >= onboardingSteps.Length)
        {
            Destroy(panel.gameObject);
            Destroy(this);
        }
        else
        {
            ResetAll();
            Invoke("Init", 1);
        }
    }
    private void OnDestroy()
    {
        Events.OnAvatarDie -= OnAvatarDie;
    }

    private void OnAvatarDie(CharacterBehavior cb)
    {
        if(cb.player.id == 0 && charactersManager.totalCharacters>0)
        {
            PlayerPrefs.SetInt("onboarding", 1);
            steps = onboardingSteps[0];
            InitOnBoarding();
        }
    }

    void Init()
    {
        panel.SetActive(false);

        foreach (GameObject go in hideInToturial)
            go.SetActive(false);

        if (last_missionActiveID <0 || (missionActiveID == 0 &&  last_missionActiveID > missionActiveID))
        {
            moveGO.SetActive(false);
            jumpGO.SetActive(false);
            shootGO.SetActive(false);
        }
    }
    private void Update()
    {
       // print("Distance: " + charactersManager.distance + " timer: " + timer + " _______ " + Time.realtimeSinceStartup);
        if (missionActiveID == 0)
        {
            if (stepID == 0 && charactersManager.distance > 40)
                InitPanel(stepID);
            else if (stepID == 1 && charactersManager.distance > 55)
                InitPanel(stepID);
            else if (stepID == 2 && charactersManager.distance > 70)
                InitPanel(stepID);
            else if (stepID == 3 && charactersManager.distance > 85)
                InitPanel(stepID);
            else if (stepID == 4 && charactersManager.distance > 100)
                InitPanel(stepID);
            else if (stepID == 5 && charactersManager.distance > 177)
                InitPanel(stepID);
            else if (stepID == 6 && charactersManager.distance > 263)
                InitPanel(stepID);
            else if (stepID == 7 && charactersManager.distance > 310)
                InitPanel(stepID);
            else if (stepID == 8 && charactersManager.distance > 380)
                InitPanel(stepID);
            else if (stepID == 9 && charactersManager.distance > 540)
                InitPanel(stepID);
            else if (stepID == 10 && charactersManager.distance > 588)
                InitPanel(stepID);
        } else if (missionActiveID == 1)
        {
            if (stepID == 0 && charactersManager.distance > 20)
                InitPanel(stepID);
            else if (stepID == 1 && charactersManager.distance > 90)
                InitPanel(stepID);
        }
        else if (missionActiveID == 2)
        {
            if (stepID == 0 && charactersManager.distance > 200)
                InitPanel(stepID);
            else if (stepID == 1 && charactersManager.distance > 204)
                InitPanel(stepID);
        }
    }
    void ResetAll()
    {
        step_move.gameObject.SetActive(false);
        step_jump.gameObject.SetActive(false);
        step_shot.gameObject.SetActive(false);
        step_generic.gameObject.SetActive(false);
    }
    System.Action OnClose;
    public void OnClicked()
    {
        if (Time.realtimeSinceStartup < timer + 0.25f) return;
        print("OnClicked " + stepID);

        ResetAll();

        if (missionActiveID == 0)
        {
            switch (stepID - 1)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 5:
                    StartCoroutine(inputs.ForceBigJump());
                    break;
                case 6:
                    StartCoroutine(inputs.ForceBigJump());
                    break;
                case 7:
                    inputs.Shoot();
                    break;
            }
        }
        Events.OnGamePaused(false);
        Events.RalentaTo(1, 0.15f);
        panel.SetActive(false);
        if (OnClose != null)
            OnClose();
    }
    void InitPanel(int id)
    {
        PlayerPrefs.SetInt("last_stepID", id);
        timer = Time.realtimeSinceStartup;
        panel.SetActive(true);

        ResetAll();

        if (missionActiveID == 0)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("Welcome. YOU ARE A MAD ROLLER!");
                    break;
                case 1:
                    step_generic.Open("...a Videogame VIRUS!");
                    break;
                case 2:
                    step_generic.Open("you hate videogames...");
                    break;
                case 3:
                    step_generic.Open(".. and all this 80s crap");
                    break;
                case 4:
                    moveGO.SetActive(true);
                    step_move.Open("Move fast. Don´t crash!");
                    break;
                case 5:
                    jumpGO.SetActive(true);
                    step_jump.Open("Jump quick or say goodbye!");
                    break;
                case 6:
                    step_jump.Open("Double jump time!");
                    break;
                case 7:
                    shootGO.SetActive(true);
                    step_shot.Open("Destroy mode: ON. Let the fun begin!");
                    break;
                case 8:
                    step_generic.Open("Watch out... everything goes BOOM");
                    break;
                case 9:
                    step_generic.Open("Nice job! You’re almost done training!");
                    break;
                case 10:
                    step_generic.Open("Jump into the portal and become a legend!");
                    break;
            }
        }
        else if (missionActiveID == 1)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("No more solo play… time to squad up!");
                    break;
                case 1:
                    step_generic.Open("Watch out—those blades are sharp!");
                    break;
            }
        }
        else if (missionActiveID == 2)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("First Boss Fight!");
                    break;
                case 1:
                    step_generic.Open("Destroy it… if you dare to win!");
                    break;
            }
        }
        stepID++;
        Time.timeScale = 0;
        Events.OnGamePaused(true);
    }


    OnBoardingSteps steps;
    void InitOnBoarding()
    {
        print("InitOnBoarding " + steps.step + " lenght; " +  steps.texts.Length);
        if (steps.step >= steps.texts.Length)
        {
            OnClose = null;
            return;
        }
        int panelID = steps.panelsID[steps.step];
        StartCoroutine(InitOnBoardingC(panelID));
    }
    IEnumerator InitOnBoardingC(int panelID)
    {
        yield return new WaitForEndOfFrame();
        panel.SetActive(true);
        ResetAll();
        OnClose = InitOnBoarding;
        string t = steps.texts[steps.step];

        if(panelID == 0)
            step_generic.Open(t);
        else if (panelID == 1)
            step_jump.Open(t);

        steps.step++;
        Time.timeScale = 0;
        Events.OnGamePaused(true);
    }
}
