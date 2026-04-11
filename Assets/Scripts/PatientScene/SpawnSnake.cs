using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSnake : MonoBehaviour
{
    public ShrinkObject newspaper;
    public GameObject snake;
    public GameObject plant;

    public int Ophidiophobia;

    [Header("Hallucination Dialogue")]
    [TextArea] public string arthurClinicianDialogue = "Help! There's a snake!";
    [TextArea] public string arthurClinicianDialogueOphidiophobia = "Help!  A man-eating plant!";

    [TextArea] public string arthurReplayMonologue = "Oh no! There's a snake!";
    [TextArea] public string arthurReplayMonologueOphidiophobia = "Arrgh! Is that plant actually trying to bite me?!";

    private string replaySceneName = "PatientScene Ruth";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
            if (PlayerPrefs.HasKey("Ophidiophobia"))
            {
                Ophidiophobia = PlayerPrefs.GetInt("Ophidiophobia");
            }
            else Ophidiophobia = 0;
        

    }

    public void TriggerHallucinationDialogue()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != replaySceneName)
        {
            //Clinician Scene

            switch (Ophidiophobia)
            {
                case 0:
                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogue);
                    break;

                case 1:

                    DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogueOphidiophobia);
                    break;
            }
        }

        if (currentScene == replaySceneName)
        {
            //Replay Scene

            switch (Ophidiophobia)
            {
                case 0:
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologue);
                    break;

                case 1:
                    DialogueManager.Instance.ShowMonologue(arthurReplayMonologueOphidiophobia);
                    break;

            }
        }
    }

    public void Newspaper()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
        {
            newspaper.StartShrinking();
        }
    }
    

    public void Snake()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
        {
            snake.SetActive(true);
        }
    }

    public void Plant()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
        {
            plant.SetActive(true);
        }
    }

    public void StartSpawn()
    {
        if (SceneManager.GetActiveScene().name == replaySceneName)
        {
            switch (Ophidiophobia)
            {
                case 0:
                    Snake();
                    break;

                case 1:
                    Plant();
                    break;

            }
        }
    }


}
