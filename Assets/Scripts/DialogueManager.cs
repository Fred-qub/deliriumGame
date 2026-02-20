using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages all dialogue display in both the doctor POV scene and the patient POV replay.
/// Attach this to a persistent GameObject in your scene (e.g. a "Managers" object).
///
/// Two display modes:
///   SPOKEN     - Speaker label shown, normal text, anchored bottom-centre.
///                Used for Arthur's spoken lines and the doctor's lines.
///                Auto-dismisses after displayDuration, or player can skip with E.
///
///   MONOLOGUE  - No speaker label, italic text, anchored centre-screen.
///                Used for Arthur's internal thoughts during the patient POV replay.
///                Auto-dismisses only — no player input required.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // -------------------------------------------------------------------------
    // Inspector References
    // Set these up by dragging your UI elements in from the Canvas hierarchy.
    // -------------------------------------------------------------------------

    [Header("Shared UI Elements")]
    [Tooltip("The root panel GameObject. We activate/deactivate this to show/hide dialogue.")]
    [SerializeField] private GameObject dialoguePanel;

    [Tooltip("The speaker label, e.g. 'Arthur:' or 'Doctor:'. Hidden during monologue.")]
    [SerializeField] private TMP_Text speakerLabel;

    [Tooltip("The main dialogue text body.")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Timing")]
    [Tooltip("How long spoken dialogue stays on screen before auto-dismissing (seconds).")]
    [SerializeField] private float displayDuration = 4f;

    [Tooltip("How long doctor lines stay on screen before auto-advancing to Arthur's line (seconds).")]
    [SerializeField] private float doctorLineDuration = 3f;

    [Tooltip("How long monologue lines stay on screen during the POV replay (seconds).")]
    [SerializeField] private float monoloagueDuration = 4f;

    [Header("Positioning")]
    [Tooltip("The RectTransform of the dialogue panel, used to reposition between modes.")]
    [SerializeField] private RectTransform dialoguePanelRect;

    [Tooltip("Anchored position for SPOKEN mode (bottom-centre). E.g. (0, 80).")]
    [SerializeField] private Vector2 spokenPosition = new Vector2(0f, 80f);

    [Tooltip("Anchored position for MONOLOGUE mode (centre-screen). E.g. (0, 0).")]
    [SerializeField] private Vector2 monologuePosition = new Vector2(0f, 0f);

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private Coroutine activeCoroutine;  // Tracks the currently running dialogue routine
    private bool playerSkipped;         // Set to true when player presses E to skip

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    private void Awake()
    {
        // Standard singleton setup — one instance persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Make sure the panel is hidden when the scene begins
        HideDialogue();
    }

    private void Update()
    {
        // Player can press E to skip the current spoken line
        // Note: monologue lines during the POV replay are auto-only and ignore this
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerSkipped = true;
        }
    }

    // -------------------------------------------------------------------------
    // Public API
    // These are the methods that DemoInteractable (and the opening scene) will call.
    // -------------------------------------------------------------------------

    /// <summary>
    /// Shows a single Arthur spoken line (normal mode, bottom-centre).
    /// Auto-dismisses after displayDuration, or player can press E to skip.
    /// </summary>
    public void ShowArthurLine(string line)
    {
        StartDialogue(ShowSpokenRoutine("Arthur", line));
    }

    /// <summary>
    /// Shows a doctor line, then automatically chains into an Arthur response.
    /// Used for the Speak and Hearing Aid interactions (clear doctor voice).
    /// </summary>
    public void ShowDoctorThenArthur(string doctorLine, string arthurLine)
    {
        StartDialogue(ShowDoctorThenArthurRoutine(doctorLine, arthurLine));
    }

    /// <summary>
    /// Shows the full hearing aid sequence:
    ///   1. Garbled doctor line (before hearing aids)
    ///   2. Fires the onHearingAidFitted callback so the animation can play
    ///   3. Clear doctor line (after hearing aids)
    ///   4. Arthur's response
    ///
    /// Pass in a System.Action callback that triggers your hearing aid animation.
    /// When the animation is done, call DialogueManager.Instance.ContinueHearingAidDialogue()
    /// via an Animation Event to resume the sequence.
    /// </summary>
    public void ShowHearingAidSequence(string garbledLine, string clearDoctorLine, string arthurLine, System.Action onHearingAidFitted)
    {
        StartDialogue(HearingAidSequenceRoutine(garbledLine, clearDoctorLine, arthurLine, onHearingAidFitted));
    }

    /// <summary>
    /// Shows Arthur's internal monologue during the patient POV replay.
    /// No speaker label. Italic text. Centre-screen position. Auto-dismiss only.
    /// </summary>
    public void ShowMonologue(string line)
    {
        StartDialogue(ShowMonologueRoutine(line));
    }

    /// <summary>
    /// Shows the hearing aid replay sequence from the patient's POV:
    ///   1. Garbled doctor line
    ///   2. Fires callback for animation
    ///   3. Clear doctor line
    ///   4. Arthur's internal monologue response
    /// </summary>
    public void ShowHearingAidReplaySequence(string garbledLine, string clearDoctorLine, string arthurMonologue, System.Action onHearingAidFitted)
    {
        StartDialogue(HearingAidReplaySequenceRoutine(garbledLine, clearDoctorLine, arthurMonologue, onHearingAidFitted));
    }

    /// <summary>
    /// Called by the hearing aid Animation Event once the animation has finished playing.
    /// Signals the waiting coroutine that it can continue to the next line.
    /// </summary>
    public void ContinueHearingAidDialogue()
    {
        hearingAidAnimationComplete = true;
    }

    // -------------------------------------------------------------------------
    // Hearing Aid Animation Handshake
    // The coroutine sets this to false before waiting, the Animation Event sets it true.
    // -------------------------------------------------------------------------

    private bool hearingAidAnimationComplete = false;

    // -------------------------------------------------------------------------
    // Private Coroutines
    // Each one handles a specific dialogue sequence.
    // -------------------------------------------------------------------------

    /// <summary>
    /// Displays a spoken line with a speaker label at the bottom of the screen.
    /// </summary>
    private IEnumerator ShowSpokenRoutine(string speaker, string line)
    {
        SetSpokenMode(speaker, line);

        playerSkipped = false;
        float elapsed = 0f;

        while (elapsed < displayDuration && !playerSkipped)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        HideDialogue();
    }

    /// <summary>
    /// Shows a doctor line that auto-advances, then shows Arthur's response.
    /// </summary>
    private IEnumerator ShowDoctorThenArthurRoutine(string doctorLine, string arthurLine)
    {
        // Doctor speaks first — auto-advances, no skip
        SetSpokenMode("Doctor", doctorLine);

        yield return new WaitForSeconds(doctorLineDuration);

        // Then Arthur responds — player can skip this one
        SetSpokenMode("Arthur", arthurLine);

        playerSkipped = false;
        float elapsed = 0f;

        while (elapsed < displayDuration && !playerSkipped)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        HideDialogue();
    }

    /// <summary>
    /// Full hearing aid sequence for the DOCTOR POV scene.
    /// Garbled line → wait for animation → clear line → Arthur responds.
    /// </summary>
    private IEnumerator HearingAidSequenceRoutine(string garbledLine, string clearDoctorLine, string arthurLine, System.Action onHearingAidFitted)
    {
        // Step 1: Show garbled doctor line (doctor POV — doctor hears themselves clearly,
        // but we show garbled text to foreshadow what the patient experiences)
        SetSpokenMode("Doctor", GarbleText(garbledLine));
        yield return new WaitForSeconds(doctorLineDuration);

        HideDialogue();

        // Step 2: Fire the animation callback
        // TODO: Trigger hearing aid animation here before continuing dialogue.
        // The animation should play between the garbled and clear doctor lines
        // so the POV replay can show the moment Arthur's hearing is restored.
        // Hook this up via an Animation Event on the clip that calls
        // DialogueManager.Instance.ContinueHearingAidDialogue() when finished.
        hearingAidAnimationComplete = false;
        onHearingAidFitted?.Invoke();

        // Step 3: Wait until the animation signals it's done
        while (!hearingAidAnimationComplete)
        {
            yield return null;
        }

        // Step 4: Doctor speaks clearly
        SetSpokenMode("Doctor", clearDoctorLine);
        yield return new WaitForSeconds(doctorLineDuration);

        // Step 5: Arthur responds
        SetSpokenMode("Arthur", arthurLine);

        playerSkipped = false;
        float elapsed = 0f;
        while (elapsed < displayDuration && !playerSkipped)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        HideDialogue();
    }

    /// <summary>
    /// Displays a monologue line during the patient POV replay.
    /// No speaker, italic, centre-screen, auto-dismiss only.
    /// </summary>
    private IEnumerator ShowMonologueRoutine(string line)
    {
        SetMonologueMode(line);
        yield return new WaitForSeconds(monoloagueDuration);
        HideDialogue();
    }

    /// <summary>
    /// Hearing aid sequence for the PATIENT POV replay.
    /// Garbled doctor line → animation → clear doctor line → Arthur monologue.
    /// </summary>
    private IEnumerator HearingAidReplaySequenceRoutine(string garbledLine, string clearDoctorLine, string arthurMonologue, System.Action onHearingAidFitted)
    {
        // Step 1: Garbled doctor line — patient cannot hear properly yet
        SetSpokenMode("Doctor", GarbleText(garbledLine));
        yield return new WaitForSeconds(doctorLineDuration);

        HideDialogue();

        // Step 2: Fire animation callback
        // TODO: Trigger hearing aid animation here before continuing dialogue.
        // Same animation as the doctor POV scene — this is the same moment
        // replayed from Arthur's perspective.
        // Animation Event should call DialogueManager.Instance.ContinueHearingAidDialogue()
        hearingAidAnimationComplete = false;
        onHearingAidFitted?.Invoke();

        while (!hearingAidAnimationComplete)
        {
            yield return null;
        }

        // Step 3: Doctor's line is now clear — hearing aids are in
        SetSpokenMode("Doctor", clearDoctorLine);
        yield return new WaitForSeconds(doctorLineDuration);

        HideDialogue();

        // Step 4: Arthur's internal response as monologue
        yield return StartCoroutine(ShowMonologueRoutine(arthurMonologue));
    }

    // -------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Stops any running dialogue coroutine and starts a new one.
    /// </summary>
    private void StartDialogue(IEnumerator routine)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }

    /// <summary>
    /// Configures the panel for SPOKEN mode (bottom-centre, speaker label visible).
    /// </summary>
    private void SetSpokenMode(string speaker, string line)
    {
        dialoguePanel.SetActive(true);
        dialoguePanelRect.anchoredPosition = spokenPosition;

        speakerLabel.gameObject.SetActive(true);
        speakerLabel.text = speaker + ":";

        dialogueText.fontStyle = FontStyles.Normal;
        dialogueText.text = line;
    }

    /// <summary>
    /// Configures the panel for MONOLOGUE mode (centre-screen, no speaker, italic).
    /// </summary>
    private void SetMonologueMode(string line)
    {
        dialoguePanel.SetActive(true);
        dialoguePanelRect.anchoredPosition = monologuePosition;

        speakerLabel.gameObject.SetActive(false);

        dialogueText.fontStyle = FontStyles.Italic;
        dialogueText.text = line;
    }

    /// <summary>
    /// Hides the dialogue panel.
    /// </summary>
    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Substitutes random characters into a string to simulate muffled/garbled hearing.
    /// Used for the pre-hearing-aid doctor line in the patient POV replay.
    /// </summary>
    private string GarbleText(string input)
    {
        char[] garbleChars = { 'ø', 'µ', '©', 'ß', '§', 'æ', 'ñ', 'ü' };
        char[] chars = input.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            // Garble roughly 1 in 4 letters, leaving spaces and punctuation intact
            if (char.IsLetter(chars[i]) && Random.Range(0, 4) == 0)
            {
                chars[i] = garbleChars[Random.Range(0, garbleChars.Length)];
            }
        }

        return new string(chars);
    }
}
