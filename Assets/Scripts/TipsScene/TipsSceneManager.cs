using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the Tips/Session Review scene.
/// Reads the two chosen action names from PlayerPrefs (saved by SceneReplayer),
/// builds the tip cards dynamically, and sets the correct optimal/suboptimal state.
///
/// SETUP:
///   1. Attach this script to the UIManager GameObject in TipsScene.
///   2. The UIManager GameObject also needs a UIDocument component with TipsScreen.uxml assigned.
///   3. Set clinicianSceneName in the Inspector to match your Build Settings scene name exactly.
/// </summary>
public class TipsSceneManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // PlayerPrefs keys — must match SceneReplayer
    // -------------------------------------------------------------------------

    private const string KEY_CHOICE_1 = "TipsChoice1";
    private const string KEY_CHOICE_2 = "TipsChoice2";

    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Scene Names")]
    [Tooltip("Must match the scene name in Build Settings exactly.")]
    public string clinicianSceneName = "Clinician Scene Ruth";

    [Header("Optimal Combination")]
    [Tooltip("Must match DemoInteractable objectName exactly.")]
    public string hearingAidActionName = "HearingAid";

    [Tooltip("Must match DemoInteractable objectName exactly.")]
    public string removeCoatActionName = "Coat";

    // -------------------------------------------------------------------------
    // Tip Data — all five interactions
    // -------------------------------------------------------------------------

    private struct TipData
    {
        public string actionName;
        public string displayTitle;
        public string body;
        public bool isPositive;
    }

    private readonly TipData[] allTips = new TipData[]
    {
        new TipData {
            actionName   = "HearingAid",
            displayTitle = "Hearing Aid Fitted",
            body         = "Restoring sensory aids is one of the most effective non-pharmacological interventions for delirium. Disorientation is significantly worsened by untreated hearing loss — fitting Arthur's hearing aids immediately improved his ability to process and respond to his environment.",
            isPositive   = true
        },
        new TipData {
            actionName   = "Coat",
            displayTitle = "Removed Coat",
            body         = "Unfamiliar objects are a leading trigger for delirium-induced hallucination. Removing misidentified items is a highly effective non-pharmacological intervention — it directly reduces environmental confusion and helps anchor the patient to a safe, recognisable space without any clinical risk.",
            isPositive   = true
        },
        new TipData {
            actionName   = "Sedative",
            displayTitle = "Sedative Administered",
            body         = "Sedatives can suppress delirium symptoms short-term but often worsen overall prognosis. In older adults, benzodiazepines and antipsychotics increase fall risk, prolong delirium duration, and can trigger respiratory complications. Non-pharmacological approaches should always be exhausted first.",
            isPositive   = false
        },
        new TipData {
            actionName   = "Verbal",
            displayTitle = "Spoke to Patient",
            body         = "Verbal engagement is valuable, but directive language often increases agitation in delirious patients. Reorientation works best through calm, open questions that acknowledge distress rather than correct it. Always pair communication with sensory checks — is the patient wearing their hearing aids and glasses?",
            isPositive   = false
        },
        new TipData {
            actionName   = "Lights",
            displayTitle = "Lights Switched On",
            body         = "Lighting aids orientation, but sudden brightness can be distressing or painful for vulnerable patients. Always check the patient's case history before adjusting the environment — gradual changes and natural light are preferable to harsh overhead fluorescents.",
            isPositive   = false
        }
    };

    // -------------------------------------------------------------------------
    // Clinical Insight text
    // -------------------------------------------------------------------------

    private const string OPTIMAL_INSIGHT_TITLE = "Optimal Intervention Combination";
    private const string OPTIMAL_INSIGHT_BODY  =
        "Fitting the hearing aids alongside removing the unfamiliar coat are the two most impactful " +
        "interventions available. Together they address Arthur's two primary delirium triggers — " +
        "sensory deprivation and environmental disorientation — without pharmacological risk.";

    private const string SUBOPTIMAL_INSIGHT_TITLE = "Key Learning Point";
    private const string SUBOPTIMAL_INSIGHT_BODY  =
        "Sensory loss is one of the biggest drivers of delirium-induced hallucination. Always ensure " +
        "the patient has their hearing aids and glasses, and that clear markers of time and place — " +
        "such as a clock or calendar — are visible.";

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    private void Start()
    {
        // to view curson during tips scene
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;     
        var root = GetComponent<UIDocument>().rootVisualElement;

        string choice1 = PlayerPrefs.GetString(KEY_CHOICE_1, "");
        string choice2 = PlayerPrefs.GetString(KEY_CHOICE_2, "");

        bool isOptimal = (choice1 == hearingAidActionName || choice2 == hearingAidActionName)
                      && (choice1 == removeCoatActionName  || choice2 == removeCoatActionName);

        SetScoreBar(root, isOptimal);
        BuildTipCards(root, choice1, choice2);
        SetInsightSection(root, isOptimal);
        WireButtons(root);
    }

    // -------------------------------------------------------------------------
    // Public Static Helper — called by SceneReplayer before loading this scene
    // -------------------------------------------------------------------------

    public static void SaveChoices(string choice1, string choice2)
    {
        PlayerPrefs.SetString(KEY_CHOICE_1, choice1);
        PlayerPrefs.SetString(KEY_CHOICE_2, choice2);
        PlayerPrefs.Save();
    }

    // -------------------------------------------------------------------------
    // Private Builders
    // -------------------------------------------------------------------------

    private void SetScoreBar(VisualElement root, bool isOptimal)
    {
        var scoreValue = root.Q<Label>("score-value");
        var pill       = root.Q<VisualElement>("outcome-pill");
        var pillText   = root.Q<Label>("outcome-pill-text");

        if (isOptimal)
        {
            scoreValue.text = "Optimal";
            scoreValue.RemoveFromClassList("score-value-partial");
            scoreValue.AddToClassList("score-value");
            pill.RemoveFromClassList("outcome-pill-suboptimal");
            pill.AddToClassList("outcome-pill");
            pillText.text = "Best outcome achieved";
            pillText.RemoveFromClassList("outcome-pill-text-suboptimal");
            pillText.AddToClassList("outcome-pill-text");
        }
        else
        {
            scoreValue.text = "Needs Improvement";
            scoreValue.RemoveFromClassList("score-value");
            scoreValue.AddToClassList("score-value-partial");
            pill.RemoveFromClassList("outcome-pill");
            pill.AddToClassList("outcome-pill-suboptimal");
            pillText.text = "Review your interventions";
            pillText.RemoveFromClassList("outcome-pill-text");
            pillText.AddToClassList("outcome-pill-text-suboptimal");
        }
    }

    private void BuildTipCards(VisualElement root, string choice1, string choice2)
    {
        var tipsList = root.Q<VisualElement>("tips-list");
        tipsList.Clear();

        AddTipCard(tipsList, choice1);
        AddTipCard(tipsList, choice2);
    }

    private void AddTipCard(VisualElement container, string actionName)
    {
        if (string.IsNullOrEmpty(actionName)) return;

        TipData? match = null;
        foreach (var tip in allTips)
        {
            if (tip.actionName == actionName)
            {
                match = tip;
                break;
            }
        }

        if (match == null)
        {
            Debug.LogWarning($"[TipsSceneManager] No tip found for action: {actionName}");
            return;
        }

        TipData data = match.Value;

        // Card
        var card = new VisualElement();
        card.AddToClassList("tip-card");

        // Coloured left bar
        var bar = new VisualElement();
        bar.AddToClassList(data.isPositive ? "tip-bar-good" : "tip-bar-warn");
        card.Add(bar);

        // Content area
        var content = new VisualElement();
        content.AddToClassList("tip-content");

        // Header row (title + tag)
        var headerRow = new VisualElement();
        headerRow.AddToClassList("tip-header-row");

        var title = new Label(data.displayTitle);
        title.AddToClassList("tip-title");

        var tag = new Label(data.isPositive ? "Positive" : "Caution");
        tag.AddToClassList(data.isPositive ? "tip-tag-good" : "tip-tag-warn");

        headerRow.Add(title);
        headerRow.Add(tag);

        // Body text
        var body = new Label(data.body);
        body.AddToClassList("tip-body");

        content.Add(headerRow);
        content.Add(body);
        card.Add(content);
        container.Add(card);
    }

    private void SetInsightSection(VisualElement root, bool isOptimal)
    {
        var label = root.Q<Label>("insight-label");
        var card  = root.Q<VisualElement>("insight-card");
        var icon  = root.Q<Label>("insight-icon");
        var title = root.Q<Label>("insight-title");
        var body  = root.Q<Label>("insight-body");

        if (isOptimal)
        {
            label.text = "OPTIMAL OUTCOME";
            card.RemoveFromClassList("insight-card-suboptimal");
            card.AddToClassList("insight-card-optimal");
            icon.text = "★";
            icon.RemoveFromClassList("insight-icon-suboptimal");
            icon.AddToClassList("insight-icon");
            title.text = OPTIMAL_INSIGHT_TITLE;
            title.RemoveFromClassList("insight-title-suboptimal");
            title.AddToClassList("insight-title-optimal");
            body.text = OPTIMAL_INSIGHT_BODY;
            body.RemoveFromClassList("insight-body-suboptimal");
            body.AddToClassList("insight-body-optimal");
        }
        else
        {
            label.text = "CLINICAL INSIGHT";
            card.RemoveFromClassList("insight-card-optimal");
            card.AddToClassList("insight-card-suboptimal");
            icon.text = "→";
            icon.RemoveFromClassList("insight-icon");
            icon.AddToClassList("insight-icon-suboptimal");
            title.text = SUBOPTIMAL_INSIGHT_TITLE;
            title.RemoveFromClassList("insight-title-optimal");
            title.AddToClassList("insight-title-suboptimal");
            body.text = SUBOPTIMAL_INSIGHT_BODY;
            body.RemoveFromClassList("insight-body-optimal");
            body.AddToClassList("insight-body-suboptimal");
        }
    }

    private void WireButtons(VisualElement root)
    {
        var playAgain  = root.Q<Button>("btn-play-again");
        var guidelines = root.Q<Button>("btn-guidelines");

        playAgain.clicked += OnPlayAgain;
        guidelines.clicked += () => Debug.Log("[TipsSceneManager] View Full Guidelines clicked.");
    }

    private void OnPlayAgain()
    {
        PlayerPrefs.DeleteKey(KEY_CHOICE_1);
        PlayerPrefs.DeleteKey(KEY_CHOICE_2);
        PlayerPrefs.Save();

        if (InteractionMaster.Instance != null)
            InteractionMaster.Instance.ResetState();

        SceneManager.LoadScene(clinicianSceneName);
    }
}