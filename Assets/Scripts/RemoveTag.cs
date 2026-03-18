using UnityEngine;

public class RemoveTag : MonoBehaviour
{
    public GameObject lightSwitch;
    public GameObject arthur;
    public void UnTagSwitch() 
    {
        lightSwitch.tag = "Untagged";

    }

    public void UnTagArthur()
    {
        arthur.tag = "Untagged";

    }
}
