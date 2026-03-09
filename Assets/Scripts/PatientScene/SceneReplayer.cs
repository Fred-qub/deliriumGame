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

        List<string> history = InteractionMaster.Instance.interactionHistory;

        foreach (string actionName in history)
        {
            Debug.Log($"Replaying Event: {actionName}");

            ReplayAction matchingAction = actionLibrary.Find(x => x.actionName == actionName);

            if (matchingAction.actionName != null)
                matchingAction.onTrigger.Invoke();
            else
                Debug.LogWarning($"Could not find a replay definition for: {actionName}");

            // Wait for dialogue to start, then wait for it to finish
            yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueActive());
            yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());
        }

        Debug.Log("Replay actions complete. Waiting for final dialogue to finish.");

        if (DialogueManager.Instance != null)
            yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());

        yield return new WaitForSeconds(1.5f);

        Debug.Log("Loading tips scene.");

        // Save choices for tips scene before loading
        string c1 = history.Count > 0 ? history[0] : "";
        string c2 = history.Count > 1 ? history[1] : "";
        TipsSceneManager.SaveChoices(c1, c2);
        SceneManager.LoadScene(nextSceneName);
    }
}