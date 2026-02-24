using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    //how far away the player can interact with an object
    public float range = 3f;
    //what the player is trying to interact with
    interactableObject target;

    public Camera camera;
    
    void Update()
    {
        CheckInteraction();
        
        //E as interact seems intuitive
        //make sure you're not looking at nothing
        if (Input.GetKeyDown(KeyCode.E) && target != null)
        {
            target.Interact();
        }
    }

    void CheckInteraction()
    {
        //draws a raycast from the camera
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(camera.transform.position, camera.transform.forward, Color.red);
        
        //if the ray hits something within range and the collider it hits is an interactable
        if (Physics.Raycast(ray, out hit, range) && hit.collider.CompareTag("Interactable"))
        { 
            //get the interactable object script
            interactableObject interactable = hit.collider.GetComponent<interactableObject>();
            
            //if it's enabled set it as the target, otherwise target nothing
            if (interactable.enabled) setTarget(interactable);
            else clearTarget();
        }
        //otherwise you're targeting nothing
        else clearTarget();
    }

    //self explanatory
    void setTarget(interactableObject interactable)
    {
        target = interactable;
        HUDmanager.instance.enableInteractionText(target.prompt);
    }
    
    //if there's still a target set, get rid of it
    void clearTarget()
    {
        HUDmanager.instance.disableInteractionText();
        if (target) target = null;
    }
}
