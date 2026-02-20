using UnityEngine;

public class DetecteurBut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ballon"))
        {
            Debug.Log("BUT encaissé !");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EnregistrerAction(false);
            }
        }
    }
}