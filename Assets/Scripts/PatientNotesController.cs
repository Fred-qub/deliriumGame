using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;

/// <summary>
/// Controls the PatientNotes UI Toolkit screen.
/// 
/// SETUP:
///   1. Create an empty GameObject in your scene, name it PatientNotes_UI
///   2. Add a UIDocument component to it
///   3. Set UIDocument → Source Asset → PatientNotes.uxml
///   4. Add this script (PatientNotesController) to the same GameObject
///   5. Set the GameObject to inactive in the Inspector (untick the checkbox)
///
/// OPENING FROM interactableObject:
///   In the terminal's interactableObject OnInteract() UnityEvent:
///   - Remove the old SetActive(Canvas) call
///   - Add: PatientNotes_UI GameObject → PatientNotesController.OpenNotes()
///
/// TAB WIRING:
///   Tab button clicks are registered here in code — no Inspector wiring needed.
/// </summary>
public class PatientNotesController : MonoBehaviour
{
    public GameObject playerCam;

    // ── Tab title strings — shown in the content header on tab switch ──
    private static readonly string[] TabTitles = {
        "Patient Details",
        "Previous Medical History",
        "Current Episode Summary",
        "Blood Results — Full Blood Count",
        "Other Investigations"
    };

    // ── Static flag — lets other scripts know the UI is open ───────────
    public static bool UIOpen { get; private set; }

    // ── Assign in Inspector: drag your Main Camera here ────────────────
    [SerializeField] private CinemachineBrain cinemachineBrain;

    // ── Internal state ─────────────────────────────────────────────────
    private UIDocument   _doc;
    private VisualElement _root;
    private int          _currentTab = 0;

    // Cached element references
    private Button[]      _tabButtons;
    private VisualElement[] _panels;
    private Label         _contentTitle;
    private Button        _exitButton;

    // ── Unity lifecycle ────────────────────────────────────────────────

    private void Awake()
    {
        _doc  = GetComponent<UIDocument>();
        _root = _doc.rootVisualElement;

        // Cache tab buttons (named tab-0 through tab-4 in UXML)
        _tabButtons = new Button[5];
        for (int i = 0; i < 5; i++)
        {
            int index = i; // capture for lambda
            _tabButtons[i] = _root.Q<Button>($"tab-{i}");
            _tabButtons[i].clicked += () => SwitchTab(index);
        }

        // Cache content panels (named panel-0 through panel-4 in UXML)
        _panels = new VisualElement[5];
        for (int i = 0; i < 5; i++)
        {
            _panels[i] = _root.Q<VisualElement>($"panel-{i}");
        }

        // Cache content title label and exit button
        _contentTitle = _root.Q<Label>("content-title");
        _exitButton   = _root.Q<Button>("btn-exit");
        _exitButton.clicked += CloseNotes;
    }

    private void Update()
    {
        if (UIOpen)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

        if (!UIOpen)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }


    }
    private void OnEnable()
    {
        UIOpen = true;
        SwitchTab(0);
        if (cinemachineBrain != null) cinemachineBrain.enabled = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible   = true;
    }

    private void OnDisable()
    {
        UIOpen = false;
        if (cinemachineBrain != null) cinemachineBrain.enabled = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible   = false;
    }

    // ── Public API ─────────────────────────────────────────────────────

    /// <summary>
    /// Call this from interactableObject's OnInteract() UnityEvent.
    /// Activates the GameObject which triggers OnEnable automatically.
    /// </summary>
    public void OpenNotes()
    {
        gameObject.SetActive(true);

    }

    /// <summary>
    /// Called by the Exit button. Deactivates the screen.
    /// </summary>
    public void CloseNotes()
    {
        gameObject.SetActive(false);
        playerCam.SetActive(true);
    }

    // ── Tab switching ──────────────────────────────────────────────────

    private void SwitchTab(int index)
    {
        _currentTab = index;

        // Update tab button styles
        for (int i = 0; i < _tabButtons.Length; i++)
        {
            if (i == index)
            {
                _tabButtons[i].AddToClassList("pn-ctab-active");
            }
            else
            {
                _tabButtons[i].RemoveFromClassList("pn-ctab-active");
            }
        }

        // Show the selected panel, hide the rest
        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == index)
            {
                _panels[i].RemoveFromClassList("pn-panel-hidden");
            }
            else
            {
                _panels[i].AddToClassList("pn-panel-hidden");
            }
        }

        // Update the content header title
        if (_contentTitle != null)
            _contentTitle.text = TabTitles[index];
    }
}
