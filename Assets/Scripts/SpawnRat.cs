using UnityEngine;

public class SpawnRat : MonoBehaviour
{
    public GameObject ratPrefab;
    private float spawnPosY = -0.88f; // rats spawn -0.88 in Y direction
    private float spawnPosX = 3; // rats spawn at -3 on x-axis
    private float spawnRangeZ = 4; 
    private float StartDelay = 0f; // delay before rats start to spawn is 0 sec
    private float SpawnInterval = 0.5f; //rats spawn every 0.5 sec

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRats() 
    {
        Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ));
        Instantiate(ratPrefab, spawnPos, ratPrefab.transform.rotation);
        
    }

    public void StartSpawn() 
    {

        InvokeRepeating("SpawnRats", StartDelay, SpawnInterval);

    }
}
