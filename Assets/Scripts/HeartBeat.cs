using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HeartBeat : MonoBehaviour
{
    // Heatbeat value starts at 1
   [SerializeField] private int heartbeat = 1;
    public int Heartbeat => heartbeat;

    // Min and max limits
    public int minValue = 0;
    public int maxValue = 2;

    void Start()
    {
        Debug.Log("Starting heart value: " + heartbeat);
    }
    

    void Update()
    {
        
    }

    // Increase value by 1 (with limit)
    public void AddOne()
    {
        if (heartbeat < maxValue)
        {
            heartbeat++;
            Debug.Log("Heart Value increased to: " + heartbeat);
        }

        else
        {
            Debug.Log("Heart Value is already at maximum.");
        }
    }

    // Decrease value by 1 (with limit)
    public void SubtractOne()
    {
        if (heartbeat > minValue)
        {
            heartbeat--;
            Debug.Log("Heart Value decreased to: " + heartbeat);
        }
        else
        {
            Debug.Log("Heart Value is already at minimum.");
        }
    }
}

