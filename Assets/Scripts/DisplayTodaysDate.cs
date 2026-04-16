using UnityEngine;
using TMPro; // For TextMeshPro support


public class DisplayTodayDate : MonoBehaviour
{
    // This script is used to retrieve the current date from the system, and convert it in the correct format for display on the whiteboard/blackboard in the scenes
    
    public TextMeshPro tmpWorldText;

    [Tooltip("C# DateTime format string, e.g. 'dd/MM/yyyy', 'MMMM dd, yyyy'")]
    public string dateFormat = "dd/MMMM/yyyy"; // sets the date format to display the month in words and the year in 4 digits

    void Start()
    {            
            // Get today's date in the specified format
            string today = System.DateTime.Now.ToString(dateFormat);


            // Display on assigned component
           
            if (tmpWorldText != null)
                tmpWorldText.text = today;

    }
}