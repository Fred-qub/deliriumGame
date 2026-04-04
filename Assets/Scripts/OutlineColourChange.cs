using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class OutlineColourChange : MonoBehaviour
{
    public GameObject lightSwitch;
    public GameObject hearingAid;
    public GameObject sedative;
    public GameObject coat;
    public GameObject computer1;
    public GameObject computer2;
    public GameObject computer3;
    public GameObject computer4;
    public GameObject computer5;
    public GameObject computer6;
    public string outlineScheme;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadOutlineScheme();       
    }


    private void LoadOutlineScheme() 
    {
        if (PlayerPrefs.HasKey("OutlineScheme"))
        { 
            outlineScheme = PlayerPrefs.GetString("OutlineScheme");
        }
        else outlineScheme = "Standard";


        switch (outlineScheme)
        {
            case "Standard":
                Standard();
                break;

            case "Protanopia":
                Protanopia();
                break;

            case "Deuteranopia":
                Deuteranopia();
                break;

            case "Tritanopia":
                Tritanopia();
                break;

        }

    }

    public void Standard() // red for computer & yellow for others
    {
        Outline lightSwitchOutline = lightSwitch.GetComponent<Outline>();
        Outline hearingAidOutline = hearingAid.GetComponent<Outline>();
        Outline sedativeOutline = sedative.GetComponent<Outline>();
        Outline coatOutline = coat.GetComponent<Outline>();
        Outline computer1Outline = computer1.GetComponent<Outline>();
        Outline computer2Outline = computer2.GetComponent<Outline>();
        Outline computer3Outline = computer3.GetComponent<Outline>();
        Outline computer4Outline = computer4.GetComponent<Outline>();
        Outline computer5Outline = computer5.GetComponent<Outline>();
        Outline computer6Outline = computer6.GetComponent<Outline>();
        lightSwitchOutline.OutlineColor = Color.yellow;
        hearingAidOutline.OutlineColor = Color.yellow;
        sedativeOutline.OutlineColor = Color.yellow;
        coatOutline.OutlineColor = Color.yellow;
        computer1Outline.OutlineColor = Color.red;
        computer2Outline.OutlineColor = Color.red;
        computer3Outline.OutlineColor = Color.red;
        computer4Outline.OutlineColor = Color.red;
        computer5Outline.OutlineColor = Color.red;
        computer6Outline.OutlineColor = Color.red;
    }

    private void Protanopia() // blue for computer & yellow for others
    {
        Outline lightSwitchOutline = lightSwitch.GetComponent<Outline>();
        Outline hearingAidOutline = hearingAid.GetComponent<Outline>();
        Outline sedativeOutline = sedative.GetComponent<Outline>();
        Outline coatOutline = coat.GetComponent<Outline>();
        Outline computer1Outline = computer1.GetComponent<Outline>();
        Outline computer2Outline = computer2.GetComponent<Outline>();
        Outline computer3Outline = computer3.GetComponent<Outline>();
        Outline computer4Outline = computer4.GetComponent<Outline>();
        Outline computer5Outline = computer5.GetComponent<Outline>();
        Outline computer6Outline = computer6.GetComponent<Outline>();
        lightSwitchOutline.OutlineColor = Color.yellow;
        hearingAidOutline.OutlineColor = Color.yellow;
        sedativeOutline.OutlineColor = Color.yellow;
        coatOutline.OutlineColor = Color.yellow;
        computer1Outline.OutlineColor = Color.blue;
        computer2Outline.OutlineColor = Color.blue;
        computer3Outline.OutlineColor = Color.blue;
        computer4Outline.OutlineColor = Color.blue;
        computer5Outline.OutlineColor = Color.blue;
        computer6Outline.OutlineColor = Color.blue;

    }

    private void Deuteranopia() // blue for computer & yellow for others
    {
        
        Outline lightSwitchOutline = lightSwitch.GetComponent<Outline>();
        Outline hearingAidOutline = hearingAid.GetComponent<Outline>();
        Outline sedativeOutline = sedative.GetComponent<Outline>();
        Outline coatOutline = coat.GetComponent<Outline>();
        Outline computer1Outline = computer1.GetComponent<Outline>();
        Outline computer2Outline = computer2.GetComponent<Outline>();
        Outline computer3Outline = computer3.GetComponent<Outline>();
        Outline computer4Outline = computer4.GetComponent<Outline>();
        Outline computer5Outline = computer5.GetComponent<Outline>();
        Outline computer6Outline = computer6.GetComponent<Outline>();
        lightSwitchOutline.OutlineColor = Color.yellow;
        hearingAidOutline.OutlineColor = Color.yellow;
        sedativeOutline.OutlineColor = Color.yellow;
        coatOutline.OutlineColor = Color.yellow;
        computer1Outline.OutlineColor = Color.blue;
        computer2Outline.OutlineColor = Color.blue;
        computer3Outline.OutlineColor = Color.blue;
        computer4Outline.OutlineColor = Color.blue;
        computer5Outline.OutlineColor = Color.blue;
        computer6Outline.OutlineColor = Color.blue;


    }

    private void Tritanopia() // red for computer & cyan for others
    {
        Outline lightSwitchOutline = lightSwitch.GetComponent<Outline>();
        Outline hearingAidOutline = hearingAid.GetComponent<Outline>();
        Outline sedativeOutline = sedative.GetComponent<Outline>();
        Outline coatOutline = coat.GetComponent<Outline>();
        Outline computer1Outline = computer1.GetComponent<Outline>();
        Outline computer2Outline = computer2.GetComponent<Outline>();
        Outline computer3Outline = computer3.GetComponent<Outline>();
        Outline computer4Outline = computer4.GetComponent<Outline>();
        Outline computer5Outline = computer5.GetComponent<Outline>();
        Outline computer6Outline = computer6.GetComponent<Outline>();
        lightSwitchOutline.OutlineColor = Color.yellow;
        lightSwitchOutline.OutlineColor = Color.cyan;
        hearingAidOutline.OutlineColor = Color.cyan;
        sedativeOutline.OutlineColor = Color.cyan;
        coatOutline.OutlineColor = Color.cyan;
        computer1Outline.OutlineColor = Color.red;
        computer2Outline.OutlineColor = Color.red;
        computer3Outline.OutlineColor = Color.red;
        computer4Outline.OutlineColor = Color.red;
        computer5Outline.OutlineColor = Color.red;
        computer6Outline.OutlineColor = Color.red;

    }

}
