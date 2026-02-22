using UnityEngine;
using UnityEngine.InputSystem; 

public class ForceHands : MonoBehaviour 
{
    // Ce champ va apparaître dans ton Inspecteur
    public InputActionAsset actions; 

    void OnEnable() 
    { 
        if (actions != null) 
        {
            actions.Enable(); 
            Debug.Log("Les Input Actions ont été forcées à s'activer !");
        }
        else 
        {
            Debug.LogError("Attention : Tu as oublié de glisser le fichier XRI Default Input Actions dans le script ForceHands !");
        }
    }
}