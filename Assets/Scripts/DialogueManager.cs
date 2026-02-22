using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages all dialogue display in both the doctor POV scene and the patient POV replay.
/// Attach this to a GameObject in each scene independently.
///
/// Two display modes:
///   SPOKEN     - Speaker label shown, normal text, anchored bottom-centre.
///                Used for Arthur's spoken lines and the doctor's lines.
///                Auto-dismisses after word-count-based duration, or player can press E to skip.
///                Doctor lines are garbled until hearing aids are fitted.
///
///   MONOLOGUE  - No speaker label, italic text, anchored centre-screen.
///                Used for Arthur's internal thoughts during the patient POV replay.
///                Auto-dismisses only — no player input required.
///
/// Improvements over v1:
///   - Doctor lines garbled until hearing aids fitted (checks InteractionMaster)
///   - Word count based display timing
///   - Typewriter effect — E skips to full line, second E dismisses
///   - Fade in/out on dialogue panel via CanvasGroup
///   - Portrait images per speaker
///   - Audio hooks for garbled, clear, and Arthur dialogue sounds
///   - Typewriter sound effect per character
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // -------------------------------------------------------------------------
    // Inspector References
    // -------------------------------------------------------------------------

    [Header("Shared UI Elements")]
    [Tooltip("The root panel GameObject. We activate/deactivate this to show/hide dialogue.")]
    [SerializeField] private GameObject dialoguePanel;

    [Tooltip("The speaker label e.g. 'Arthur:' or 'Doctor:'. Hidden during monologue.")]
    [SerializeField] private TMP_Text speakerLabel;

    [Tooltip("The main dialogue text body.")]
    [SerializeField] private TMP_Text dialogueText;

    [Tooltip("The CanvasGroup on the dialogue panel, used for fading in and out.")]
    [SerializeField] private CanvasGroup dialoguePanelCanvasGroup;

    [Tooltip("Portrait image displayed next to the speaker label. Hidden during monologue.")]
    [SerializeField] private Image portraitImage;

    [Header("Portrait Sprites")]
    [Tooltip("Portrait sprite for the doctor.")]
    [SerializeField] private Sprite doctorPortrait;

    [Tooltip("Portrait sprite for Arthur.")]
    [SerializeField] private Sprite arthurPortrait;

    [Header("Audio")]
    [Tooltip("AudioSource used to play dialogue sounds. Add an AudioSource component to this GameObject.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("Sound played when garbled doctor dialogue appears.")]
    [SerializeField] private AudioClip garbledSound;

    [Tooltip("Sound played when clear doctor dialogue appears after hearing aids are fitted.")]
    [SerializeField] private AudioClip clearSound;

    [Tooltip("Sound played when Arthur's spoken lines appear.")]
    [SerializeField] private AudioClip arthurSound;

    [Tooltip("Sound played per character during the typewriter effect. Use a very short clip (0.05-0.1 seconds).")]
    [SerializeField] private AudioClip typewriterSound;

    [Tooltip("Volume of the typewriter sound — keep this low (0.1 to 0.3) so it doesn't overpower dialogue.")]
    [SerializeField] [Range(0f, 1f)] private float typewriterVolume = 0.2f;

    [Header("Timing")]
    [Tooltip("Minimum time a line stays on screen regardless of word count (seconds).")]
    [SerializeField] private float minDisplayDuration = 2f;

    [Tooltip("Reading speed used to calculate display duration. 200 words per minute is average adult reading speed.")]
    [SerializeField] private float wordsPerMinute = 200f;

    [Header("Typewriter")]
    [Tooltip("How fast characters appear (characters per second).")]
    [SerializeField] private float typewriterSpeed = 40f;

    [Header("Fading")]
    [Tooltip("How long the fade in takes (seconds).")]
    [SerializeField] private float fadeInDuration = 0.2f;

    [Tooltip("How long the fade out takes (seconds).")]
    [SerializeField] private float fadeOutDuration = 0.3f;

    [Header("Positioning")]
    [Tooltip("The RectTransform of the dialogue panel, used to reposition between modes.")]
    [SerializeField] private RectTransform dialoguePanelRect;

    [Tooltip("Anchored position for SPOKEN mode (bottom-centre).")]
    [SerializeField] private Vector2 spokenPosition = new Vector2(0f, -400f);

    [Tooltip("Anchored position for MONOLOGUE mode (centre-screen).")]
    [SerializeField] private Vector2 monologuePosition = new Vector2(0f, 0f);

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private Coroutine activeCoroutine;
    private bool playerSkipped;     // First E press — skip typewriter to full line
    private bool playerDismissed;   // Second E press — dismiss line entirely
    private bool hearingAidAnimationComplete = false;

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
        HideDialogueImmediate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // First press skips typewriter to show full line
            // Second press dismisses the line
            if (!playerSkipped) playerSkipped = true;
            else playerDismissed = true;
        }
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Shows a single Arthur spoken line.
    /// Auto-dismisses after word-count duration, or player can press E to skip.
    /// </summary>
    public void ShowArthurLine(string line)
    {
        StartDialogue(ShowSpokenRoutine("Arthur", arthurPortrait, line, false));
    }

    /// <summary>
    /// Shows a doctor line then automatically chains into Arthur's response.
    /// Doctor line is garbled if hearing aids have not been fitted yet.
    /// </summary>
    public void ShowDoctorThenArthur(string doctorLine, string arthurLine)
    {
        StartDialogue(ShowDoctorThenArthurRoutine(doctorLine, arthurLine));
    }

    /// <summary>
    /// Shows the full hearing aid sequence:
    ///   1. Garbled doctor line (always garbled — this is the pre-fitting moment)
    ///   2. Fires animation callback
    ///   3. Clear doctor line
    ///   4. Arthur's response
    /// </summary>
    public void ShowHearingAidSequence(string doctorLineBefore, string doctorLineAfter, string arthurLine, System.Action onHearingAidFitted)
    {
        StartDialogue(HearingAidSequenceRoutine(doctorLineBefore, doctorLineAfter, arthurLine, onHearingAidFitted));
    }

    /// <summary>
    /// Shows Arthur's internal monologue during the patient POV replay.
    /// No speaker label. Italic. Centre-screen. Auto-dismiss only.
    /// </summary>
    public void ShowMonologue(string line)
    {
        StartDialogue(ShowMonologueRoutine(line));
    }

    /// <summary>
    /// Shows the hearing aid replay sequence from the patient's POV.
    /// Garbled doctor line → animation → clear doctor line → Arthur monologue.
    /// </summary>
    public void ShowHearingAidReplaySequence(string garbledLine, string clearDoctorLine, string arthurMonologue, System.Action onHearingAidFitted)
    {
        StartDialogue(HearingAidReplaySequenceRoutine(garbledLine, clearDoctorLine, arthurMonologue, onHearingAidFitted));
    }

    /// <summary>
    /// Called by the hearing aid Animation Event once the animation has finished.
    /// Signals the waiting coroutine to continue.
    /// </summary>
    public void ContinueHearingAidDialogue()
    {
        hearingAidAnimationComplete = true;
    }

    // -------------------------------------------------------------------------
    // Hearing Aid State
    // -------------------------------------------------------------------------

    /// <summary>
    /// Checks InteractionMaster to see if hearing aids have been fitted.
    /// If true, all doctor lines are displayed clearly.
    /// If false, all doctor lines are garbled.
    /// </summary>
    private bool HearingAidFitted()
    {
        if (InteractionMaster.Instance == null) return false;
        return InteractionMaster.Instance.HasInteractedWith("HearingAid");
    }

    /// <summary>
    /// Processes a doctor line before display.
    /// Returns garbled text and plays garbled sound if hearing aids not yet fitted.
    /// Returns clear text and plays clear sound if hearing aids have been fitted.
    /// </summary>
    private string ProcessDoctorLine(string line)
    {
        if (HearingAidFitted())
        {
            // TODO: Assign a real clear-voice audio clip to the clearSound field in the Inspector.
            // Suggestion: a soft tone or click to mark the improved hearing moment.
            PlaySound(clearSound);
            return line;
        }
        else
        {
            // TODO: Assign a real garbled/muffled audio clip to the garbledSound field in the Inspector.
            // Suggestion: a muffled or underwater speech effect from Freesound.org.
            PlaySound(garbledSound);
            return GarbleText(line);
        }
    }

    // -------------------------------------------------------------------------
    // Private Coroutines
    // -------------------------------------------------------------------------

    /// <summary>
    /// Core spoken line routine. Handles fade, portrait, typewriter, and skip/dismiss logic.
    /// isAutoAdvance: true = no player input, line auto-advances after duration (doctor lines).
    ///                false = player can skip typewriter with E, dismiss with second E.
    /// </summary>
    private IEnumerator ShowSpokenRoutine(string speaker, Sprite portrait, string line, bool isAutoAdvance)
    {
        SetSpokenMode(speaker, portrait, "");
        yield return StartCoroutine(FadeIn());

        // Play the appropriate character sound when their line appears
        if (speaker == "Arthur")
            PlaySound(arthurSound);


        playerSkipped = false;
        playerDismissed = false;

        yield return StartCoroutine(TypewriterRoutine(line));

        if (isAutoAdvance)
        {
            yield return new WaitForSeconds(CalculateDuration(line));
            yield return StartCoroutine(FadeOut());
            yield break;
        }

        // Wait for player to dismiss or auto-dismiss after word-count duration
        float elapsed = 0f;
        float duration = CalculateDuration(line);

        while (elapsed < duration && !playerDismissed)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Doctor line (auto-advance) followed by Arthur's response (player can dismiss).
    /// Doctor line is processed through ProcessDoctorLine — garbled or clear based on hearing aid state.
    /// </summary>
    private IEnumerator ShowDoctorThenArthurRoutine(string doctorLine, string arthurLine)
    {
        string processedLine = ProcessDoctorLine(doctorLine);
        yield return StartCoroutine(ShowSpokenRoutine("Doctor", doctorPortrait, processedLine, true));
        yield return StartCoroutine(ShowSpokenRoutine("Arthur", arthurPortrait, arthurLine, false));
    }

    /// <summary>
    /// Full hearing aid sequence for the doctor POV scene.
    /// The first doctor line is always garbled — this IS the pre-fitting moment,
    /// so we bypass ProcessDoctorLine and garble directly regardless of interaction order.
    /// The second line is always clear — hearing aids are now in.
    /// </summary>
    private IEnumerator HearingAidSequenceRoutine(string doctorLineBefore, string doctorLineAfter, string arthurLine, System.Action onHearingAidFitted)
    {
        // Step 1: Always garbled — bypass ProcessDoctorLine
        PlaySound(garbledSound);
        yield return StartCoroutine(ShowSpokenRoutine("Doctor", doctorPortrait, GarbleText(doctorLineBefore), true));

        // Step 2: Fire animation callback
        // TODO: Trigger hearing aid animation here.
        // Animation Event on the clip should call:
        // DialogueManager.Instance.ContinueHearingAidDialogue()
        hearingAidAnimationComplete = false;
        onHearingAidFitted?.Invoke();

        while (!hearingAidAnimationComplete)
            yield return null;

        // Step 3: Always clear — hearing aids now in
        PlaySound(clearSound);
        yield return StartCoroutine(ShowSpokenRoutine("Doctor", doctorPortrait, doctorLineAfter, true));

        // Step 4: Arthur responds — only if a line was provided
        // The hearing aid interaction has no spoken Arthur response in the doctor POV scene
        if (!string.IsNullOrEmpty(arthurLine))
        yield return StartCoroutine(ShowSpokenRoutine("Arthur", arthurPortrait, arthurLine, false));
    }

    /// <summary>
    /// Monologue routine for patient POV replay.
    /// No portrait, italic, centre-screen, auto-dismiss only.
    /// </summary>
    private IEnumerator ShowMonologueRoutine(string line)
    {
        SetMonologueMode("");
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(TypewriterRoutine(line));
        yield return new WaitForSeconds(CalculateDuration(line));
        yield return StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Hearing aid replay sequence for the patient POV scene.
    /// </summary>
    private IEnumerator HearingAidReplaySequenceRoutine(string garbledLine, string clearDoctorLine, string arthurMonologue, System.Action onHearingAidFitted)
    {
        // Step 1: Garbled — patient cannot hear properly yet
        PlaySound(garbledSound);
        yield return StartCoroutine(ShowSpokenRoutine("Doctor", doctorPortrait, GarbleText(garbledLine), true));

        // Step 2: Fire animation callback
        // TODO: Same animation as doctor POV scene, from Arthur's perspective.
        // Animation Event should call DialogueManager.Instance.ContinueHearingAidDialogue()
        hearingAidAnimationComplete = false;
        onHearingAidFitted?.Invoke();

        while (!hearingAidAnimationComplete)
            yield return null;

        // Step 3: Clear — hearing aids now in
        PlaySound(clearSound);
        yield return StartCoroutine(ShowSpokenRoutine("Doctor", doctorPortrait, clearDoctorLine, true));

        // Step 4: Arthur's internal monologue
        yield return StartCoroutine(ShowMonologueRoutine(arthurMonologue));
    }

    // -------------------------------------------------------------------------
    // Typewriter
    // -------------------------------------------------------------------------

    /// <summary>
    /// Reveals the line character by character at typewriterSpeed.
    /// Player can press E to skip to the full line immediately.
    /// </summary>
    private IEnumerator TypewriterRoutine(string fullLine)
    {
        dialogueText.text = "";
        float delay = 1f / typewriterSpeed;

        for (int i = 0; i < fullLine.Length; i++)
        {
            if (playerSkipped)
            {
                dialogueText.text = fullLine;
                yield break;
            }

            dialogueText.text += fullLine[i];

            // Play typewriter tick for letters and numbers only — skip spaces and punctuation
            // so the sound doesn't fire on every character and become too rapid or uneven
            if (char.IsLetterOrDigit(fullLine[i]) && typewriterSound != null && audioSource != null)
                audioSource.PlayOneShot(typewriterSound, typewriterVolume);

            yield return new WaitForSeconds(delay);
        }
    }

    // -------------------------------------------------------------------------
    // Fading
    // -------------------------------------------------------------------------

    private IEnumerator FadeIn()
    {
        dialoguePanel.SetActive(true);
        dialoguePanelCanvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            dialoguePanelCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }

        dialoguePanelCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            dialoguePanelCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeOutDuration));
            yield return null;
        }

        HideDialogueImmediate();
    }

    // -------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------

private void StartDialogue(IEnumerator routine)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            HideDialogueImmediate(); // Reset panel state cleanly before starting new dialogue
        }

        activeCoroutine = StartCoroutine(routine);
    }

    /// <summary>
    /// Calculates display duration based on word count and reading speed.
    /// Always respects the minimum display duration.
    /// </summary>
    private float CalculateDuration(string line)
    {
        int wordCount = line.Split(' ').Length;
        float duration = (wordCount / wordsPerMinute) * 60f;
        return Mathf.Max(minDisplayDuration, duration);
    }

    private void SetSpokenMode(string speaker, Sprite portrait, string line)
    {
        dialoguePanelRect.anchoredPosition = spokenPosition;

        speakerLabel.gameObject.SetActive(true);
        speakerLabel.text = speaker + ":";

        if (portraitImage != null)
        {
            portraitImage.gameObject.SetActive(portrait != null);
            if (portrait != null) portraitImage.sprite = portrait;
        }

        dialogueText.fontStyle = FontStyles.Normal;
        dialogueText.text = line;
    }

    private void SetMonologueMode(string line)
    {
        dialoguePanelRect.anchoredPosition = monologuePosition;

        speakerLabel.gameObject.SetActive(false);

        if (portraitImage != null)
        {
            portraitImage.gameObject.SetActive(arthurPortrait != null);
            if (arthurPortrait != null) portraitImage.sprite = arthurPortrait;
        }

        dialogueText.fontStyle = FontStyles.Italic;
        dialogueText.text = line;
    }

    private void HideDialogueImmediate()
    {
        if (dialoguePanelCanvasGroup != null)
            dialoguePanelCanvasGroup.alpha = 0f;

        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Plays a sound if both AudioSource and clip are assigned.
    /// Safe to call with a null clip — does nothing, no error thrown.
    /// This is intentional so audio slots can be left empty as placeholders.
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Substitutes random characters to simulate garbled/muffled hearing.
    /// Spaces and punctuation are preserved so the sentence shape remains readable.
    /// </summary>
    private string GarbleText(string input)
    {
        char[] garbleChars = { 'ø', 'µ', '©', 'ß', '§', 'æ', 'ñ', 'ü' };
        char[] chars = input.ToCharArray();

        int letterCount = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsLetter(chars[i]))
            {
                letterCount++;
                if (letterCount % 5 == 0)
                    chars[i] = garbleChars[Random.Range(0, garbleChars.Length)];
            }
        }

        return new string(chars);
    }
}
