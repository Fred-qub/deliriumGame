using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    //how far away the player can interact with an object
    public float range = 3f;
    //what the player is trying to interact with
    interactableObject target;

    public Camera camera2;
    
    void Update()
    {
        CheckInteraction();
        
        //E as interact seems intuitive
        //make sure you're not looking at nothing
        if (Input.GetKeyDown(KeyCode.E) && target != null)
        {
            int totalChoices = InteractionMaster.Instance.successCount + InteractionMaster.Instance.failureCount;
            if (totalChoices >= InteractionMaster.Instance.maxInteractions)
            {
                return;
            }
            
            DemoInteractable obj = target.interactionLink;
            if (obj != null)
            {
                bool isBlocked = obj.IsBlocked();
                bool isAlreadyUsed = InteractionMaster.Instance.HasInteractedWith(obj.objectName);

                if (isBlocked || isAlreadyUsed)
                {
                    return;
                }
            }
            
            target.Interact();
        }
    }

    void CheckInteraction()
    {
        //checks if dialogue is active and hides the prompt
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            clearTarget();
            return;
        }
        
        //draws a raycast from the camera2
        RaycastHit hit;
        Ray ray = new Ray(camera2.transform.position, camera2.transform.forward);
        Debug.DrawRay(camera2.transform.position, camera2.transform.forward, Color.red);
        
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

    
    void setTarget(interactableObject interactable)
    {
        target = interactable;
        DemoInteractable obj = interactable.interactionLink;
        
        int totalChoices = InteractionMaster.Instance.successCount + InteractionMaster.Instance.failureCount;

        if (totalChoices >= InteractionMaster.Instance.maxInteractions)
        {
            HUDmanager.instance.enableMaxInteractionText("<color=orange>Max interactions reached (2/2)</color>");
        }
        else if (obj != null)
        {
            //Check if Blocked OR Already Used
            bool isBlocked = obj.IsBlocked();
            bool isAlreadyUsed = InteractionMaster.Instance.HasInteractedWith(obj.objectName);

            if (isBlocked || isAlreadyUsed)
            {
                HUDmanager.instance.disableInteractionText();
            }
            else
            {
                HUDmanager.instance.enableInteractionText(target.prompt);   
            }
        }
        else
        {
            HUDmanager.instance.enableInteractionText(target.prompt);   
        }
    }
    
    //if there's still a target set, get rid of it
    void clearTarget()
    {
        HUDmanager.instance.disableInteractionText();
        if (target) target = null;
    }
}