using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuration")]
    public int scoreObjectif = 1000;
    // CHANGEMENT : 'public' pour que Naruto puisse lire le numéro du tir
    public int tirsEffectues = 0; 
    private int scoreTotal = 0;
    private bool dernierTirArrete = false;

    private string[] questionsFoot = {
        "Le PSG a-t-il gagné la LDC ?",
        "Y a-t-il 11 joueurs par équipe ?",
        "Mbappé joue-t-il au Real Madrid ?",
        "La France a-t-elle 2 étoiles ?",
        "Le gardien peut-il toucher le ballon à la main ?"
    };

    [Header("UI Interne")]
    private GameObject canvasQuiz;
    private TextMeshProUGUI texteQuestion;
    private TextMeshProUGUI texteScoreUI;

    void Awake()
    {
        Instance = this;
        CreerInterfaceParCode();
    }

    public void EnregistrerAction(bool arretReussi)
    {
        dernierTirArrete = arretReussi;
        if (arretReussi) scoreTotal += 400; 

        MettreAJourUI();
        StartCoroutine(PauseAvantQuestion());
    }

    IEnumerator PauseAvantQuestion()
    {
        yield return new WaitForSeconds(1.0f);
        
        // On force la vérification pour éviter le "Figeage"
        if (questionsFoot != null && questionsFoot.Length > 0)
        {
            canvasQuiz.SetActive(true);
            int indexAleatoire = Random.Range(0, questionsFoot.Length);
            texteQuestion.text = questionsFoot[indexAleatoire];
        }
        else
        {
            // Sécurité : si le tableau est vide, on affiche un texte par défaut au lieu de planter
            canvasQuiz.SetActive(true);
            texteQuestion.text = "Bien joué ! On double les points ?";
        }
    }

    public void ValiderReponse(bool bonneReponse)
    {
        if (bonneReponse && dernierTirArrete) scoreTotal += 400;

        canvasQuiz.SetActive(false);
        tirsEffectues++;
        MettreAJourUI();

        if (tirsEffectues >= 5) TerminerJeu();
        else RelancerTir();
    }

    void RelancerTir()
    {
        TirMascotte tm = FindObjectOfType<TirMascotte>();
        if (tm != null) tm.ReplacerBallon();

        MascotteAutonome naruto = FindObjectOfType<MascotteAutonome>();
        // AMÉLIORATION : Délai de 2 secondes avant que Naruto ne reparte
        if (naruto != null) naruto.Invoke("DemarrerCourse", 2.0f);
    }

    void TerminerJeu()
    {
        canvasQuiz.SetActive(true);
        string message = scoreTotal >= scoreObjectif ? "GAGNÉ !" : "PERDU...";
        texteQuestion.text = message + "\nScore Final : " + scoreTotal;
        
        // On cache les boutons Vrai/Faux actuels
        foreach(Button b in canvasQuiz.GetComponentsInChildren<Button>()) b.gameObject.SetActive(false);

        // On crée le bouton Recommencer
        CreerBouton(canvasQuiz.transform.GetChild(0), "REJOUER", new Vector2(-150, -150), () => RecommencerPartie(), Color.blue);
        
        // On crée le bouton Quitter
        CreerBouton(canvasQuiz.transform.GetChild(0), "QUITTER", new Vector2(150, -150), () => QuitterJeu(), Color.gray);
    }

    public void RecommencerPartie()
    {
        // Recharge la scène actuelle pour tout remettre à zéro
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitterJeu()
    {
        Debug.Log("Le jeu se ferme...");
        Application.Quit(); // Ferme l'application (ne marche que dans le build final)
    }

    void MettreAJourUI()
    {
        if (texteScoreUI != null) texteScoreUI.text = "SCORE: " + scoreTotal + " | TIRS: " + tirsEffectues + "/5";
    }

    void CreerInterfaceParCode()
    {
        GameObject go = new GameObject("MonSuperQuiz");
        canvasQuiz = go;
        Canvas c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        go.AddComponent<GraphicRaycaster>();

        GameObject fond = new GameObject("Fond");
        fond.transform.SetParent(go.transform);
        Image img = fond.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.85f);
        RectTransform rtFond = fond.GetComponent<RectTransform>();
        rtFond.anchorMin = rtFond.anchorMax = new Vector2(0.5f, 0.5f);
        rtFond.sizeDelta = new Vector2(800, 500);
        rtFond.anchoredPosition = Vector2.zero;

        GameObject qGo = new GameObject("QuestionTxt");
        qGo.transform.SetParent(fond.transform);
        texteQuestion = qGo.AddComponent<TextMeshProUGUI>();
        texteQuestion.alignment = TextAlignmentOptions.Center;
        texteQuestion.fontSize = 40;
        RectTransform rtQ = qGo.GetComponent<RectTransform>();
        rtQ.anchorMin = rtQ.anchorMax = new Vector2(0.5f, 0.5f);
        rtQ.sizeDelta = new Vector2(700, 150);
        rtQ.anchoredPosition = new Vector2(0, 50);

        GameObject scoreGo = new GameObject("ScoreTxt");
        scoreGo.transform.SetParent(fond.transform);
        texteScoreUI = scoreGo.AddComponent<TextMeshProUGUI>();
        texteScoreUI.fontSize = 30;
        texteScoreUI.alignment = TextAlignmentOptions.Center;
        RectTransform rtS = scoreGo.GetComponent<RectTransform>();
        rtS.anchorMin = rtS.anchorMax = new Vector2(0.5f, 1f);
        rtS.sizeDelta = new Vector2(600, 50);
        rtS.anchoredPosition = new Vector2(0, -50);

        CreerBouton(fond.transform, "VRAI", new Vector2(-150, -100), () => ValiderReponse(true), Color.green);
        CreerBouton(fond.transform, "FAUX", new Vector2(150, -100), () => ValiderReponse(false), Color.red);
        canvasQuiz.SetActive(false);
    }

    void CreerBouton(Transform parent, string nom, Vector2 pos, UnityEngine.Events.UnityAction action, Color coul)
    {
        GameObject bGo = new GameObject("Btn_" + nom);
        bGo.transform.SetParent(parent);
        bGo.AddComponent<Image>().color = coul;
        bGo.AddComponent<Button>().onClick.AddListener(action);
        GameObject tGo = new GameObject("L");
        tGo.transform.SetParent(bGo.transform);
        TextMeshProUGUI t = tGo.AddComponent<TextMeshProUGUI>();
        t.text = nom; t.color = Color.white; t.alignment = TextAlignmentOptions.Center;
        RectTransform rt = bGo.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(200, 80);
        rt.anchoredPosition = pos;
    }
}