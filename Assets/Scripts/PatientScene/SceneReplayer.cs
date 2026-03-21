using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneReplayer : MonoBehaviour
{
    [System.Serializable]
    public struct ReplayAction
    {
        public string actionName;
        public UnityEvent onTrigger;
    }

    [Header("Configuration")]
    public float startDelay = 2.0f;
    public float delayBetweenActions = 3.0f;

    [Header("Scene Transition")]
    public string nextSceneName = "TipsScene";

    [Header("The Actions Mapping")]
    public List<ReplayAction> actionLibrary;

    // -------------------------------------------------------------------------
    // Events
    // -------------------------------------------------------------------------

    public delegate void Interaction(string actionName);
    public static event Interaction OnInteraction;

    private void Start()
    {
        if (ReplayDialogue.Instance != null)
            ReplayDialogue.Instance.OnOpeningLineComplete += StartReplay;
        else
            StartCoroutine(PlayBackHistory());
    }

    private void StartReplay()
    {
        ReplayDialogue.Instance.OnOpeningLineComplete -= StartReplay;
        StartCoroutine(PlayBackHistory());
    }

    IEnumerator PlayBackHistory()
    {
        if (InteractionMaster.Instance == null)
        {
            Debug.LogError("No InteractionMaster found");
            yield break;
        }

        yield return new WaitForSeconds(startDelay);
        
        List<string> history = InteractionMaster.Instance.interactionHistory;

        
        
        foreach (string actionName in history)
        {
            Debug.Log($"Replaying Event: {actionName}");
            OnInteraction?.Invoke(actionName);
            ReplayAction matchingAction = actionLibrary.Find(x => x.actionName == actionName);

            if (!string.IsNullOrEmpty(matchingAction.actionName))
                matchingAction.onTrigger.Invoke();
            else
                Debug.LogWarning($"Could not find a replay definition for: {actionName}");

            //wait to see if an event triggered dialogue
            yield return new WaitForSeconds(0.2f);
            // Wait for dialogue to start, then wait for it to finish, if no dialogue wait a delay before next action
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
            {
                yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());
            }
            else
            {
                yield return new WaitForSeconds(delayBetweenActions);
            }
            
        }

        Debug.Log("Replay actions complete. Waiting for final dialogue to finish.");

        if (DialogueManager.Instance != null)
            yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());

        yield return new WaitForSeconds(1.5f);

        Debug.Log("Loading tips scene.");
        
        //Only use the first two choices not hallucinations
        List<string> choicesOnly = history.FindAll(x => x !="RatHallucination" && x !="SnakeHallucination");

        // Save choices for tips scene before loading
        string c1 = choicesOnly.Count > 0 ? choicesOnly[0] : "";
        string c2 = choicesOnly.Count > 1 ? choicesOnly[1] : "";
        TipsSceneManager.SaveChoices(c1, c2);
        SceneManager.LoadScene(nextSceneName);
    }
}