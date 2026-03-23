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
/// OPTIONS MODAL:
///   - Musophobia toggle: reads/writes PlayerPrefs key "Musophobia" (int 0/1).
///     NOTE: MusophobiaMode.cs uses UnityEngine.UI (uGUI) and must NOT be attached
///     to any GameObject in the MainMenu scene. This controller handles the logic.
///   - Time of day: reads/writes PlayerPrefs key "TimeOfDay" (int minutes-since-midnight).
///     Values: 540 = 9 AM, 960 = 4 PM, 1380 = 11 PM, 0 = actual system time.
///     NOTE: TimeOfDaySelect.cs uses UnityEngine.UI (uGUI) and must NOT be attached
///     to any GameObject in the MainMenu scene. This controller handles the logic.
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
    // Private references — About modal
    // -------------------------------------------------------------------------

    private VisualElement _root;
    private VisualElement _modalOverlay;
    private VisualElement _patientDot;
    private Button        _btnStart;
    private Button        _btnAbout;
    private Button        _btnModalClose;
    private Button        _btnModalOk;

    // -------------------------------------------------------------------------
    // Private references — Options modal
    // -------------------------------------------------------------------------

    private VisualElement _optionsModalOverlay;
    private Button        _btnOptions;
    private Button        _btnOptionsClose;
    private Button        _btnOptionsOk;

    // Musophobia
    private Toggle        _toggleMusophobia;

    // Time-of-day selector buttons
    private Button        _todMorning;    // 540  = 9 AM
    private Button        _todAfternoon;  // 960  = 4 PM
    private Button        _todNight;      // 1380 = 11 PM
    private Button        _todActual;     // 0    = actual system time

    // Tracks which tod button is currently selected so we can swap the class
    private Button        _todSelected;

    private bool _isTransitioning = false;

    // -------------------------------------------------------------------------
    // Unity lifecycle
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        // ── About modal ───────────────────────────────────────────────────────
        _btnStart      = _root.Q<Button>("btn-start");
        _btnAbout      = _root.Q<Button>("btn-about");
        _btnModalClose = _root.Q<Button>("modal-close");
        _btnModalOk    = _root.Q<Button>("modal-ok");
        _modalOverlay  = _root.Q<VisualElement>("modal-overlay");
        _patientDot    = _root.Q<VisualElement>("patient-dot");

        _btnStart.clicked      += OnStartClicked;
        _btnAbout.clicked      += OnAboutClicked;
        _btnModalClose.clicked += CloseAboutModal;
        _btnModalOk.clicked    += CloseAboutModal;
        _modalOverlay.RegisterCallback<ClickEvent>(OnAboutOverlayClicked);

        // ── Options modal ─────────────────────────────────────────────────────
        _btnOptions         = _root.Q<Button>("btn-options");
        _optionsModalOverlay = _root.Q<VisualElement>("options-modal-overlay");
        _btnOptionsClose    = _root.Q<Button>("options-modal-close");
        _btnOptionsOk       = _root.Q<Button>("options-modal-ok");
        _toggleMusophobia   = _root.Q<Toggle>("toggle-musophobia");
        _todMorning         = _root.Q<Button>("tod-btn-morning");
        _todAfternoon       = _root.Q<Button>("tod-btn-afternoon");
        _todNight           = _root.Q<Button>("tod-btn-night");
        _todActual          = _root.Q<Button>("tod-btn-actual");

        _btnOptions.clicked          += OnOptionsClicked;
        _btnOptionsClose.clicked     += CloseOptionsModal;
        _btnOptionsOk.clicked        += CloseOptionsModal;
        _optionsModalOverlay.RegisterCallback<ClickEvent>(OnOptionsOverlayClicked);

        _todMorning.clicked   += () => SelectTimeOfDay(_todMorning,   540);
        _todAfternoon.clicked += () => SelectTimeOfDay(_todAfternoon, 960);
        _todNight.clicked     += () => SelectTimeOfDay(_todNight,    1380);
        _todActual.clicked    += () => SelectTimeOfDay(_todActual,      0);

        // ── Restore saved prefs ───────────────────────────────────────────────
        LoadMusophobiaPref();
        LoadTimeOfDayPref();

        // ── Cursor ───────────────────────────────────────────────────────────
        UnityEngine.Cursor.visible   = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        // About modal
        if (_btnStart      != null) _btnStart.clicked      -= OnStartClicked;
        if (_btnAbout      != null) _btnAbout.clicked      -= OnAboutClicked;
        if (_btnModalClose != null) _btnModalClose.clicked -= CloseAboutModal;
        if (_btnModalOk    != null) _btnModalOk.clicked    -= CloseAboutModal;
        if (_modalOverlay  != null) _modalOverlay.UnregisterCallback<ClickEvent>(OnAboutOverlayClicked);

        // Options modal
        if (_btnOptions          != null) _btnOptions.clicked          -= OnOptionsClicked;
        if (_btnOptionsClose     != null) _btnOptionsClose.clicked     -= CloseOptionsModal;
        if (_btnOptionsOk        != null) _btnOptionsOk.clicked        -= CloseOptionsModal;
        if (_optionsModalOverlay != null) _optionsModalOverlay.UnregisterCallback<ClickEvent>(OnOptionsOverlayClicked);

        // Time-of-day buttons — lambdas registered above are anonymous, so we
        // just let them go when the VisualElement tree is destroyed with the scene.
    }

    private void Update()
    {
        PulsePatientDot();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) PlayerPrefs.Save();
    }

    // -------------------------------------------------------------------------
    // Button handlers — About modal
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
        OpenAboutModal();
    }

    private void OpenAboutModal()  => _modalOverlay.AddToClassList("visible");
    private void CloseAboutModal() => _modalOverlay.RemoveFromClassList("visible");

    private void OnAboutOverlayClicked(ClickEvent evt)
    {
        if (evt.target == _modalOverlay)
            CloseAboutModal();
    }

    // -------------------------------------------------------------------------
    // Button handlers — Options modal
    // -------------------------------------------------------------------------

    private void OnOptionsClicked()
    {
        if (_isTransitioning) return;
        OpenOptionsModal();
    }

    private void OpenOptionsModal()  => _optionsModalOverlay.AddToClassList("visible");

    private void CloseOptionsModal()
    {
        // Persist musophobia immediately (toggle fires onValueChanged, but save
        // again here for belt-and-braces consistency with TimeOfDay pattern).
        PlayerPrefs.SetInt("Musophobia", _toggleMusophobia.value ? 1 : 0);
        PlayerPrefs.Save();
        _optionsModalOverlay.RemoveFromClassList("visible");
    }

    private void OnOptionsOverlayClicked(ClickEvent evt)
    {
        if (evt.target == _optionsModalOverlay)
            CloseOptionsModal();
    }

    // -------------------------------------------------------------------------
    // Musophobia
    // -------------------------------------------------------------------------

    private void LoadMusophobiaPref()
    {
        bool isOn = PlayerPrefs.GetInt("Musophobia", 0) == 1;
        _toggleMusophobia.value = isOn;
        // Wire value-change so it saves immediately if the user toggles without
        // explicitly pressing "Save & close".
        _toggleMusophobia.RegisterValueChangedCallback(OnMusophobiaChanged);
    }

    private void OnMusophobiaChanged(ChangeEvent<bool> evt)
    {
        PlayerPrefs.SetInt("Musophobia", evt.newValue ? 1 : 0);
    }

    // -------------------------------------------------------------------------
    // Time of day
    // -------------------------------------------------------------------------

    private void LoadTimeOfDayPref()
    {
        // Default to 540 (9 AM) if the key has never been set
        int saved = PlayerPrefs.GetInt("TimeOfDay", 540);
        Button toSelect = saved switch
        {
            960  => _todAfternoon,
            1380 => _todNight,
            0    => _todActual,
            _    => _todMorning   // 540 and anything unexpected
        };
        ApplyTodSelection(toSelect);
    }

    private void SelectTimeOfDay(Button btn, int minutes)
    {
        PlayerPrefs.SetInt("TimeOfDay", minutes);
        ApplyTodSelection(btn);
    }

    /// <summary>
    /// Moves the "selected" USS class to <paramref name="btn"/>.
    /// Safe to call when _todSelected is null (first load).
    /// </summary>
    private void ApplyTodSelection(Button btn)
    {
        if (_todSelected != null)
            _todSelected.RemoveFromClassList("selected");

        _todSelected = btn;

        if (_todSelected != null)
            _todSelected.AddToClassList("selected");
    }

    // -------------------------------------------------------------------------
    // Scene transition
    // -------------------------------------------------------------------------

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t     = Mathf.Clamp01(elapsed / fadeOutDuration);
            float eased = t * t;
            _root.style.opacity = 1f - eased;
            yield return null;
        }

        _root.style.opacity = 0f;
        SceneManager.LoadScene(sceneName);
    }

    // -------------------------------------------------------------------------
    // Atmosphere
    // -------------------------------------------------------------------------

    private void PulsePatientDot()
    {
        if (_patientDot == null) return;
        float sin     = Mathf.Sin(Time.time * dotPulseSpeed * Mathf.PI);
        float opacity = Mathf.Lerp(0.35f, 1.0f, (sin + 1f) * 0.5f);
        _patientDot.style.opacity = opacity;
    }
}
