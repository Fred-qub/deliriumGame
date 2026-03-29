using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HeartBeat : MonoBehaviour
{
    [SerializeField] private int heartbeat = 2; // equivalent to 82 bpm
    public int Heartbeat => heartbeat;

    public delegate void HeartRateChanged(int heartbeat); 
    public static event HeartRateChanged OnHeartRateChanged;
    
    public static void BroadcastRateChanged(int heartbeat)
    {
        OnHeartRateChanged?.Invoke(heartbeat);
    }

  
    // Min and max limits
    private int minValue = 0; // 52 bpm
    private int maxValue = 4; // 121 bpm

    void Start()
    {
        Debug.Log("Starting heart value: " + heartbeat);
        BroadcastRateChanged(heartbeat);
    }
    

    // Increase value by 1 (with limit)
    public void AddOne()
    {
        if (heartbeat < maxValue)
        {
            heartbeat++;
            Debug.Log("Heart Value increased to: " + heartbeat);
            BroadcastRateChanged(heartbeat);
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
            BroadcastRateChanged(heartbeat);
        }
        else
        {
            Debug.Log("Heart Value is already at minimum.");
        }
    }
}

