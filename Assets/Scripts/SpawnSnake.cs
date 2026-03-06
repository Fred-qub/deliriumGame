using UnityEngine;

public class SpawnSnake : MonoBehaviour
{
    private ShrinkObject newspaper;
    public GameObject snake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newspaper = GameObject.FindWithTag("Newspaper").GetComponent<ShrinkObject>();
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
