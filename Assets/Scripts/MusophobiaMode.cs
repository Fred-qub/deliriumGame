using UnityEngine;
using UnityEngine.UI;

public class MusophobiaMode : MonoBehaviour
{
    [SerializeField] private Toggle musophobiaCheckbox;
    public int savedMusophobia;
    private void Start()
    {
        savedMusophobia = PlayerPrefs.GetInt("Musophobia");

        if (savedMusophobia == 1) 
        {
            musophobiaCheckbox.isOn = true;
          
        }

        else musophobiaCheckbox.isOn = false;

        // Subscribe to value change event
        musophobiaCheckbox.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Called whenever the toggle changes state
    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("Musophobia", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Musophobia", 0);
        }
    }

  
}
