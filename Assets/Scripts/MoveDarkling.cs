using UnityEngine;

public class MoveDarkling : MonoBehaviour
{
    private float xLimit = -5.0f; // beyond this darklings are destroyed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

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
