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
        audioSource = GetComponent<AudioSource>();
        transition.SetTrigger("End");
        // Play first clip
        audioSource.clip = firstClip;
        audioSource.Play();

        // Wait until the first clip finishes
        yield return new WaitForSeconds(firstClip.length);

        // Play second clip
        audioSource.clip = secondClip;
        audioSource.Play();

        yield return new WaitForSeconds(secondClip.length);

        SceneManager.LoadScene(levelIndex);
    }
}
