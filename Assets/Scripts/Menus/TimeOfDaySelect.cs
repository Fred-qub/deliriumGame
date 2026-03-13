using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TimeOfDaySelect : MonoBehaviour
{

    public Button button;
    public int TimeOfDay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetTimeOfDay); // When button is clicked, it selects the time of day

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetTimeOfDay()
    {
        switch (TimeOfDay)
        {
            case 540: // 9AM
                PlayerPrefs.SetInt("TimeOfDay", 540);
                break;
            case 960: //4PM
                PlayerPrefs.SetInt("TimeOfDay", 960);
                break;
            case 1380: //11PM
                PlayerPrefs.SetInt("TimeOfDay", 1380);
                break;
            case 0: //ACTUAL TIME
                PlayerPrefs.SetInt("TimeOfDay", 0);
                break;
        }
  

    }
}
