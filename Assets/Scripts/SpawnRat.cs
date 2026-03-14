using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRat : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject darklingPrefab;
    public int Musophobia;
    private float spawnPosY = -0.88f; // items spawn -0.88 in Y direction
    private float spawnPosX = 3; // items spawn at 3 on x-axis
    private float spawnRangeZ = 4; 
    private float StartDelay = 0f; // delay before items start to spawn is 0 sec
    private float SpawnInterval = 1f; //items spawn every 0.5 sec
    private string currentSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey("Musophobia"))
        {
            Musophobia = PlayerPrefs.GetInt("Musophobia");
        }
        else Musophobia = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRats() 
    {
        if (currentSceneName == "Patient Scene Ruth") // if Scene is patient scene
        {
            Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ));
            Instantiate(ratPrefab, spawnPos, ratPrefab.transform.rotation);
        }

        else Debug.Log("No spawn as wrong scene");
    }

    public void SpawnDarklings()
    {
        if (currentSceneName == "Patient Scene Ruth") // if Scene is patient scene
        {
            Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ));
            Instantiate(darklingPrefab, spawnPos, darklingPrefab.transform.rotation);
        }

        else Debug.Log("No spawn as wrong scene");
    }

    public void StartSpawn() 
    {
        switch (Musophobia)
        {
            case 0:
                InvokeRepeating("SpawnRats", StartDelay, SpawnInterval);
                break;

            case 1:
                InvokeRepeating("SpawnDarklings", StartDelay, SpawnInterval);
                break;

        }     

    }

    
}
