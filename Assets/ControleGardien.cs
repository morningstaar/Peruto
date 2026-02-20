using UnityEngine;

public class ControleGardien : MonoBehaviour
{
    public float vitesse = 15f;

    void Update()
    {
        // On récupère les touches Gauche/Droite ou Q/D
        float mouvement = Input.GetAxis("Horizontal") * vitesse * Time.deltaTime;
        
        // IMPORTANT : On déplace sur l'axe X pour glisser devant le but
        transform.Translate(mouvement, 0, 0);
    }
}