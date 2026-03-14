using System.Collections;
using UnityEngine;

public class HallucinationReplay : MonoBehaviour
{
    private int hallucinationType;
    public SpawnRat spawnRat;
    public SpawnSnake spawnSnake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hallucinationType = PlayerPrefs.GetInt("HallucinationType");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHalluccinationType()
    {
        switch (hallucinationType)
        {
            case 0:
                break;
            case 1:
                spawnRat.StartSpawn();
                break;
            case 2:
                spawnSnake.Newspaper();
                spawnSnake.Snake(); 
                break;



        }

    }
}
