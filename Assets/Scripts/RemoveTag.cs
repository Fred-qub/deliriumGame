using UnityEngine;

public class RemoveTag : MonoBehaviour
{
    public GameObject lightSwitch;
    public void UnTag() 
    {
        lightSwitch.tag = "Untagged";

    }
}
