using UnityEngine;

public class MoveRat : MonoBehaviour
{
    public float xLimit = -10.0f; // beyond this rats are destroyed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime);

        
    }
}
