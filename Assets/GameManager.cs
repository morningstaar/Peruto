using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuration")]
    public int scoreObjectif = 1000;
    private int tirsEffectues = 0;
    private int scoreTotal = 0;
    private bool dernierTirArrete = false;

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
        canvasQuiz.SetActive(true);
        texteQuestion.text = "Tir n°" + (tirsEffectues + 1) + " terminé.\nVoulez-vous doubler vos points ?";
    }

    public void ValiderReponse(bool bonneReponse)
    {
        if (bonneReponse && dernierTirArrete)
        {
            scoreTotal += 400; // Bonus pour faire 800 total (400 arrêt + 400 bonus)
        }

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
        if (naruto != null) naruto.Invoke("DemarrerCourse", 0.5f);
    }

    void TerminerJeu()
    {
        canvasQuiz.SetActive(true);
        string message = scoreTotal >= scoreObjectif ? "GAGNÉ !" : "PERDU...";
        texteQuestion.text = message + "\nScore Final : " + scoreTotal;
        foreach(Button b in canvasQuiz.GetComponentsInChildren<Button>()) b.gameObject.SetActive(false);
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

        GameObject scoreGo = new GameObject("ScoreTxt");
        scoreGo.transform.SetParent(fond.transform);
        texteScoreUI = scoreGo.AddComponent<TextMeshProUGUI>();
        texteScoreUI.fontSize = 30;
        texteScoreUI.alignment = TextAlignmentOptions.Center;
        RectTransform rtS = scoreGo.GetComponent<RectTransform>();
        rtS.anchorMin = rtS.anchorMax = new Vector2(0.5f, 1f);
        rtS.sizeDelta = new Vector2(600, 50);
        rtS.anchoredPosition = new Vector2(0, -50);

        GameObject qGo = new GameObject("QuestionTxt");
        qGo.transform.SetParent(fond.transform);
        texteQuestion = qGo.AddComponent<TextMeshProUGUI>();
        texteQuestion.alignment = TextAlignmentOptions.Center;
        texteQuestion.fontSize = 40;
        RectTransform rtQ = qGo.GetComponent<RectTransform>();
        rtQ.anchorMin = rtQ.anchorMax = new Vector2(0.5f, 0.5f);
        rtQ.sizeDelta = new Vector2(700, 150);
        rtQ.anchoredPosition = new Vector2(0, 50);

        CreerBouton(fond.transform, "OUI", new Vector2(-150, -100), () => ValiderReponse(true), Color.green);
        CreerBouton(fond.transform, "NON", new Vector2(150, -100), () => ValiderReponse(false), Color.red);
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