using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InteractionMaster : MonoBehaviour
{
    public static InteractionMaster Instance { get; private set; }
    public HallucinationChance hallucinationChance;
 

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

        hallucinationChance = GetComponent<HallucinationChance>();
    }

    /// <summary>
    /// Called by objects when interacted with.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="isSuccessAction">Is this a 'correct' action?</param>
    public void RecordInteraction(string objectName, bool isSuccessAction)
    {
        // Prevent interacting if limit has been reached
        int totalChoices = successCount + failureCount;
        if (totalChoices >= maxInteractions)
        {
            Debug.Log("Max interactions reached. Ignoring input.");
            return;
        }

        // Boolean State (True = Activated)
        if (!objectActivationStates.ContainsKey(objectName))
        {
            objectActivationStates.Add(objectName, true);
        }

        // Order of Events
        interactionHistory.Add(objectName);

        // Success/Failure
        if (isSuccessAction)
        {
            successCount++;
        }

        if (!isSuccessAction && objectName != "Lights")
        {
            failureCount++;
            CheckHallucinationChance();
        }
            

        // Debug Output for current state
        Debug.Log($"--- ACTION RECORDED ---");
        Debug.Log($"Object: {objectName} | Type: {(isSuccessAction ? "SUCCESS" : "FAILURE")}");
        Debug.Log($"Current History: {string.Join(" -> ", interactionHistory)}");

        // Check if limit has been reached and show the result
        totalChoices = successCount + failureCount;
        if (totalChoices >= maxInteractions)
        {
            CalculateFinalResult();
        }

        
    }
    
    public bool HasInteractedWith(string objectName)
    {
        // Checks the dictionary to see if this object exists and is true
        if (objectActivationStates.ContainsKey(objectName))
        {
            return objectActivationStates[objectName];
        }
        return false;
    }

    public void ResetState()
    {
        interactionHistory.Clear();
        objectActivationStates.Clear();
        successCount = 0;
        failureCount = 0;
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
    
    public string GetFinalResultText()
    {
        if (successCount >= 2)
        {
            return "RESULT: TOTAL SUCCESS (Patient Calm)";
        }
        else if (failureCount >= 2)
        {
            return "RESULT: TOTAL FAILURE (Patient Upset)";
        }
        else
        {
            return "RESULT: MIXED RESULT (Patient Mixed)";
        }
    }

    System.Collections.IEnumerator SwitchSceneRoutine()
    {
        Debug.Log($"Switching to {nextSceneName} — waiting for dialogue to finish...");

        // Wait for dialogue to start (in case it hasn't yet)
        yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueActive());

        // Then wait for it to finish
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());

        // Small buffer so the last line doesn't feel abrupt
        yield return new WaitForSeconds(1.5f);

        Debug.Log($"Dialogue finished. Loading {nextSceneName}.");
        SceneManager.LoadScene(nextSceneName);
    }

    public void CheckHallucinationChance() 
    {

        hallucinationChance.AuxHallucinationLottery();   

    }
}
