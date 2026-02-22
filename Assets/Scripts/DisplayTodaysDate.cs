using UnityEngine;
using TMPro; // For TextMeshPro support


public class DisplayTodayDate : MonoBehaviour
{
    
    public TextMeshPro tmpWorldText; // Assign if using 3D TextMeshPro


    [Header("Date Format")]
    [Tooltip("C# DateTime format string, e.g. 'dd/MM/yyyy', 'MMMM dd, yyyy'")]
    public string dateFormat = "dd/MM/yyyy";

    void Start()
    {
        try
        {
            // Get today's date in the specified format
            string today = System.DateTime.Now.ToString(dateFormat);

            // Display on whichever component is assigned
           
            if (tmpWorldText != null)
                tmpWorldText.text = today;
           
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error displaying date: " + ex.Message);
        }
    }
}