using UnityEngine;
using UnityEngine.UI;

public class OutlineDisable : MonoBehaviour
{

    // This script is used to disable the outline on an object if required

    public Outline outline;
    public void OutlineOff() 
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

    }
}
