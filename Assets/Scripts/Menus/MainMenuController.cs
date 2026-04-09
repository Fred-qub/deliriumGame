using System.Collections;
using System;
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
/// OPTIONS MODAL — three sections:
///
///   ACCESSIBILITY
///   - Content warnings: Musophobia (key "Musophobia") and Ophidiophobia
///     (key "Ophidiophobia") toggles, both int 0/1 in PlayerPrefs.
///   - Additional subtitles: key "Subtitles" (int 0/1).
///   - Colour blindness accommodation: key "OutlineScheme" (int 0/1/2/3).
///
///   CONTROLS
///   - Crosshair size: key "CrosshairSize" (int 0/1/2 = small/medium/large).
///   - Crosshair colour: key "CrosshairColour" (int 0/1/2/3/4 = white/red/blue/yellow/orange)
///
///   SIMULATION
///   - Time of day: key "TimeOfDay" (int minutes-since-midnight).
///     Values: 540 = 9 AM, 960 = 4 PM, 1380 = 11 PM, 0 = actual system time.
///
///   All selector button groups share the USS class "options-btn" with a
///   "selected" modifier applied by ApplySelection() helpers.
///
///   NOTE: MusophobiaMode.cs and TimeOfDaySelect.cs use UnityEngine.UI (uGUI)
///   and must NOT be attached to any GameObject in the MainMenu scene.
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

    // Ophidiophobia
    private Toggle        _toggleOphidiophobia;

    // Radio Subtitles
    private Toggle        _toggleSubtitles;

    // Crosshair Size selector buttons
    private Button _crosshairSmall; // 0
    private Button _crosshairMedium; // 1 
    private Button _crosshairLarge;  // 2

    // Tracks which crosshair size button is currently selected so we can swap the class
    private Button _crosshairSelected;

    // Crosshair Colour selector buttons
    private Button _crosshairColourWhite; //0
    private Button _crosshairColourRed; //1
    private Button _crosshairColourBlue; //2
    private Button _crosshairColourYellow; //3
    private Button _crosshairColourOrange; //4

    // Tracks which crosshair colour button is currently selected so we can swap the class
    private Button _crosshairColourSelected;

    // Outline Scheme selector buttons
    private Button _outlineStandard;
    private Button _outlineProtanopia;
    private Button _outlineDeuteranopia;
    private Button _outlineTritanopia;

    // Tracks which outline selector button is currently selected so we can swap the class
    private Button _outlineSelected;


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
        _toggleOphidiophobia = _root.Q<Toggle>("toggle-ophidiophobia");
        _toggleSubtitles = _root.Q<Toggle>("toggle-subtitles");
        _crosshairSmall = _root.Q<Button>("crosshair-btn-small");
        _crosshairMedium = _root.Q<Button>("crosshair-btn-medium");
        _crosshairLarge = _root.Q<Button>("crosshair-btn-large");
        _crosshairColourWhite = _root.Q<Button>("crosshairColour-btn-white");
        _crosshairColourRed = _root.Q<Button>("crosshairColour-btn-red");
        _crosshairColourBlue = _root.Q<Button>("crosshairColour-btn-blue");
        _crosshairColourYellow = _root.Q<Button>("crosshairColour-btn-yellow");
        _crosshairColourOrange = _root.Q<Button>("crosshairColour-btn-orange");
        _outlineStandard = _root.Q<Button>("outline-btn-standard");
        _outlineProtanopia = _root.Q<Button>("outline-btn-protanopia");
        _outlineDeuteranopia = _root.Q<Button>("outline-btn-deuteranopia");
        _outlineTritanopia = _root.Q<Button>("outline-btn-tritanopia");
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

        _crosshairSmall.clicked += () => SelectCrosshairSize(_crosshairSmall, 0);
        _crosshairMedium.clicked += () => SelectCrosshairSize(_crosshairMedium, 1);
        _crosshairLarge.clicked += () => SelectCrosshairSize(_crosshairLarge, 2);

        _crosshairColourWhite.clicked += () => SelectCrosshairColour(_crosshairColourWhite, 0);
        _crosshairColourRed.clicked += () => SelectCrosshairColour(_crosshairColourRed, 1);
        _crosshairColourBlue.clicked += () => SelectCrosshairColour(_crosshairColourBlue, 2);
        _crosshairColourYellow.clicked += () => SelectCrosshairColour(_crosshairColourYellow, 3);
        _crosshairColourOrange.clicked += () => SelectCrosshairColour(_crosshairColourOrange, 4);
        
        _outlineStandard.clicked += () => SelectOutlineScheme(_outlineStandard, 0);
        _outlineProtanopia.clicked += () => SelectOutlineScheme(_outlineProtanopia, 1);
        _outlineDeuteranopia.clicked += () => SelectOutlineScheme(_outlineDeuteranopia, 2);
        _outlineTritanopia.clicked += () => SelectOutlineScheme(_outlineTritanopia, 3);


        // ── Restore saved prefs ───────────────────────────────────────────────
        LoadMusophobiaPref();
        LoadOphidiophobiaPref();
        LoadSubtitlesPref();
        LoadTimeOfDayPref();
        LoadCrosshairSizePref();
        LoadCrosshairColourPref();
        LoadOutlineScheme();

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
        // All selector buttons share the USS "options-btn" + "selected" pattern.
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
        // Persist musophobia & ophidiophobia immediately (toggle fires onValueChanged, but save
        // again here for belt-and-braces consistency with TimeOfDay pattern).
        PlayerPrefs.SetInt("Musophobia", _toggleMusophobia.value ? 1 : 0);
        PlayerPrefs.SetInt("Ophidiophobia", _toggleOphidiophobia.value ? 1 : 0);
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
    // Ophidiophobia
    // -------------------------------------------------------------------------

    private void LoadOphidiophobiaPref()
    {
        bool isOn = PlayerPrefs.GetInt("Ophidiophobia", 0) == 1;
        _toggleOphidiophobia.value = isOn;
        // Wire value-change so it saves immediately if the user toggles without
        // explicitly pressing "Save & close".
        _toggleOphidiophobia.RegisterValueChangedCallback(OnOphidiophobiaChanged);
    }

    private void OnOphidiophobiaChanged(ChangeEvent<bool> evt)
    {
        PlayerPrefs.SetInt("Ophidiophobia", evt.newValue ? 1 : 0);
    }

    // -------------------------------------------------------------------------
    // Radio Subtitles
    // -------------------------------------------------------------------------

    private void LoadSubtitlesPref()
    {
        bool isOn = PlayerPrefs.GetInt("Subtitles", 0) == 1;
        _toggleSubtitles.value = isOn;
        // Wire value-change so it saves immediately if the user toggles without
        // explicitly pressing "Save & close".
        _toggleSubtitles.RegisterValueChangedCallback(OnSubtitlesChanged);
    }

    private void OnSubtitlesChanged(ChangeEvent<bool> evt)
    {
        PlayerPrefs.SetInt("Subtitles", evt.newValue ? 1 : 0);
    }

    // -------------------------------------------------------------------------
    // Crosshair Size
    // -------------------------------------------------------------------------

    private void LoadCrosshairSizePref()
    {
        // Default to small if the key has never been set
        int saved = PlayerPrefs.GetInt("CrosshairSize", 0);
        Button toSelect = saved switch
        {         
            1 => _crosshairMedium,
            2 => _crosshairLarge,
            _ => _crosshairSmall, // 0 & anything unexpected

        };
        ApplyCrosshairSelection(toSelect);
    }

    private void SelectCrosshairSize(Button btn, int value)
    {
        PlayerPrefs.SetInt("CrosshairSize", value);
        ApplyCrosshairSelection(btn);
    }

    /// <summary>
    /// Moves the "selected" USS class to <paramref name="btn"/>.
    /// Safe to call when _CrosshairSelected is null (first load).
    /// </summary>
    private void ApplyCrosshairSelection(Button btn)
    {
        if (_crosshairSelected != null)
            _crosshairSelected.RemoveFromClassList("selected");

        _crosshairSelected = btn;

        if (_crosshairSelected != null)
            _crosshairSelected.AddToClassList("selected");
    }


    // -------------------------------------------------------------------------
    // Crosshair Colour
    // -------------------------------------------------------------------------

    private void LoadCrosshairColourPref()
    {
        // Default to white if the key has never been set
        int saved = PlayerPrefs.GetInt("CrosshairColour", 0);
        Button toSelect = saved switch
        {
            1 => _crosshairColourRed,
            2 => _crosshairColourBlue,
            3 => _crosshairColourYellow,
            4 => _crosshairColourOrange,
            0 => _crosshairColourWhite,
            _ => _crosshairColourWhite,
        };
        ApplyCrosshairColourSelection(toSelect);
    }

    private void SelectCrosshairColour(Button btn, int value)
    {
        PlayerPrefs.SetInt("CrosshairColour", value);
        ApplyCrosshairColourSelection(btn);
    }

    /// <summary>
    /// Moves the "selected" USS class to <paramref name="btn"/>.
    /// Safe to call when _CrosshairSelected is null (first load).
    /// </summary>
    private void ApplyCrosshairColourSelection(Button btn)
    {
        if (_crosshairColourSelected != null)
            _crosshairColourSelected.RemoveFromClassList("selected");

        _crosshairColourSelected = btn;

        if (_crosshairColourSelected != null)
            _crosshairColourSelected.AddToClassList("selected");
    }

    // -------------------------------------------------------------------------
    // Outline Scheme
    // -------------------------------------------------------------------------

    private void LoadOutlineScheme()
    {
        // Default to standard if the key has never been set
        int saved = PlayerPrefs.GetInt("OutlineScheme", 0);
        Button toSelect = saved switch
        {
            0 => _outlineStandard,
            1 => _outlineProtanopia,
            2 => _outlineDeuteranopia,
            3 => _outlineTritanopia,
            _ => _outlineStandard,
        };
        ApplyOutlineColourScheme(toSelect);
    }

    private void SelectOutlineScheme(Button btn, int value)
    {
        PlayerPrefs.SetInt("OutlineScheme", value);
        ApplyOutlineColourScheme(btn);
    }

    /// <summary>
    /// Moves the "selected" USS class to <paramref name="btn"/>.
    /// Safe to call when _OutlineSelected is null (first load).
    /// </summary>
    private void ApplyOutlineColourScheme(Button btn)
    {
        if (_outlineSelected != null)
            _outlineSelected.RemoveFromClassList("selected");

        _outlineSelected = btn;

        if (_outlineSelected != null)
            _outlineSelected.AddToClassList("selected");
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
