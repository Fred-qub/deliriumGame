using UnityEngine;

public class DemoInteractable : MonoBehaviour
{
    [Header("Settings")]
    public string objectName; // e.g., "Verbal", "Sedative"
    public bool isSuccessOption; // Check this box if this is a "Good" choice

    [Header("Dependency System")]
    [Tooltip("Name of the object that must be used FIRST to make this a success.")]
    public string requiredObjectName; // e.g., "Hearing Aid"
    
    private bool hasInteracted = false;
    

    public void ExecuteChoice()
    {
        // Check if already used
        if (hasInteracted)
        {
            Debug.LogWarning($"{objectName} has already been used");
            return;
        }

        // Check if Master allows more use
        if (InteractionMaster.Instance.interactionHistory.Count >= InteractionMaster.Instance.maxInteractions)
        {
            Debug.Log("Game Over - Cannot interact further.");
            return;
        }
        
        // Determine the final result
        bool finalOutcome = isSuccessOption;

        // Check if a dependency is requirement
        if (!string.IsNullOrEmpty(requiredObjectName))
        {
            // Ask Master if the required object been used yet
            bool conditionMet = InteractionMaster.Instance.HasInteractedWith(requiredObjectName);

            if (conditionMet)
            {
                Debug.Log($"[DEPENDENCY MET] {requiredObjectName} was used. Changing {objectName} to SUCCESS.");
                finalOutcome = true; // Flip to Success
            }
            else
            {
                Debug.Log($"[DEPENDENCY FAILED] {requiredObjectName} was NOT used. {objectName} remains {(finalOutcome ? "SUCCESS" : "FAILURE")}.");
            }
        }

        // Execute Interaction
        hasInteracted = true;
        
        // Notify Master Script
        InteractionMaster.Instance.RecordInteraction(objectName, finalOutcome);
    }
}
