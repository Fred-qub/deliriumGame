using UnityEngine;
using UnityEngine.UI;

public class OutlineDisable : MonoBehaviour
{

    public Outline outline;
    public void OutlineOff() 
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

    }
}
