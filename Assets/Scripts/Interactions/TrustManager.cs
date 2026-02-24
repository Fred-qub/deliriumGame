using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InteractionMaster : MonoBehaviour
{
    public static InteractionMaster Instance { get; private set; }

    [Header("Game State")]
    // Tracks if specific objects have been used (Name -> True/False)
    public Dictionary<string, bool> objectActivationStates = new Dictionary<string, bool>();
    
    // Records the order of events
    public List<string> interactionHistory = new List<string>();

    [Header("Scoring")]
    public int successCount = 0;
    public int failureCount = 0;
    public int maxInteractions = 2; // Trigger result after this many choices
    
    [Header("Scene Management")]
    public string nextSceneName = "Scene_Replay"; //Name of next scene

    public float delayBeforeSwitch = 5.0f; //Time to read result before switching

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    /// <summary>
    /// Called by objects when interacted with.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="isSuccessAction">Is this a 'correct' action?</param>
    public void RecordInteraction(string objectName, bool isSuccessAction)
    {
        // 1. Prevent interacting if we already hit the limit
        if (interactionHistory.Count >= maxInteractions)
        {
            Debug.Log("Max interactions reached. Ignoring input.");
            return;
        }

        // 2. Record the Boolean State (True = Activated)
        if (!objectActivationStates.ContainsKey(objectName))
        {
            objectActivationStates.Add(objectName, true);
        }

        // 3. Record Order of Events
        interactionHistory.Add(objectName);

        // 4. Track Success/Failure
        if (isSuccessAction)
            successCount++;
        else
            failureCount++;

        // Debug Output for current state
        Debug.Log($"--- ACTION RECORDED ---");
        Debug.Log($"Object: {objectName} | Type: {(isSuccessAction ? "SUCCESS" : "FAILURE")}");
        Debug.Log($"Current History: {string.Join(" -> ", interactionHistory)}");

        // 5. Check if we reached the limit to show the result
        if (interactionHistory.Count >= maxInteractions)
        {
            CalculateFinalResult();
        }
    }
    
    public bool HasInteractedWith(string objectName)
    {
        // Checks our dictionary to see if this object exists and is true
        if (objectActivationStates.ContainsKey(objectName))
        {
            return objectActivationStates[objectName];
        }
        return false;
    }
    private void CalculateFinalResult()
    {
        Debug.Log("--- FINAL RESULT ---");
        
        if (successCount == 2)
        {
            Debug.Log("RESULT: TOTAL SUCCESS (Patient Calm)");
        }
        else if (failureCount == 2)
        {
            Debug.Log("RESULT: TOTAL FAILURE (Patient Upset)");
        }
        else
        {
            Debug.Log("RESULT: MIXED RESULT (Patient Mixed)");
        }
        
        //Start the scene change
        StartCoroutine(SwitchSceneRoutine());
    }

    System.Collections.IEnumerator SwitchSceneRoutine()
    {
        Debug.Log($"Switching to {nextSceneName} in {delayBeforeSwitch} seconds...");
        yield return new WaitForSeconds(delayBeforeSwitch);
        SceneManager.LoadScene(nextSceneName);
    }
}
