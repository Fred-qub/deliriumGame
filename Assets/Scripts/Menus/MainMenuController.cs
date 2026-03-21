using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Controls the MainMenu scene UI for Misperception.
/// 
/// Mirrors the pattern used in TipsSceneManager:
///   - UIDocument queried in OnEnable via rootVisualElement.Q<>()
///   - Buttons wired with .clicked += delegates
///   - SceneManager.LoadScene() used for scene transitions
///   - No legacy uGUI / onClick() Inspector wiring
/// 
/// SETUP:
///   1. Attach this script to the UIManager GameObject in the MainMenu scene.
///   2. UIManager also needs a UIDocument component with MainMenu.uxml assigned.
///   3. Set clinicianSceneName in the Inspector to match Build Settings exactly.
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class MainMenuController : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector fields
    // -------------------------------------------------------------------------

    [Header("Scene Navigation")]
    [Tooltip("Must match the scene name in Build Settings exactly. " +
             "Copy from TipsSceneManager.clinicianSceneName to be safe.")]
    public string clinicianSceneName = "Clinician Scene Ruth";

    [Header("Transition")]
    [Tooltip("Duration in seconds for the fade-out before scene load.")]
    public float fadeOutDuration = 0.4f;

    [Header("Atmosphere")]
    [Tooltip("Speed of the patient-dot pulse animation (cycles per second).")]
    public float dotPulseSpeed = 1.4f;

    // -------------------------------------------------------------------------
    // Private references
    // -------------------------------------------------------------------------

    private VisualElement _root;
    private VisualElement _modalOverlay;
    private VisualElement _patientDot;
    private Button        _btnStart;
    private Button        _btnAbout;
    private Button        _btnModalClose;
    private Button        _btnModalOk;

    private bool _isTransitioning = false;

    // -------------------------------------------------------------------------
    // Unity lifecycle
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        // Query all interactive elements — same pattern as TipsSceneManager.WireButtons()
        _btnStart      = _root.Q<Button>("btn-start");
        _btnAbout      = _root.Q<Button>("btn-about");
        _btnModalClose = _root.Q<Button>("modal-close");
        _btnModalOk    = _root.Q<Button>("modal-ok");
        _modalOverlay  = _root.Q<VisualElement>("modal-overlay");
        _patientDot    = _root.Q<VisualElement>("patient-dot");

        // Wire buttons
        _btnStart.clicked      += OnStartClicked;
        _btnAbout.clicked      += OnAboutClicked;
        _btnModalClose.clicked += CloseModal;
        _btnModalOk.clicked    += CloseModal;

        // Close modal when clicking outside the card
        _modalOverlay.RegisterCallback<ClickEvent>(OnOverlayClicked);

        // Ensure cursor is visible (TipsSceneManager does the same)
        UnityEngine.Cursor.visible   = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        // Always unregister to avoid memory leaks between scene loads
        if (_btnStart      != null) _btnStart.clicked      -= OnStartClicked;
        if (_btnAbout      != null) _btnAbout.clicked      -= OnAboutClicked;
        if (_btnModalClose != null) _btnModalClose.clicked -= CloseModal;
        if (_btnModalOk    != null) _btnModalOk.clicked    -= CloseModal;

        if (_modalOverlay != null)
            _modalOverlay.UnregisterCallback<ClickEvent>(OnOverlayClicked);
    }

    private void Update()
    {
        PulsePatientDot();
    }

    // -------------------------------------------------------------------------
    // Button handlers
    // -------------------------------------------------------------------------

    private void OnStartClicked()
    {
        if (_isTransitioning) return;
        _isTransitioning = true;
        StartCoroutine(LoadSceneWithFade(clinicianSceneName));
    }

    private void OnAboutClicked()
    {
        if (_isTransitioning) return;
        OpenModal();
    }

    // -------------------------------------------------------------------------
    // Scene transition
    // -------------------------------------------------------------------------

    /// <summary>
    /// Fades the entire UI out then loads the target scene.
    /// The fade is achieved by tweening the root VisualElement's opacity.
    /// </summary>
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            // Ease-in quadratic: starts slow, accelerates into black
            float t = Mathf.Clamp01(elapsed / fadeOutDuration);
            float eased = t * t;
            _root.style.opacity = 1f - eased;
            yield return null;
        }

        _root.style.opacity = 0f;

        // Mirror TipsSceneManager: use scene name, not index
        SceneManager.LoadScene(sceneName);
    }

    // -------------------------------------------------------------------------
    // Modal
    // -------------------------------------------------------------------------

    private void OpenModal()
    {
        // USS class swap — same technique used throughout TipsSceneManager
        _modalOverlay.AddToClassList("visible");
    }

    private void CloseModal()
    {
        _modalOverlay.RemoveFromClassList("visible");
    }

    private void OnOverlayClicked(ClickEvent evt)
    {
        // Only close if the click landed on the overlay itself, not the card inside it
        if (evt.target == _modalOverlay)
            CloseModal();
    }

    // -------------------------------------------------------------------------
    // Atmosphere
    // -------------------------------------------------------------------------

    /// <summary>
    /// Pulses the patient status dot opacity in a smooth sine wave.
    /// Runs in Update — no coroutine needed for a continuous loop.
    /// </summary>
    private void PulsePatientDot()
    {
        if (_patientDot == null) return;

        // Oscillates between 0.35 and 1.0
        float sin     = Mathf.Sin(Time.time * dotPulseSpeed * Mathf.PI);
        float opacity = Mathf.Lerp(0.35f, 1.0f, (sin + 1f) * 0.5f);
        _patientDot.style.opacity = opacity;
    }
}
