using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSnake : MonoBehaviour
{
    public ShrinkObject newspaper;
    public GameObject snake;
    
    [Header("Hallucination Dialogue")]
    [TextArea] public string arthurClinicianDialogue = "Help! There's a snake!";

    [TextArea] public string arthurReplayMonologue = "Oh no! There's a snake!";
    
    private string replaySceneName = "PatientScene Ruth";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void TriggerHallucinationDialogue()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != replaySceneName)
        {
            DialogueManager.Instance.ShowArthurLine(arthurClinicianDialogue);
        }
        else
        {
            DialogueManager.Instance.ShowArthurLine(arthurReplayMonologue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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



}
