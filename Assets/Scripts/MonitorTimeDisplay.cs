using TMPro;
using UnityEngine;

public class MonitorTimeDisplay : MonoBehaviour
{
    //-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;
    public int seconds = 0;

    public float TimeOfDay;
    public TextMeshPro tmpWorldText;

    float msecs = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("TimeOfDay"))
        {
            TimeOfDay = PlayerPrefs.GetInt("TimeOfDay");
        }

        switch (TimeOfDay)
        {
            case 540: //9AM
                hour = 9;
                minutes = 0;
                seconds = 0;
                break;

            case 960: //4PM
                hour = 16;
                minutes = 0;
                seconds = 0;
                break;

            case 1380: //11PM
                hour = 23;
                minutes = 0;
                seconds = 0;
                break;

            case 0: //ACTUAL TIME
                hour = System.DateTime.Now.Hour;
                minutes = System.DateTime.Now.Minute;
                seconds = System.DateTime.Now.Second;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        string timeString;

        //-- calculate time
        msecs += Time.deltaTime;
        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes > 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour >= 24)
                        hour = 0;
                }
            }
        }

        timeString = string.Format("{0:00}:{1:00}:{2:00}", hour, minutes, seconds);
        tmpWorldText.text = timeString;

    }
}
