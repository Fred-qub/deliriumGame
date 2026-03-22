using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSnake : MonoBehaviour
{
    private ShrinkObject newspaper;
    public GameObject snake;
    private string currentSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        newspaper = GameObject.FindWithTag("Newspaper").GetComponent<ShrinkObject>();
        Debug.Log(currentSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Newspaper() 
    {
        newspaper.StartShrinking();
    }

    public void Snake()
    {
        snake.SetActive(true);
    }



}
