using System.Collections;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallucinationChance : MonoBehaviour
{
    private int auxHallucinationChancePercentage = 50; // chance of an auxilliary hallucination occuring on a bad choice
    private int hallucinationTypeChancePercentage = 50; // chance of particular hallucination occurring
    public InteractionMaster trustManager;
    private string rat = "RatHallucination";
    private string snake = "SnakeHallucination";
    public SpawnRat ratSpawner;
    public SpawnSnake snakeSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AuxHallucinationLottery() 
    {

        if (Random.Range(0f, 100f) < auxHallucinationChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {
            Debug.Log("Hallucination occurs");
            HallucinationTypeLottery(); // the hallucination type is chosen

        }

        else Debug.Log("No Hallucination occurs");

    }



    public void HallucinationTypeLottery()
    {

        if (Random.Range(0f, 100f) < hallucinationTypeChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {                  
                Debug.Log("SpawnRat");
                trustManager.interactionHistory.Add(rat);
                if (ratSpawner != null)
                {
                    StartCoroutine(DelayedHallucinationDialogue(ratSpawner));
                }
        }

        else
        {
            Debug.Log("Spawn Snake"); 
            trustManager.interactionHistory.Add(snake);
            if (snakeSpawner != null)
            {
                StartCoroutine(DelayedHallucinationDialogue(snakeSpawner));
            }
        }
    }

    //Wait until Current Dialogue is finished to say Hallucination Dialogue
    private IEnumerator DelayedHallucinationDialogue(MonoBehaviour spawner)
    {
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive());
        
        yield return new WaitForSeconds(0.5f);
        
        if (spawner is SpawnRat rat) rat.TriggerHallucinationDialogue();
        else if (spawner is SpawnSnake snake) snake.TriggerHallucinationDialogue();
    }

  
}
