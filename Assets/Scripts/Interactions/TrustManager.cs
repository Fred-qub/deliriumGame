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
    public string nextSceneName = "Scene_Replay"; // Name of next scene
    public float delayBeforeSwitch = 5.0f; // Time to read result before switching

    // Prevents the hallucination dialogue line from firing more than once per run.
    // Reset in ResetState() so each playthrough starts clean.
    [HideInInspector] public bool hallucinationLineShown = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        hallucinationChance = GetComponent<HallucinationChance>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Automatically resets all game state when the clinician scene loads.
    /// This ensures a clean slate whether arriving via Play Again or directly from the editor.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Clinician Scene Ruth")
        {
            ResetState();
            Debug.Log("[InteractionMaster] State reset for new playthrough.");
        }
    }

    /// <summary>
    /// Called by objects when interacted with.
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    /// <param name="isSuccessAction">Is this a 'correct' action?</param>
    public void RecordInteraction(string objectName, bool isSuccessAction)
    {
        // Count only real player choices — hallucination entries added by HallucinationChance
        // are excluded so they never count toward the interaction limit.
        int totalChoices = interactionHistory.FindAll(x =>
            x != "RatHallucination" && x != "SnakeHallucination").Count;

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

        // CalculateFinalResult via the totalChoices count below.
        if (!isSuccessAction)
        {
            failureCount++;
            CheckHallucinationChance();
        }

        // Debug Output for current state
        Debug.Log($"--- ACTION RECORDED ---");
        Debug.Log($"Object: {objectName} | Type: {(isSuccessAction ? "SUCCESS" : "FAILURE")}");
        Debug.Log($"Current History: {string.Join(" -> ", interactionHistory)}");

        // Recount after adding the new interaction — still excluding hallucination entries
        totalChoices = interactionHistory.FindAll(x =>
            x != "RatHallucination" && x != "SnakeHallucination").Count;

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
        hallucinationLineShown = false;
        Debug.Log("[InteractionMaster] ResetState called — history and scores cleared.");
    }

    /// <summary>
    /// Returns "rat", "snake", or empty string if no hallucination assigned yet.
    /// Called by DemoInteractable and ReplayDialogue to inject the correct
    /// hallucination word into Arthur's dialogue at runtime via the {hallucination} token.
    /// </summary>
    public string GetHallucinationType()
    {
        if (interactionHistory.Contains("RatHallucination"))
            return PlayerPrefs.GetInt("Musophobia", 0) == 1 ? "thing" : "rat";
        if (interactionHistory.Contains("SnakeHallucination")) return "snake";
        return "";
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

        // Start the scene change
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

        // Wait for it to finish
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());

        // Brief pause to catch any appended dialogue (e.g. hallucination line)
        // which fires after a short gap following the main line
        yield return new WaitForSeconds(0.3f);
        if (DialogueManager.Instance.IsDialogueActive())
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
