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

    int stepID = 0;
    float timer;
    int missionActiveID;
    void Start()
    {
        missionActiveID = Data.Instance.missions.MissionActiveID;
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
        if (missionActiveID > 2)
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
    void Init()
    {
        panel.SetActive(false);

        foreach (GameObject go in hideInToturial)
            go.SetActive(false);

        if (missionActiveID == 0)
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
        Time.timeScale = 1;
        panel.SetActive(false);
    }
    void InitPanel(int id)
    {
        timer = Time.realtimeSinceStartup;
        panel.SetActive(true);

        ResetAll();

        if (missionActiveID == 0)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("BIENVENIDX. YA ERES UN MAD ROLLER!");
                    break;
                case 1:
                    step_generic.Open("O SEA, UN VIDEOGAME HATTER!");
                    break;
                case 2:
                    step_generic.Open("SI... ODIAS LOS VIDEOJUEGOS...");
                    break;
                case 3:
                    step_generic.Open("...SOBRE TODO ESTAS CHATARRAS OCHENTOSAS");
                    break;
                case 4:
                    moveGO.SetActive(true);
                    step_move.Open("MOVETE PARA ESQUIVAR O RECOGER PIXELES");
                    break;
                case 5:
                    jumpGO.SetActive(true);
                    step_jump.Open("SALTA! CUIDADO CON CAERTE!");
                    break;
                case 6:
                    step_jump.Open("HAY DOBLE SALTO! (A VECES TE SALVA)");
                    break;
                case 7:
                    shootGO.SetActive(true);
                    step_shot.Open("ROMPE TODO! DISPARA, DESTRUYE, DISFRUTALO!");
                    break;
                case 8:
                    step_generic.Open("OJO... ABSOLUTAMENTE TODO ES ROMPIBLE");
                    break;
                case 9:
                    step_generic.Open("MUY BIEN! YA CASI TERMINAS EL ENTRENAMIENTO...");
                    break;
                case 10:
                    step_generic.Open("ENTRA EN EL PORTAL Y ESTARAS LISTO!");
                    break;
            }
        }
        else if (missionActiveID == 1)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("A VER COMO TE VA AHORA, CON EL TEAM COMPLETO!");
                    break;
                case 1:
                    step_generic.Open("CUIDADO CON ESTAS CUCHILLAS!");
                    break;
            }
        }
        else if (missionActiveID == 2)
        {
            switch (id)
            {
                case 0:
                    step_generic.Open("UN BOSS!");
                    break;
                case 1:
                    step_generic.Open("DESTRUYANLO! SI SE ANIMAN A GANAR!");
                    break;
            }
        }
        stepID++;
        Time.timeScale = 0;
    }
}
