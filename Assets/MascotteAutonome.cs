using UnityEngine;

public class MascotteAutonome : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Transform cibleBut;
    public Animator animator;
    public float vitesseCourse = 3.0f;
    
    private bool enTrainDeCourir = false;
    private Vector3 positionDepartNaruto;

    void Start()
    {
        positionDepartNaruto = transform.position;
        animator = GetComponent<Animator>();
        Invoke("DemarrerCourse", 2f);
    }

    public void DemarrerCourse()
    {
        transform.position = positionDepartNaruto;
        enTrainDeCourir = true;
        animator.SetBool("isRunning", true); 
    }

    void Update()
    {
        if (enTrainDeCourir)
        {
            transform.position = Vector3.MoveTowards(transform.position, rbBallon.transform.position, vitesseCourse * Time.deltaTime);
            transform.LookAt(new Vector3(rbBallon.transform.position.x, transform.position.y, rbBallon.transform.position.z));

            if (Vector3.Distance(transform.position, rbBallon.transform.position) < 0.8f)
                TerminerCourseEtTirer();
        }
    }

    void TerminerCourseEtTirer()
    {
        enTrainDeCourir = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Kick"); 
        Invoke("AppliquerForceBallon", 0.2f);
    }

    void AppliquerForceBallon()
    {
        rbBallon.isKinematic = false;
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        rbBallon.AddForce(direction * 22f, ForceMode.Impulse);
    }
}