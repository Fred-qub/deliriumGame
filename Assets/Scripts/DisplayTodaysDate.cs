using UnityEngine;
using TMPro; // For TextMeshPro support


public class DisplayTodayDate : MonoBehaviour
{
    
    public TextMeshPro tmpWorldText;

    [Tooltip("C# DateTime format string, e.g. 'dd/MM/yyyy', 'MMMM dd, yyyy'")]
    public string dateFormat = "dd/MMMM/yyyy";

    void Start()
    {            
            // Get today's date in the specified format
            string today = System.DateTime.Now.ToString(dateFormat);


            // Display on assigned component
           
            if (tmpWorldText != null)
                tmpWorldText.text = today;

    }
}