using UnityEngine;
using UnityEngine.UI;

public class ClosePatientNotes : MonoBehaviour
{
   
    public Button targetButton1; //Patient notes login screen exit button
    public Button targetButton2; //Patient notes exit button
    public KeyCode triggerKey1 = KeyCode.E; //Keyboard Key to Trigger Button
    public KeyCode triggerKey2 = KeyCode.Escape; //2nd keyboard key to trigger button

    void Update()
    {
        // Check if either key is pressed down this frame
        if (Input.GetKeyDown(triggerKey1) || Input.GetKeyDown(triggerKey2))
        {
            if (targetButton1.interactable || targetButton2.interactable)
            {
                // Simulate button click

                targetButton2.onClick.Invoke();
            }

        }
    }
}
