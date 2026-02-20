using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Holds all dialogue data for the patient POV replay scene.
/// Attach this to a GameObject in the patient scene (e.g. a "Managers" object).
///
/// SceneReplayer fires onTrigger events for each action — wire those events
/// to this script's ExecuteReplay() method, passing in the action name.
///
/// This script is intentionally independent of the doctor scene — it holds
/// its own copy of the dialogue data so the two scenes don't depend on each other.
/// </summary>
public class ReplayDialogue : MonoBehaviour
{
    public static ReplayDialogue Instance { get; private set; }

    // -------------------------------------------------------------------------
    // Replay Dialogue Entry
    // Each interaction gets one of these in the Inspector.
    // -------------------------------------------------------------------------

    [System.Serializable]
    public class ReplayEntry
    {
        [Tooltip("Must match the objectName in DemoInteractable and the actionName in SceneReplayer exactly.")]
        public string actionName;

        [Tooltip("Optional. Only fill in for Speak and Hearing Aid interactions.")]
        public string doctorLine;

        [Tooltip("Arthur's internal monologue response.")]
        [TextArea] public string arthurMonologue;

        [Header("Hearing Aid Only")]
        [Tooltip("Check this only for the Hearing Aid interaction.")]
        public bool isHearingAidInteraction;

        [Tooltip("Hearing Aid only. The doctor's second line after hearing aids are fitted (always clear).")]
        [TextArea] public string doctorLineAfter;
    }

    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Opening Scene Line")]
    [Tooltip("Arthur's opening line, replayed at the start of the patient POV scene.")]
    [TextArea] public string openingLine;

    [Tooltip("How long to wait before playing the opening line.")]
    [SerializeField] private float openingDelay = 2f;

    [Header("Replay Entries")]
    [Tooltip("Add one entry for each possible interaction. Action names must match exactly.")]
    public List<ReplayEntry> entries = new List<ReplayEntry>();

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Play Arthur's opening line at the start of the replay
        // so the player is reoriented into the scene before the interactions replay
        Invoke(nameof(PlayOpeningLine), openingDelay);
    }

    // -------------------------------------------------------------------------
    // Public API
    // Wire SceneReplayer's onTrigger events to this method, passing the action name.
    // -------------------------------------------------------------------------

    /// <summary>
    /// Called by SceneReplayer's onTrigger event for each replayed action.
    /// Looks up the matching entry and triggers the appropriate dialogue.
    /// </summary>
    public void ExecuteReplay(string actionName)
    {
        ReplayEntry entry = entries.Find(x => x.actionName == actionName);

        if (entry == null)
        {
            Debug.LogWarning($"[ReplayDialogue] No entry found for action: {actionName}");
            return;
        }

        if (entry.isHearingAidInteraction)
        {
            // Hearing Aid: garbled doctor line → animation → clear doctor line → Arthur monologue
            DialogueManager.Instance.ShowHearingAidReplaySequence(
                entry.doctorLine,
                entry.doctorLineAfter,
                entry.arthurMonologue,
                OnHearingAidAnimationTrigger
            );
        }
        else if (!string.IsNullOrEmpty(entry.doctorLine))
        {
            // Speak: doctor line → Arthur monologue
            DialogueManager.Instance.ShowDoctorThenArthur(entry.doctorLine, entry.arthurMonologue);
        }
        else
        {
            // All other interactions: Arthur monologue only
            DialogueManager.Instance.ShowMonologue(entry.arthurMonologue);
        }
    }

    // -------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------

    private void PlayOpeningLine()
    {
        DialogueManager.Instance.ShowMonologue(openingLine);
    }

    /// <summary>
    /// Hearing aid animation placeholder.
    /// TODO: Replace with actual animation trigger when animation is ready.
    /// Remove the immediate ContinueHearingAidDialogue() call and instead
    /// call it via an Animation Event at the end of the hearing aid animation clip.
    /// </summary>
    private void OnHearingAidAnimationTrigger()
    {
        DialogueManager.Instance.ContinueHearingAidDialogue();
    }
}
