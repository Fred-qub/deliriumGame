using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections; 

public class HeadTrackSmooth : MonoBehaviour
{
    [Header("Rig Settings")]
    [SerializeField] private MultiAimConstraint multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder; // assign if you want instant rig rebuilds
    [SerializeField] private GameObject rack;

    [Header("Blend Settings")]
    [SerializeField] private float switchDuration = 0.5f;

    private WeightedTransformArray sources;
    private Coroutine switchRoutine;

    void Awake()
    {
        if (multiAimConstraint == null)
        {
            Debug.LogError("MultiAimConstraint reference is missing!");
            enabled = false;
            return;
        }

        sources = multiAimConstraint.data.sourceObjects;

        if (sources.Count != 2)
        {
            Debug.LogError("Please assign exactly 2 sources in the MultiAimConstraint.");
            enabled = false;
            return;
        }

        // Start with rack active
        sources.SetWeight(0, 1f);
        sources.SetWeight(1, 0f);
        multiAimConstraint.data.sourceObjects = sources;
        StartSwitch(0);
        
    }

    private void Update()
    {
        rigBuilder.Build();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartSwitch(1); // Switch to looking at player
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && rack.activeInHierarchy == true)
            {
            StartSwitch(0); // Switch back to looking at coat rack

        }
        
    }

    private void StartSwitch(int activeIndex)
    {
        if (switchRoutine != null)
            StopCoroutine(switchRoutine);

        switchRoutine = StartCoroutine(SwitchRoutine(activeIndex));
    }

    private IEnumerator SwitchRoutine(int activeIndex)
    {
        float startWeight0 = sources.GetWeight(0);
        float startWeight1 = sources.GetWeight(1);

        float targetWeight0 = (activeIndex == 0) ? 1f : 0f;
        float targetWeight1 = (activeIndex == 1) ? 1f : 0f;

        float elapsed = 0f;

        while (elapsed < switchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / switchDuration);

            sources.SetWeight(0, Mathf.Lerp(startWeight0, targetWeight0, t));
            sources.SetWeight(1, Mathf.Lerp(startWeight1, targetWeight1, t));

            // Apply changes so rig sees them
            multiAimConstraint.data.sourceObjects = sources;

            // Force rig to rebuild immediately
            if (rigBuilder != null)
                rigBuilder.Build();

            // Wait until end of frame so changes are applied before next rig evaluation
            yield return new WaitForEndOfFrame();
        }

        // Ensure exact final weights
        sources.SetWeight(0, targetWeight0);
        sources.SetWeight(1, targetWeight1);
        multiAimConstraint.data.sourceObjects = sources;

        if (rigBuilder != null)
            rigBuilder.Build();
    }

    public void CoatRemoved() 
    {
        StartSwitch(1);

    }
}
