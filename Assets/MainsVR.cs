using UnityEngine;

public class MainsVR : MonoBehaviour 
{
    public GameObject effetConfettis;

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Ballon")) 
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EnregistrerAction(true);
            }

            if (effetConfettis != null) 
            {
                Instantiate(effetConfettis, transform.position, Quaternion.identity);
            }
        }
    }
}