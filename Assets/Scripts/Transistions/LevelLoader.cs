using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    // This loads the transitions between main menu & first scene, and also between clinician & patient scenes.  Sounds are played and captions for those sounds are displayed if necessary
    public Animator transition;
    public float transitionTime = 2f;

    [Header("Assign your audio clips here")]
    public AudioClip firstClip;
    public AudioClip secondClip;

    [Header("Assign your subtitles here")]
    public GameObject knock;
    public GameObject door;


    public int subtitles;
    private AudioSource audioSource;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // player presses E key to continue laoading sequence
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(PlaySoundsSequentially(SceneManager.GetActiveScene().buildIndex + 1)); 

    }

   IEnumerator PlaySoundsSequentially(int levelIndex)
    {
        CheckSubtitleOption(); // Checks if subtitles are on

        audioSource = GetComponent<AudioSource>();
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1); //wait
        // Play first clip
        audioSource.clip = firstClip;
        audioSource.Play();
        if (subtitles == 1) // if subtitles are on
        {
            knock.SetActive(true); // enable the caption for that sound
        }
       
        // Wait until the first clip finishes
        yield return new WaitForSeconds(firstClip.length + 1);

        // Play second clip
        audioSource.clip = secondClip;
        audioSource.Play();
        knock.SetActive(false); // disable first sound caption

        if (subtitles == 1)  // if subtitles are on
        {
            door.SetActive(true); // enable the caption for that sound
        }

        yield return new WaitForSeconds(secondClip.length + 0.5f); // wait til clip finishes

        SceneManager.LoadScene(levelIndex); // load next scene
    }

    void CheckSubtitleOption() 
    {
        if (PlayerPrefs.HasKey("Subtitles")) // checks playerprefs for subtitles key.  If it's there, use it; if not, assume off.
        {
            subtitles = PlayerPrefs.GetInt("Subtitles");
        }
        else subtitles = 0;

    }

    
}
