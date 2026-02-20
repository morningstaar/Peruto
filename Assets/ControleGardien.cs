using UnityEngine;

public class ControleGardien : MonoBehaviour
{
    public float vitesse = 15f;
    
    [Header("Limites du But (Axe Z)")]
    public float limiteMinimum = 0.05f;
    public float limiteMaximum = -0.05f;

    void Update()
    {
        // 1. On récupère l'entrée (Flèches Gauche/Droite ou Q/D)
        float mouvement = -Input.GetAxis("Horizontal") * vitesse * Time.deltaTime;
        
        // 2. On calcule la nouvelle position théorique sur l'axe Z
        float nouvellePositionZ = transform.position.z + mouvement;

        // 3. On bloque la position entre les poteaux
        nouvellePositionZ = Mathf.Clamp(nouvellePositionZ, limiteMinimum, limiteMaximum);

        // 4. On applique la position (X et Y restent fixes, seul Z change)
        transform.position = new Vector3(transform.position.x, transform.position.y, nouvellePositionZ);
    }
}