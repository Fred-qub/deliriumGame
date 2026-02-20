using UnityEngine;

public class DemoInteractable : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Matthew's original fields — unchanged
    // -------------------------------------------------------------------------

    [Header("Settings")]
    public string objectName;       // e.g., "Verbal", "Sedative"
    public bool isSuccessOption;    // Check this box if this is a "Good" choice

    [Header("Dependency System")]
    [Tooltip("Name of the object that must be used FIRST to make this a success.")]
    public string requiredObjectName;

    private bool hasInteracted = false;

    // -------------------------------------------------------------------------
    // New dialogue fields
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

    // -------------------------------------------------------------------------
    // Matthew's original ExecuteChoice — with dialogue calls added at the end
    // -------------------------------------------------------------------------

    public void ExecuteChoice()
    {
        // Check if already used
        if (hasInteracted)
        {
            Debug.LogWarning($"{objectName} has already been used");
            return;
        }

        // Check if Master allows more interactions
        if (InteractionMaster.Instance.interactionHistory.Count >= InteractionMaster.Instance.maxInteractions)
        {
            Debug.Log("Game Over - Cannot interact further.");
            return;
        }

        // Determine the final result
        bool finalOutcome = isSuccessOption;

        // Check dependency
        if (!string.IsNullOrEmpty(requiredObjectName))
        {
            bool conditionMet = InteractionMaster.Instance.HasInteractedWith(requiredObjectName);

            if (conditionMet)
            {
                Debug.Log($"[DEPENDENCY MET] {requiredObjectName} was used. Changing {objectName} to SUCCESS.");
                finalOutcome = true;
            }
            else
            {
                Debug.Log($"[DEPENDENCY FAILED] {requiredObjectName} was NOT used. {objectName} remains {(finalOutcome ? "SUCCESS" : "FAILURE")}.");
            }
        }

        // Mark as used and notify master — Matthew's original logic, unchanged
        hasInteracted = true;
        InteractionMaster.Instance.RecordInteraction(objectName, finalOutcome);

        // -------------------------------------------------------------------------
        // Trigger dialogue based on which interaction this is
        // -------------------------------------------------------------------------

        if (isHearingAidInteraction)
        {
            // Hearing Aid: garbled doctor line → animation → clear doctor line → Arthur
            // doctorLine        = the doctor's first line (before hearing aids, shown garbled in replay)
            // doctorLineAfter   = the doctor's second line (after hearing aids, always clear)
            // arthurLine        = Arthur's spoken response
            DialogueManager.Instance.ShowHearingAidSequence(
                doctorLine,
                doctorLineAfter,
                arthurLine,
                OnHearingAidAnimationTrigger
            );
        }
        else if (!string.IsNullOrEmpty(doctorLine))
        {
            // Speak interaction: doctor line → Arthur responds
            DialogueManager.Instance.ShowDoctorThenArthur(doctorLine, arthurLine);
        }
        else
        {
            // All other interactions: Arthur responds only (Coat, Lights, Sedate)
            DialogueManager.Instance.ShowArthurLine(arthurLine);
        }
    }

    // -------------------------------------------------------------------------
    // New method for the patient POV replay
    // Call this instead of ExecuteChoice() when replaying from the patient's perspective
    // -------------------------------------------------------------------------

    public void ExecuteReplay()
    {
        if (isHearingAidInteraction)
        {
            // Hearing Aid replay: garbled doctor line → animation → clear doctor line → Arthur monologue
            // replayDoctorLine      = same first doctor line (will be garbled)
            // replayDoctorLineAfter = same second doctor line (clear)
            // arthurMonologue       = Arthur's internal response
            DialogueManager.Instance.ShowHearingAidReplaySequence(
                replayDoctorLine,
                replayDoctorLineAfter,
                arthurMonologue,
                OnHearingAidAnimationTrigger
            );
        }
        else if (!string.IsNullOrEmpty(replayDoctorLine))
        {
            // Speak replay: doctor line → Arthur internal monologue
            DialogueManager.Instance.ShowDoctorThenArthur(replayDoctorLine, arthurMonologue);
        }
        else
        {
            // All other interactions: Arthur internal monologue only
            DialogueManager.Instance.ShowMonologue(arthurMonologue);
        }
    }

    // -------------------------------------------------------------------------
    // Hearing aid animation trigger
    // This is the callback passed to the DialogueManager to fire the animation.
    // For now it immediately signals the animation is complete so dialogue continues.
    //
    // TODO: Replace the body of this method with your actual animation trigger,
    // e.g. GetComponent<Animator>().SetTrigger("FitHearingAid");
    // Then remove the immediate ContinueHearingAidDialogue() call below,
    // and instead call it via an Animation Event at the end of the animation clip.
    // -------------------------------------------------------------------------
    private void OnHearingAidAnimationTrigger()
    {
        // Placeholder — signals dialogue to continue immediately until animation exists
        DialogueManager.Instance.ContinueHearingAidDialogue();
    }
}
