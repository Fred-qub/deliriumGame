using UnityEngine;

public class DoctorAnimationStateController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            Debug.Log("Animator unassigned, set via getcomponent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startWalking()
    {
        animator.SetBool("IsWalking", true);
    }
    
    public void stopWalking()
    {
        animator.SetBool("IsWalking", false);
    }

    public void triggerInteract()
    {
        animator.SetTrigger("InteractTrigger");
    }
}
