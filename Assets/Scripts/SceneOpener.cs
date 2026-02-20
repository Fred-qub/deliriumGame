using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers Arthur's opening line when the scene loads, before the player
/// has made any interactions. Attach this to the Arthur NPC GameObject.
/// </summary>
public class SceneOpener : MonoBehaviour
{
    [Tooltip("How long to wait after the scene loads before Arthur speaks. " +
             "Gives the player a moment to take in the environment first.")]
    [SerializeField] private float delayBeforeDialogue = 2f;

    private void Start()
    {
        StartCoroutine(PlayOpeningLine());
    }

    private IEnumerator PlayOpeningLine()
    {
        // Brief pause so the player can orient themselves before Arthur speaks
        yield return new WaitForSeconds(delayBeforeDialogue);

        DialogueManager.Instance.ShowArthurLine(
            "I don't know who you are. What do you want? " +
            "Why are you sitting there saying nothing?"
        );
    }
}
