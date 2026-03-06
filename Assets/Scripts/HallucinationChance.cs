using Unity.Collections;
using UnityEngine;

public class HallucinationChance : MonoBehaviour
{
    public int hallucinationRatChancePercentage; //chance of a rat (or rat alternative) hallucination occurring
    public int hallucinationSnakeChancePercentage; //chance of a snake hallucination occurring
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

    public void HallucinationRatLottery()
    {

        if (Random.Range(0f, 100f) < hallucinationRatChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {
            Debug.Log("SpawnRat");
            spawnRat.StartSpawn();

        }

        else Debug.Log("NoRat");
    }

    public void HallucinationSnakeLottery()
    {

        if (Random.Range(0f, 100f) < hallucinationSnakeChancePercentage) // if random number between 0 & 100 is less than the hallucination chance percentage
        {
            Debug.Log("SpawnSnake");
            spawnSnake.Newspaper();
            spawnSnake.Snake();
            

        }

        else Debug.Log("NoSnake");
    }
}
