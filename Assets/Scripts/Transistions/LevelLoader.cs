using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.E))
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
        CheckSubtitleOption();

        audioSource = GetComponent<AudioSource>();
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);
        // Play first clip
        audioSource.clip = firstClip;
        audioSource.Play();
        if (subtitles == 1)
        {
            knock.SetActive(true);
        }
       
        // Wait until the first clip finishes
        yield return new WaitForSeconds(firstClip.length + 1);

        // Play second clip
        audioSource.clip = secondClip;
        audioSource.Play();
        knock.SetActive(false);

        if (subtitles == 1)
        {
            door.SetActive(true);
        }

        yield return new WaitForSeconds(secondClip.length + 0.5f);

        SceneManager.LoadScene(levelIndex);
    }

    void CheckSubtitleOption() 
    {
        if (PlayerPrefs.HasKey("Subtitles"))
        {
            subtitles = PlayerPrefs.GetInt("Subtitles");
        }
        else subtitles = 0;

    }

    
}
