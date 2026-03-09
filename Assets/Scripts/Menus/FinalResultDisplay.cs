using UnityEngine;
using TMPro;

public class FinalResultDisplay : MonoBehaviour
{
    private void Start()
    {
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();

        if (txt == null)
        {
            return;
        }
        
        if (InteractionMaster.Instance != null)
        {
            txt.text = InteractionMaster.Instance.GetFinalResultText();
        }
        else
        {
            // Fallback 
            txt.text = "Result Unknown (No Data Found)";
        }
    }
}
