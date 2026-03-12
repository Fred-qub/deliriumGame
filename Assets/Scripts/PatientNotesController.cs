using System;
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

    // Patient is always 70 years old — DOB calculated backwards from today.
    // When this becomes a full product, replace with a [SerializeField] PatientData
    // ScriptableObject and read age/dob from there.
    private static readonly DateTime PatientDOB = DateTime.Now.AddYears(-70);

    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject playerCam;

    public static bool UIOpen { get; private set; }

    private UIDocument _doc;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _doc.rootVisualElement;

        root.Q<Button>("tab-0").clicked += () => SwitchTab(0);
        root.Q<Button>("tab-1").clicked += () => SwitchTab(1);
        root.Q<Button>("tab-2").clicked += () => SwitchTab(2);
        root.Q<Button>("tab-3").clicked += () => SwitchTab(3);
        root.Q<Button>("tab-4").clicked += () => SwitchTab(4);
        root.Q<Button>("btn-exit").clicked += CloseNotes;

        PopulateDynamicFields(root);

        UIOpen = true;
        Time.timeScale = 0f;
        SwitchTab(0);

        if (playerCam != null)        playerCam.SetActive(false);
        if (cinemachineBrain != null)  cinemachineBrain.enabled = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible   = true;
    }

    private void OnDisable()
    {
        UIOpen = false;
        Time.timeScale = 1f;
        if (playerCam != null)        playerCam.SetActive(true);
        if (cinemachineBrain != null)  cinemachineBrain.enabled = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible   = false;
    }

    public void OpenNotes()  => gameObject.SetActive(true);
    public void CloseNotes() => gameObject.SetActive(false);

    // ── Dynamic field population ─────────────────────────────────────────────

    private static void PopulateDynamicFields(VisualElement root)
    {
        DateTime today         = DateTime.Now;
        DateTime admissionDate = today.AddDays(-1);
        int      age           = CalculateAge(PatientDOB, today);
        string   dobStr        = PatientDOB.ToString("dd/MM/yyyy");
        string   admitStr      = admissionDate.ToString("dd/MM/yyyy");

        // Top bar — date only (time and clinician stay static)
        root.Q<Label>("topbar-meta").text =
            $"{admitStr}  ·  09:41  ·  Dr A. Other";

        // Patient identity banner sub-line
        root.Q<Label>("banner-sub").text =
            $"Male  ·  {age} yo  ·  DOB: {dobStr}  ·  HCN: 308 721 3008  ·  Tel: 028 9012 3456  ·  Bed: BCH Med Ward–Rm1";

        // Sidebar DOB row
        root.Q<Label>("sidebar-dob").text =
            $"Male, {age} yo. {dobStr}";

        // Sidebar admission row
        root.Q<Label>("sidebar-admitted").text =
            $"Admitted: {admitStr}";

        // Content area timestamp
        root.Q<Label>("content-timestamp").text =
            $"Last updated: {admitStr} 09:00  ·  Dr Alice N Other";
    }

    private static int CalculateAge(DateTime birthDate, DateTime today)
    {
        int age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month ||
            (today.Month == birthDate.Month && today.Day < birthDate.Day))
            age--;
        return age;
    }

    // ── Tab switching ────────────────────────────────────────────────────────

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
