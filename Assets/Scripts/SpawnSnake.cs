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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Newspaper() 
    {
        if (currentSceneName == "Patient Scene Ruth") // if Scene is patient scene
        {
            newspaper.StartShrinking();
        }

        else  Debug.Log("No shrink as wrong scene");
    }

    public void Snake()
    {
        if (currentSceneName == "Patient Scene Ruth") // if Scene is patient scene
        {
            snake.SetActive(true);
        }

        else Debug.Log("No snake as wrong scene");

    }



}
