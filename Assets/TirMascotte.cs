using UnityEngine;

public class TirMascotte : MonoBehaviour
{
    public Rigidbody rbBallon;
    private Vector3 positionInitiale;

    void Start()
    {
        if (rbBallon != null) positionInitiale = rbBallon.transform.position;
    }

    public void ReplacerBallon()
    {
        rbBallon.velocity = Vector3.zero;
        rbBallon.angularVelocity = Vector3.zero;
        rbBallon.isKinematic = true;
        rbBallon.transform.position = positionInitiale;
    }
}