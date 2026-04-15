using UnityEngine;
using UnityEditor;

public class AlignForwardAxis : EditorWindow
{
    // This script was added to assist with figuring out the angles for the patient's head to turn to track the player

    private GameObject targetObject;
    private Vector3 desiredDirection = Vector3.forward; // Default to world +Z
    private GameObject lookAtTarget;

    [MenuItem("Tools/Align Forward Axis")]
    public static void ShowWindow()
    {
        GetWindow<AlignForwardAxis>("Align Forward Axis");
    }

    private void OnGUI()
    {
        GUILayout.Label("Align Forward Axis Tool", EditorStyles.boldLabel);

        targetObject = (GameObject)EditorGUILayout.ObjectField(
            "Target Object", targetObject, typeof(GameObject), true);

        desiredDirection = EditorGUILayout.Vector3Field(
            "Desired Direction", desiredDirection);

        lookAtTarget = (GameObject)EditorGUILayout.ObjectField(
            "Look At Target (Optional)", lookAtTarget, typeof(GameObject), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Align Forward"))
        {
            AlignForward();
        }
    }

    private void AlignForward()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("No target object selected.");
            return;
        }

        Undo.RecordObject(targetObject.transform, "Align Forward Axis");

        if (lookAtTarget != null)
        {
            // Align forward to face the target object
            Vector3 direction = lookAtTarget.transform.position - targetObject.transform.position;
            if (direction.sqrMagnitude > 0.0001f)
                targetObject.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
        else
        {
            // Align forward to a fixed world direction
            if (desiredDirection.sqrMagnitude > 0.0001f)
                targetObject.transform.rotation = Quaternion.LookRotation(desiredDirection.normalized, Vector3.up);
        }

        Debug.Log($"Aligned {targetObject.name}'s forward axis.");
    }
}

