using System.Collections;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallucinationChance : MonoBehaviour
{
    // This script is used to figure out whether or not an hallucination should play on a negative player action (i.e. lights or sedative), and if so, which one.
    // It would be more effective if there were more possible interactions.  It was felt that having the same hallucination every time could lead the player to believe that a particular patient reaction was a foregone conclusion, whereas
    // this is not the case.  This script is used in both scenes, because the decision must be made in the clinician scene to allow correct dialogue to play.  However, the spawn scripts themselvevs
    // prevent the hallucinations actually spawning in the clinician scene.

    private int auxHallucinationChancePercentage = 50; // chance of an auxilliary hallucination occurring on a bad choice
    private int hallucinationTypeChancePercentage = 50; // chance of particular hallucination occurring
    public InteractionMaster trustManager;
    private string rat = "RatHallucination";
    private string snake = "SnakeHallucination";
    public SpawnRat ratSpawner;
    public SpawnSnake snakeSpawner;

    public void AuxHallucinationLottery() 
    {

        if (Random.Range(0f, 100f) < auxHallucinationChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {
            Debug.Log("Hallucination occurs");
            HallucinationTypeLottery(); // the hallucination type is chosen

        }

        else Debug.Log("No Hallucination occurs");

    }
    public void HallucinationTypeLottery() // this figures out what hallucination type should appear, and ensures the same one does not appear more than once in a run
    {

        if (Random.Range(0f, 100f) < hallucinationTypeChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {
            if (!trustManager.interactionHistory.Contains(rat)) // if the rat hallucination does not already appear in the interaction history
            {
                TriggerRatSpawner(); // spawn a rat
            }
            else if (trustManager.interactionHistory.Contains(rat) && !trustManager.interactionHistory.Contains(snake)) // if the interaction history already contains rat, and does not already contain snake
            {
                TriggerSnakeSpawner(); // spawn a snake
            }
        }

        else
        {
            if (!trustManager.interactionHistory.Contains(snake)) // if the snake hallucination does not already appear in the interaction history
            {
                TriggerSnakeSpawner(); // spawn a snake
            }
            else if (trustManager.interactionHistory.Contains(snake) && !trustManager.interactionHistory.Contains(rat)) // if the interaction history already contains snake, and does not already contain rat
            {
                
                TriggerRatSpawner(); // spawn a rat
            }
        }
    }

    private void TriggerRatSpawner()
    {
        Debug.Log("Spawn Rat");
        trustManager.interactionHistory.Add(rat); // add rat to the interaction history
        if (ratSpawner != null) // if the rat spawner exists
        {
            StartCoroutine(DelayedHallucinationDialogue(ratSpawner));
        }
    }

    private void TriggerSnakeSpawner()
    {
        Debug.Log("Spawn Snake"); 
        trustManager.interactionHistory.Add(snake); // add snake to the interaction history
        if (snakeSpawner != null) // if the snake spawner exists
        {
            StartCoroutine(DelayedHallucinationDialogue(snakeSpawner));
        }
    }

    //Wait until Current Dialogue is finished to say correct Hallucination Dialogue
    private IEnumerator DelayedHallucinationDialogue(MonoBehaviour spawner)
    {
        yield return new WaitUntil(() => DialogueManager.Instance.IsDialogueActive());
        
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());
        
        yield return new WaitForSeconds(1.0f);
        
        if (spawner is SpawnRat rat) rat.TriggerHallucinationDialogue();
        else if (spawner is SpawnSnake snake) snake.TriggerHallucinationDialogue();
    }
  
}
