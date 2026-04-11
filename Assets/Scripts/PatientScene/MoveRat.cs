using UnityEngine;

public class MoveRat : MonoBehaviour
{
    // THis script is attached to the rat gameobjects (hallucinations) & controls their movement through the scene

    private float xLimit = -5.0f; // beyond this rats are destroyed

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime); // move rats through the scene from sink area toward arthur in the bed

        if (transform.position.x < xLimit) // if the rat passes the set limit on the x-axis
        { 
            Destroy(gameObject); // destroy it
        
        }

    }
}
