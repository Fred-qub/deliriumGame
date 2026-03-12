using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;

public class PatientNotesController : MonoBehaviour
{
    private static readonly string[] TabTitles = {
        "Patient Details",
        "Previous Medical History",
        "Current Episode Summary",
        "Blood Results — Full Blood Count",
        "Other Investigations"
    };

    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject playerCam;

    public static bool UIOpen { get; private set; }

    private UIDocument _doc;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        Debug.Log("[PatientNotes] Awake fired");
    }

    private void OnEnable()
    {
        Debug.Log("[PatientNotes] OnEnable fired");

        var root = _doc.rootVisualElement;
        Debug.Log("[PatientNotes] root is null: " + (root == null));

        var tab0 = root.Q<Button>("tab-0");
        Debug.Log("[PatientNotes] tab-0 found: " + (tab0 != null));

        tab0.clicked += () => { Debug.Log("[PatientNotes] tab-0 clicked"); SwitchTab(0); };
        root.Q<Button>("tab-1").clicked += () => { Debug.Log("[PatientNotes] tab-1 clicked"); SwitchTab(1); };
        root.Q<Button>("tab-2").clicked += () => { Debug.Log("[PatientNotes] tab-2 clicked"); SwitchTab(2); };
        root.Q<Button>("tab-3").clicked += () => { Debug.Log("[PatientNotes] tab-3 clicked"); SwitchTab(3); };
        root.Q<Button>("tab-4").clicked += () => { Debug.Log("[PatientNotes] tab-4 clicked"); SwitchTab(4); };
        root.Q<Button>("btn-exit").clicked += CloseNotes;

        UIOpen = true;
        SwitchTab(0);

        if (playerCam != null)       playerCam.SetActive(false);
        if (cinemachineBrain != null) cinemachineBrain.enabled = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible   = true;

        Debug.Log("[PatientNotes] Setup complete. Cursor visible: " + UnityEngine.Cursor.visible);
    }

    private void OnDisable()
    {
        Debug.Log("[PatientNotes] OnDisable fired");
        UIOpen = false;
        if (playerCam != null)       playerCam.SetActive(true);
        if (cinemachineBrain != null) cinemachineBrain.enabled = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible   = false;
    }

    public void OpenNotes()
    {
        Debug.Log("[PatientNotes] OpenNotes called");
        gameObject.SetActive(true);
    }

    public void CloseNotes()
    {
        Debug.Log("[PatientNotes] CloseNotes called");
        gameObject.SetActive(false);
    }

    private void SwitchTab(int index)
    {
        var root = _doc.rootVisualElement;

        for (int i = 0; i < 5; i++)
        {
            var btn = root.Q<Button>($"tab-{i}");
            if (i == index) btn.AddToClassList("pn-ctab-active");
            else            btn.RemoveFromClassList("pn-ctab-active");
        }

        for (int i = 0; i < 5; i++)
        {
            var panel = root.Q<VisualElement>($"panel-{i}");
            if (i == index) panel.RemoveFromClassList("pn-panel-hidden");
            else            panel.AddToClassList("pn-panel-hidden");
        }

        root.Q<Label>("content-title").text = TabTitles[index];
    }
}
