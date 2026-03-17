using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ChangeHeadTrack : MonoBehaviour
{
    public MultiAimConstraint aimConstraint; // Assign in Inspector
    public Transform corner;              // New source Transform
    public Transform mainCam;
    public RigBuilder rigBuilder;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        
            if (aimConstraint == null || corner == null)
            {
                Debug.LogError("MultiAimConstraint or corner is not assigned.");
                return;
            }

            // Get the current list of sources
            WeightedTransformArray sources = aimConstraint.data.sourceObjects;

            if (sources.Count > 0)
            {
            // Replace the first source
            sources.SetTransform(0, corner);
            sources.SetWeight(0, 1f); // Full influence
            }
            else
            {
            // Add a new source if none exist
            sources.Add(new WeightedTransform(corner, 1f));
            }

            // Apply the modified sources back to the constraint
            aimConstraint.data.sourceObjects = sources;

            // Force the constraint to update immediately
            aimConstraint.weight = 1f;

            rigBuilder.Build();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")

            if (aimConstraint == null || mainCam == null)
            {
                Debug.LogError("MultiAimConstraint or mainCam is not assigned.");
                return;
            }

        // Get the current list of sources
        WeightedTransformArray sources = aimConstraint.data.sourceObjects;

        if (sources.Count > 0)
        {
            // Replace the first source
            sources.SetTransform(0, mainCam);
            sources.SetWeight(0, 1f); // Full influence
        }
        else
        {
            // Add a new source if none exist
            sources.Add(new WeightedTransform(mainCam, 1f));
        }

        // Apply the modified sources back to the constraint
        aimConstraint.data.sourceObjects = sources;

        // Force the constraint to update immediately
        aimConstraint.weight = 1f;

        rigBuilder.Build();
    }


}
