using UnityEngine;

public class ControleGardien : MonoBehaviour
{
    public float vitesse = 15f;
    
    [Header("Limites du But (Axe Z)")]
    // Modifie ces valeurs dans l'Inspecteur Unity
    public float limiteMinimum = -1.0f; 
    public float limiteMaximum = 1.0f;

    void Update()
    {
        // 1. On récupère l'entrée (Flèches Gauche/Droite)
        // Note : ajoute un "-" devant Input si les directions sont inversées
        float mouvement = -Input.GetAxis("Horizontal") * vitesse * Time.deltaTime;
        
        // 2. On calcule la position souhaitée sur l'axe Z
        float nouvellePositionZ = transform.position.z + mouvement;

        // 3. On bloque la position entre tes limites
        // IMPORTANT : limiteMinimum doit être plus petit que limiteMaximum
        nouvellePositionZ = Mathf.Clamp(nouvellePositionZ, limiteMinimum, limiteMaximum);

        // 4. On applique la position bloquée (X et Y restent identiques)
        transform.position = new Vector3(transform.position.x, transform.position.y, nouvellePositionZ);
    }
}