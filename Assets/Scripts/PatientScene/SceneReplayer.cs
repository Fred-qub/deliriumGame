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
        public string actionName; // Must match the name used in Scene 1
        public UnityEvent onTrigger; 
    }

    [Header("Configuration")]
    public float startDelay = 2.0f;
    public float delayBetweenActions = 3.0f;

    [Header("Scene Transition")]
    public string nextSceneName = "EndScreen";

    [Header("The Actions Mapping")]
    
    public List<ReplayAction> actionLibrary; 

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
        // Get the history from the Master script
        if (InteractionMaster.Instance == null)
        {
            Debug.LogError("No InteractionMaster found");
            yield break;
        }

        List<string> history = InteractionMaster.Instance.interactionHistory;

        //Loop through every action the player took
        foreach (string actionName in history)
        {
            Debug.Log($"Replaying Event: {actionName}");

            // Find the matching event in library
            ReplayAction matchingAction = actionLibrary.Find(x => x.actionName == actionName);

            if (matchingAction.actionName != null)
            {
                //Start event
                matchingAction.onTrigger.Invoke();
            }
            else
            {
                Debug.LogWarning($"Could not find a replay definition for: {actionName}");
            }

            // Wait for dialogue to start, then wait for it to finish
            yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueActive);
            yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive);
        }

        Debug.Log("Replay Complete. Switching scenes.");
        //Switching Scenes
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(nextSceneName);
    }
}

