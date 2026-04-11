using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRat : MonoBehaviour
{

    //This script is used to spawn rat or darkling hallucinations

    public GameObject ratPrefab;
    public GameObject darklingPrefab;

    [Header("HallucinationDialogue")] // What Arthur says in each scene for rats and for the darklings (rat alternative)
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

    void Start()
    {
        if (PlayerPrefs.HasKey("Musophobia")) //Check playerprefs for the musophobia key; if it is there, use the setting (0 or 1) to indicate musophobia mode off or on, otherwise assume off
        {
            Musophobia = PlayerPrefs.GetInt("Musophobia");
        }
        else Musophobia = 0;
    }

    public void TriggerHallucinationDialogue()
    {
        string currentScene = SceneManager.GetActiveScene().name; // check scene name

        if (currentScene != replaySceneName) // If the scene is not the replay
        {
            //Clinician Scene

            switch (Musophobia)
            { 
                case 0: // musophobia mode is off, i.e. rats are spawning
                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogue); // pass this line to the Dialogue manager for display
                    break;

                case 1: // musophobia mode is on, i.e. darklings are spawning         
                     DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogueMusophobia); // pass this line to the Dialogue manager for display
                    break;
            }
        }
        
        if (currentScene == replaySceneName) // if the scene is the replay
        {
            //Replay Scene

            switch (Musophobia)          
            {
                case 0: // musophobia mode is off, i.e. rats are spawning
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologue); // pass this line to the Dialogue manager for display
                    break;

                case 1:  // musophobia mode is on, i.e. darklings are spawning 
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologueMusophobia); // pass this line to the Dialogue manager for display
                    break;

            }
        }
    }

    public void SpawnRats() 
    {
        Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ)); // rats spawn at fixed x & y position, but a range on z, so will spawn across the wall arthur is looking at
        Instantiate(ratPrefab, spawnPos, ratPrefab.transform.rotation); // instantiate rats in the correct rotation
        
    }

    public void SpawnDarklings()
    {
        Vector3 spawnPos = new(spawnPosX, spawnPosY, Random.Range(-spawnRangeZ, spawnRangeZ)); // darklings spawn at fixed x & y position, but a range on z, so will spawn across the wall arthur is looking at
        Instantiate(darklingPrefab, spawnPos, darklingPrefab.transform.rotation);  // instantiate darklings in the correct rotation
    }

    public void StartSpawn()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName) // if the scene is the replay, run this code.  Necessary to ensure spawn does not occur during clinician scene,
                                                                   // because this script is being used there to run the dialogue
        {
            switch (Musophobia)
            {
                case 0:  // musophobia mode is off, i.e. rats are spawning
                    InvokeRepeating("SpawnRats", StartDelay, SpawnInterval); // spawn rats at intervals following a set delay
                    break;

                case 1: // musophobia mode is on, i.e. darklings are spawning 
                    InvokeRepeating("SpawnDarklings", StartDelay, SpawnInterval); // spawn darklings at intervals following a set delay
                    break;

            }     
        }
    }

    
}
