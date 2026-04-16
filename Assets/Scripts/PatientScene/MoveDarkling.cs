using UnityEngine;

public class MoveDarkling : MonoBehaviour
{
 // This script is attached to the darkling hallucinations and controls their movement through the room

    private float xLimit = -5.0f; // beyond this darklings are destroyed

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime); // move darkling forwawrd

        if (transform.position.x < xLimit) // if darkling passes the set limit on the x-axis
        {
            Destroy(gameObject); // destroy it

        }
    }
}
