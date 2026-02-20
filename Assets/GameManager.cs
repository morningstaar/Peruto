using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuration")]
    public int scoreObjectif = 1000;
    public int tirsEffectues = 0; 
    private int scoreTotal = 0;
    private bool dernierTirArrete = false;
    private bool tirEnCours = false; 

    private string[] questionsFoot = {
        "Le PSG a-t-il gagné la LDC ?",
        "Y a-t-il 11 joueurs par équipe ?",
        "Mbappé joue-t-il au Real Madrid ?",
        "La France a-t-elle 2 étoiles ?",
        "Le gardien peut-il toucher le ballon à la main ?"
    };

    [Header("UI Interne")]
    private GameObject canvasQuiz;
    private GameObject canvasRegles;
    private GameObject canvasScoreFixe;
    private TextMeshProUGUI texteQuestion;
    private TextMeshProUGUI texteScorePermanent;

    void Awake()
    {
        Instance = this;
        CreerInterfaceParCode();
    }

    void Start()
    {
        MettreAJourUI();
        canvasRegles.SetActive(true);
        canvasScoreFixe.SetActive(true);
        tirEnCours = false;
        
        // Sécurité : On s'assure que le temps ne s'écoule pas ou que Naruto attend
        Time.timeScale = 1f; 
    }

    public void BoutonCommencer()
    {
        canvasRegles.SetActive(false);
        tirEnCours = true;
        
        // On force la recherche de Naruto
        MascotteAutonome naruto = FindObjectOfType<MascotteAutonome>();
        if (naruto != null) 
        {
            naruto.DemarrerCourse();
        }
        else {
            Debug.LogError("Erreur : Script MascotteAutonome non trouvé dans la scène !");
        }
    }

    public void EnregistrerAction(bool arretReussi)
    {
        if (!tirEnCours) return; 
        tirEnCours = false; 
        dernierTirArrete = arretReussi;
        if (arretReussi) scoreTotal += 400; 
        MettreAJourUI();
        StartCoroutine(PauseAvantQuestion());
    }

    IEnumerator PauseAvantQuestion()
    {
        yield return new WaitForSeconds(1.0f);
        if (questionsFoot != null && questionsFoot.Length > 0)
        {
            canvasQuiz.SetActive(true);
            int indexAleatoire = Random.Range(0, questionsFoot.Length);
            texteQuestion.text = questionsFoot[indexAleatoire];
        }
    }

    public void ValiderReponse(bool bonneReponse)
    {
        if (bonneReponse && dernierTirArrete) scoreTotal += 400;
        if (bonneReponse && !dernierTirArrete) scoreTotal += 20;

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
        if (naruto != null) 
        {
            // On attend 2 secondes avant le prochain tir
            StartCoroutine(AttendreProchainTir(naruto));
        }
    }

    IEnumerator AttendreProchainTir(MascotteAutonome n)
    {
        yield return new WaitForSeconds(2.0f);
        tirEnCours = true;
        n.DemarrerCourse();
    }

    void TerminerJeu()
    {
        canvasQuiz.SetActive(true);
        canvasScoreFixe.SetActive(false);
        string message = scoreTotal >= scoreObjectif ? "GAGNÉ !" : "PERDU...";
        texteQuestion.text = message + "\nScore Final : " + scoreTotal;
        
        foreach(Button b in canvasQuiz.GetComponentsInChildren<Button>()) 
            b.gameObject.SetActive(false);

        CreerBouton(canvasQuiz.transform.GetChild(0), "REJOUER", new Vector2(-150, -150), () => RecommencerPartie(), Color.blue);
        CreerBouton(canvasQuiz.transform.GetChild(0), "QUITTER", new Vector2(150, -150), () => QuitterJeu(), Color.gray);
    }

    public void RecommencerPartie() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void QuitterJeu() { Application.Quit(); }

    void MettreAJourUI()
    {
        if (texteScorePermanent != null) 
            texteScorePermanent.text = "SCORE: " + scoreTotal + " | TIRS: " + tirsEffectues + "/5";
    }

    void CreerInterfaceParCode()
    {
        // 1. SCORE FIXE
        GameObject goScore = new GameObject("CanvasScoreFixe");
        canvasScoreFixe = goScore;
        ConfigurerCanvas(goScore);
        GameObject fondS = new GameObject("Bandeau");
        fondS.transform.SetParent(goScore.transform);
        fondS.AddComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        RectTransform rtS = fondS.GetComponent<RectTransform>();
        rtS.anchorMin = new Vector2(0.5f, 1f); rtS.anchorMax = new Vector2(0.5f, 1f);
        rtS.sizeDelta = new Vector2(400, 50);
        rtS.anchoredPosition = new Vector2(0, -40);
        texteScorePermanent = CreerTexte(fondS.transform, "", 24, Vector2.zero);

        // 2. QUIZ
        GameObject goQuiz = new GameObject("CanvasQuiz");
        canvasQuiz = goQuiz;
        ConfigurerCanvas(goQuiz);
        GameObject fondQ = CreerFond(goQuiz.transform, new Vector2(800, 500));
        texteQuestion = CreerTexte(fondQ.transform, "Question", 40, new Vector2(0, 50));
        CreerBouton(fondQ.transform, "VRAI", new Vector2(-150, -100), () => ValiderReponse(true), Color.green);
        CreerBouton(fondQ.transform, "FAUX", new Vector2(150, -100), () => ValiderReponse(false), Color.red);
        canvasQuiz.SetActive(false);

        // 3. RÈGLES
        GameObject goRegles = new GameObject("CanvasRegles");
        canvasRegles = goRegles;
        ConfigurerCanvas(goRegles);
        GameObject fondR = CreerFond(goRegles.transform, new Vector2(900, 600));
        CreerTexte(fondR.transform, "RÈGLES DU JEU\n\n- Arrête les ballons en utilisant les flèches gauches et droite du clavier (400 pts)\n- Réponds au quiz (bonus)\n- Gagne en cumulant 1000 pts !", 30, new Vector2(0, 50));
        CreerBouton(fondR.transform, "COMMENCER", new Vector2(0, -180), () => BoutonCommencer(), new Color(1f, 0.5f, 0f));
    }

    void ConfigurerCanvas(GameObject go) {
        Canvas c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        go.AddComponent<GraphicRaycaster>();
    }
    GameObject CreerFond(Transform parent, Vector2 taille) {
        GameObject f = new GameObject("Fond");
        f.transform.SetParent(parent);
        f.AddComponent<Image>().color = new Color(0, 0, 0, 0.95f);
        f.GetComponent<RectTransform>().sizeDelta = taille;
        f.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return f;
    }
    TextMeshProUGUI CreerTexte(Transform parent, string contenu, int taille, Vector2 pos) {
        GameObject tGo = new GameObject("Texte");
        tGo.transform.SetParent(parent);
        TextMeshProUGUI t = tGo.AddComponent<TextMeshProUGUI>();
        t.text = contenu; t.fontSize = taille; t.alignment = TextAlignmentOptions.Center;
        RectTransform rt = tGo.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(750, 450);
        rt.anchoredPosition = pos;
        return t;
    }
    void CreerBouton(Transform parent, string nom, Vector2 pos, UnityEngine.Events.UnityAction action, Color coul) {
        GameObject bGo = new GameObject("Btn_" + nom);
        bGo.transform.SetParent(parent);
        bGo.AddComponent<Image>().color = coul;
        bGo.AddComponent<Button>().onClick.AddListener(action);
        GameObject tGo = new GameObject("Txt");
        tGo.transform.SetParent(bGo.transform);
        TextMeshProUGUI t = tGo.AddComponent<TextMeshProUGUI>();
        t.text = nom; t.color = Color.white; t.alignment = TextAlignmentOptions.Center; t.fontSize = 26;
        RectTransform rt = bGo.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(220, 80);
        rt.anchoredPosition = pos;
        RectTransform rtT = tGo.GetComponent<RectTransform>();
        rtT.anchorMin = Vector2.zero; rtT.anchorMax = Vector2.one; rtT.sizeDelta = Vector2.zero;
    }
}