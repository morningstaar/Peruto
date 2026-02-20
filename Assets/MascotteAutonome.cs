using UnityEngine;

public class MascotteAutonome : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Animator animator;
    public float vitesseCourse = 3.0f;
    
    [Header("Cibles et Puissance")]
    public Transform[] ciblesPossibles; 
    public float forceInitiale = 18f;

    [Header("Réglages Timing (IMPORTANT)")]
    public float distanceDeTir = 1.1f; // Réglage conseillé : 1.1
    public float delaiImpactBalle = 0.35f; // Réglage conseillé : 0.35

    private bool enTrainDeCourir = false;
    private Vector3 positionDepartNaruto;
    private Rigidbody rbNaruto;

    void Start()
    {
        positionDepartNaruto = transform.position;
        animator = GetComponent<Animator>();
        rbNaruto = GetComponent<Rigidbody>();
        if (rbNaruto != null) rbNaruto.isKinematic = true; 
    }

    public void DemarrerCourse()
    {
        if (rbBallon == null) return;
        if (rbNaruto != null) rbNaruto.isKinematic = false;
        
        transform.position = positionDepartNaruto;
        enTrainDeCourir = true;
        animator.SetBool("isRunning", true); 
    }

    void Update()
    {
        if (enTrainDeCourir && rbBallon != null)
        {
            // Calcul de la direction sans l'axe Y pour éviter que Naruto ne penche
            Vector3 positionCible = new Vector3(rbBallon.transform.position.x, transform.position.y, rbBallon.transform.position.z);
            
            // Déplacement
            transform.position = Vector3.MoveTowards(transform.position, positionCible, vitesseCourse * Time.deltaTime);
            
            // Rotation fluide vers la balle
            Vector3 directionRegard = positionCible - transform.position;
            if (directionRegard != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(directionRegard);

            // Vérification de la distance
            if (Vector3.Distance(transform.position, positionCible) < distanceDeTir)
                DeclencherKick();
        }
    }

    void DeclencherKick()
    {
        enTrainDeCourir = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Kick"); 
        
        // On fige Naruto immédiatement pour une animation propre
        if (rbNaruto != null) {
            rbNaruto.velocity = Vector3.zero;
            rbNaruto.isKinematic = true;
        }

        // On lance le chrono pour l'impact physique
        Invoke("AppliquerForceBallon", delaiImpactBalle); 
    }

    public void AppliquerForceBallon()
    {
        if (rbBallon == null) return;
        rbBallon.isKinematic = false;
        
        if (ciblesPossibles != null && ciblesPossibles.Length > 0)
        {
            int index = Random.Range(0, ciblesPossibles.Length);
            Transform cibleChoisie = ciblesPossibles[index];

            // On récupère le score depuis le GameManager
            float forceActuelle = forceInitiale;
            if(GameManager.Instance != null)
                forceActuelle += (GameManager.Instance.tirsEffectues * 3f);

            Vector3 direction = (cibleChoisie.position - rbBallon.transform.position).normalized;
            rbBallon.AddForce(direction * forceActuelle, ForceMode.Impulse);
        }
    }
}