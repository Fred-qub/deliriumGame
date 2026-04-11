
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour
{
    // This script changes patient heartrate depending on player actions, and broadcasts it so other scrcipts can pick it up

    [SerializeField] private int heartbeat = 2; // sets int for starting rate, equivalent to 82 bpm
    public int Heartbeat => heartbeat;

    public delegate void HeartRateChanged(int heartbeat); 
    public static event HeartRateChanged OnHeartRateChanged;
    
    public static void BroadcastRateChanged(int heartbeat)
    {
        OnHeartRateChanged?.Invoke(heartbeat);
    }

  
    // Min and max limits for int
    private int minValue = 0; // 52 bpm
    private int maxValue = 4; // 121 bpm

    void Start()
    {
        Debug.Log("Starting heart value: " + heartbeat);
        BroadcastRateChanged(heartbeat); // broadcasts the initial heartrate for other scripts to read
    }
    

    // Increase heartrate int value by 1 as long as value is not already at max, and broadcast change.  Called when player performs a negative action (sedative or lights)
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

    // Decrease value by 1 as long as value is not already at min, and broadcast change.  Called when player performs a positive action (hearing aid or remove coat)
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

