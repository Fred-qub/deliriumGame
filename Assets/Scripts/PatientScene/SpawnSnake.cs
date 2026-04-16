using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSnake : MonoBehaviour
{
    // This scrcipt is used to spawn the snake or man-eating plant hallucinations
    public ShrinkObject newspaper;
    public GameObject snake;
    public GameObject plant;

    public int Ophidiophobia;

    [Header("Hallucination Dialogue")] // What Arthur says in each scene for snake and for the plant (snake alternative)
    [TextArea] public string arthurClinicianDialogue = "Help! There's a snake!";
    [TextArea] public string arthurClinicianDialogueOphidiophobia = "Help!  A man-eating plant!";

    [TextArea] public string arthurReplayMonologue = "Oh no! There's a snake!";
    [TextArea] public string arthurReplayMonologueOphidiophobia = "Arrgh! Is that plant actually trying to bite me?!";

    private string replaySceneName = "PatientScene Ruth";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
            if (PlayerPrefs.HasKey("Ophidiophobia")) //Check playerprefs for the ophidiophobia key; if it is there, use the setting (0 or 1) to indicate ophidiophobia mode off or on, otherwise assume off
        {
                Ophidiophobia = PlayerPrefs.GetInt("Ophidiophobia");
            }
            else Ophidiophobia = 0;
        

    }

    public void TriggerHallucinationDialogue()
    {
        string currentScene = SceneManager.GetActiveScene().name; // check scene name

        if (currentScene != replaySceneName) // if scene is not the replay
        {
            //Clinician Scene

            switch (Ophidiophobia)
            {
                case 0: // ophidiophobia mode is off, i.e. snake will spawn
                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogue);
                    break;

                case 1: // ophidiophobia mode is on, i.e. plant will spawn
                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogueOphidiophobia);
                    break;
            }
        }

        if (currentScene == replaySceneName)
        {
            //Replay Scene

            switch (Ophidiophobia)
            {
                case 0:  // ophidiophobia mode is off, i.e. snake will spawn
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologue);
                    break;

                case 1: // ophidiophobia mode is on, i.e. plant will spawn
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologueOphidiophobia);
                    break;

            }
        }
    }

    public void Newspaper()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName) // if scene is replay
        {
            newspaper.StartShrinking(); // shrink the newspaper 
        }
    }
    

    public void Snake()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName) // if scene is replay
        {
            snake.SetActive(true); // set the snake object active (this will also cause the grow object script on that object to trigger)
        }
    }

    public void Plant()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
        {
            plant.SetActive(true); // set the plant object active (this will also cause the grow object script on that object to trigger)
        }
    }
    

    public void StartSpawn()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName) // if the scene is the replay, run this code.  Necessary to ensure spawn does not occur during clinician scene,
                                                                   // because this script is being used there to run the dialogue
        {
            switch (Ophidiophobia)
            {
                case 0: // ophidiophobia mode is off, i.e. snake will spawn
                    Snake();
                    break;

                case 1: // ophidiophobia mode is on, i.e. plant will spawn
                    Plant();
                    break;

            }
        }
    }


}
