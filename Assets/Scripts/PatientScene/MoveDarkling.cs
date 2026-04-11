using UnityEngine;

public class MoveDarkling : MonoBehaviour
{
    private float xLimit = -5.0f; // beyond this darklings are destroyed

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);

        if (transform.position.x < xLimit)
        {
            Destroy(gameObject);

        }
    }
}
