using Unity.Collections;
using UnityEngine;

public class HallucinationChance : MonoBehaviour
{
    public int auxHallucinationChancePercentage; // chance of an auxially hallucination occuring on a bad choice
    public int hallucinationTypeChancePercentage; // chance of particular hallucination occurring
    public SpawnRat spawnRat;
    public SpawnSnake spawnSnake;

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
            spawnRat.StartSpawn();  // rat (or rat alternative) is spawned

        }

        else Debug.Log("Spawn Snake"); 
        spawnSnake.Newspaper();
        spawnSnake.Snake(); // otherwise a snake is spawned
    }

  
}
