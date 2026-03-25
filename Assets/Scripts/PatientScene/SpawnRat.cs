using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRat : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject darklingPrefab;

    [Header("HallucinationDialogue")] 
    [TextArea] public string arthurClinicianDialogue = "Help! There's rats everywhere!";
    [TextArea] public string arthurClinicianDialogueMusophobia = "Help! Those things with glowing eyes are everywhere!";

    [TextArea] public string arthurReplayMonologue = "Oh no! There's rats everywhere!";
    [TextArea] public string arthurReplayMonologueMusophobia = "Oh no! Those things with glowing eyes are everywhere!";

    public int Musophobia;
    private float spawnPosY = -0.88f; // items spawn -0.88 in Y direction
    private float spawnPosX = 3; // items spawn at 3 on x-axis
    private float spawnRangeZ = 4; 
    private float StartDelay = 0f; // delay before items start to spawn is 0 sec
    private float SpawnInterval = 1f; //items spawn every 0.5 sec
    
    private string replaySceneName = "PatientScene Ruth";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("Musophobia"))
        {
            Musophobia = PlayerPrefs.GetInt("Musophobia");
        }
        else Musophobia = 0;
    }

    public void TriggerHallucinationDialogue()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != replaySceneName)
        {
            //Clinician Scene

            switch (Musophobia)
            { 
                case 0:
                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogue);
                    break;

                case 1:
            
                     DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogueMusophobia);
                     break;
            }
        }
        
        if (currentScene == replaySceneName)
        {
            //Replay Scene

            switch (Musophobia)          
            {
                case 0:
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologue);
                    break;

                case 1:
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologueMusophobia);
                    break;

            }
        }
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

    public void SpawnDarklings()
    {
        Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ));
        Instantiate(darklingPrefab, spawnPos, darklingPrefab.transform.rotation);
    }

    public void StartSpawn()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
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

    
}
