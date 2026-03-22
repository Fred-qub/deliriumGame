using UnityEngine;
using System.Collections;

public class DemoInteractable : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Matthew's original fields — unchanged
    // -------------------------------------------------------------------------

    [Header("Settings")]
    public string objectName;       // e.g., "Verbal", "Sedative"
    public InteractionMaster.OutcomeType defaultOutcome;    

    [Header("Dependency System")]
    [Tooltip("Name of the object that must be used FIRST to make this a success.")]
    public string requiredObjectName;
    [Tooltip("Name of the object that blocks this object from being interacted with.")]
    public string blockerObjectName;
    
    [Header("Hallucinations")]
    public bool triggersHallucinationOnFail = true;
    
    private bool hasInteracted = false;
    
    // -------------------------------------------------------------------------
    // Events
    // -------------------------------------------------------------------------
    
    public delegate void Interaction(string name);
    public static event Interaction OnInteraction;
    

    // -------------------------------------------------------------------------
    // Dialogue fields
    // -------------------------------------------------------------------------

    [Header("Dialogue - Doctor POV")]
    [Tooltip("Optional. Only fill in if the doctor speaks before acting (Speak and Hearing Aid interactions only).")]
    public string doctorLine;

    [Tooltip("What Arthur says in response to this interaction.")]
    [TextArea] public string arthurLine;

    [Header("Dialogue - Patient POV Replay")]
    [Tooltip("Optional. Only fill in if the doctor speaks in this interaction (Speak and Hearing Aid interactions only).")]
    public string replayDoctorLine;

    [Tooltip("Arthur's internal monologue response during the patient POV replay.")]
    [TextArea] public string arthurMonologue;

    [Header("Hearing Aid Sequence")]
    [Tooltip("Check this ONLY for the Hearing Aid interaction. Enables the two-part doctor dialogue with garbled first line.")]
    public bool isHearingAidInteraction;

    [Tooltip("Hearing Aid only. The doctor's first line before the hearing aids are fitted (will be garbled in patient POV replay).")]
    [TextArea] public string doctorLineAfter;

    [Tooltip("Hearing Aid only. Arthur's response after the hearing aids are fitted.")]
    [TextArea] public string replayDoctorLineAfter;

    [Header("Hallucination Dialogue")]
    [Tooltip("Appended after arthurLine only if a hallucination was triggered on this interaction. " +
             "Use {hallucination} to insert 'rat' or 'snake' dynamically. " +
             "Example: 'Get that {hallucination} away from me!' " +
             "Leave blank on interactions that should never show a hallucination line.")]
    [TextArea] public string arthurHallucinationLine;

    // -------------------------------------------------------------------------
    // ExecuteChoice
    // -------------------------------------------------------------------------

    public void ExecuteChoice()
    {
        if (hasInteracted)
        {
            Debug.LogWarning($"{objectName} has already been used");
            return;
        }

        if (!string.IsNullOrEmpty(blockerObjectName))
        {
            if (InteractionMaster.Instance.HasInteractedWith(blockerObjectName))
            {
                Debug.LogWarning($"{blockerObjectName} has been used, {objectName} cannot be used");
                return;
            }
        }

        int choiceCount = InteractionMaster.Instance.successCount + InteractionMaster.Instance.failureCount + InteractionMaster.Instance.neutralCount;

        if (choiceCount >= InteractionMaster.Instance.maxInteractions)
        {
            Debug.Log("Game Over - Cannot interact further.");
            return;
        }

        InteractionMaster.OutcomeType finalOutcome = defaultOutcome;

        if (!string.IsNullOrEmpty(requiredObjectName))
        {
            bool conditionMet = InteractionMaster.Instance.HasInteractedWith(requiredObjectName);

            if (conditionMet)
            {
                Debug.Log($"Dependency met! Upgrading {objectName} to SUCCESS.");
                finalOutcome = InteractionMaster.OutcomeType.Success;
            }
        }

        hasInteracted = true;
        
        if (finalOutcome == InteractionMaster.OutcomeType.Failure && triggersHallucinationOnFail)
        {
            InteractionMaster.Instance.CheckHallucinationChance();
        }

        // RecordInteraction fires HallucinationTypeLottery() internally if this is a bad choice,
        // so by the time dialogue plays, GetHallucinationType() already has the correct result.
        InteractionMaster.Instance.RecordInteraction(objectName, finalOutcome);
        
        OnInteraction?.Invoke(objectName);
        
        // -------------------------------------------------------------------------
        // Trigger main dialogue — unchanged from before
        // -------------------------------------------------------------------------

        if (isHearingAidInteraction)
        {
            DialogueManager.Instance.ShowHearingAidSequence(
                doctorLine,
                doctorLineAfter,
                arthurLine,
                OnHearingAidAnimationTrigger
            );
        }
        else if (!string.IsNullOrEmpty(doctorLine))
        {
            DialogueManager.Instance.ShowDoctorThenArthur(doctorLine, arthurLine);
        }
        else
        {
            DialogueManager.Instance.ShowArthurLine(arthurLine);
        }

        // -------------------------------------------------------------------------
        // Append hallucination line after main dialogue completes,
        // but only if this interaction has a hallucination line set in the Inspector
        // AND a hallucination was actually assigned this run.
        // -------------------------------------------------------------------------

        if (!string.IsNullOrEmpty(arthurHallucinationLine))
        {
            StartCoroutine(AppendHallucinationLine());
        }
    }

    // -------------------------------------------------------------------------
    // Patient POV replay — unchanged from before
    // -------------------------------------------------------------------------

    public void ExecuteReplay()
    {
        if (isHearingAidInteraction)
        {
            DialogueManager.Instance.ShowHearingAidReplaySequence(
                replayDoctorLine,
                replayDoctorLineAfter,
                arthurMonologue,
                OnHearingAidAnimationTrigger
            );
        }
        else if (!string.IsNullOrEmpty(replayDoctorLine))
        {
            DialogueManager.Instance.ShowDoctorThenArthur(replayDoctorLine, arthurMonologue);
        }
        else
        {
            DialogueManager.Instance.ShowMonologue(arthurMonologue);
        }
    }

    // -------------------------------------------------------------------------
    // Hallucination append coroutine
    // Waits for the current dialogue to finish, then fires the hallucination
    // line only if a hallucination was actually assigned this run.
    // Safe to call on any interaction — silently does nothing if no hallucination.
    // -------------------------------------------------------------------------

    private IEnumerator AppendHallucinationLine()
    {
        // Wait for the main dialogue line to finish
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());

        string hallucinationType = InteractionMaster.Instance.GetHallucinationType();

        // Only fire if a hallucination was actually assigned this run
        if (string.IsNullOrEmpty(hallucinationType)) yield break;

        // Only fire once per run — prevents repeating on a second bad interaction
        if (InteractionMaster.Instance.hallucinationLineShown) yield break;
        InteractionMaster.Instance.hallucinationLineShown = true;

        string resolvedLine = arthurHallucinationLine.Replace("{hallucination}", hallucinationType);
        DialogueManager.Instance.ShowArthurLine(resolvedLine);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private void OnHearingAidAnimationTrigger()
    {
        DialogueManager.Instance.ContinueHearingAidDialogue();
    }

    public bool IsBlocked()
    {
        if (!string.IsNullOrEmpty(blockerObjectName))
            return InteractionMaster.Instance.HasInteractedWith(blockerObjectName);
        return false;
    }
}
