using UnityEngine;

public class MoveRat : MonoBehaviour
{
    private float xLimit = -5.0f; // beyond this rats are destroyed

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime);

        if (transform.position.x < xLimit) 
        { 
            Destroy(gameObject);
        
        }

    }
}
