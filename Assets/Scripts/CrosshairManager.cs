using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{

    public int crosshairSize;
    public string crosshairColour;
    public RectTransform crosshairTransform; // Assign in Inspector
    public Graphic crosshairGraphic; // Assign in Inspector
    private Vector3 smallScale = new Vector3(0.06f, 0.06f, 0.06f);
    private Vector3 mediumScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 largeScale = new Vector3(0.3f, 0.3f, 0.3f);



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCrossHairSize();
        LoadCrossHairColour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCrossHairSize()
    {
        if (PlayerPrefs.HasKey("CrosshairSize"))
        {
            crosshairSize = PlayerPrefs.GetInt("CrosshairSize");
        }
        else crosshairSize = 0;


        switch (crosshairSize)
        {
            case 0:
                crosshairTransform.localScale = smallScale; 
                break;

            case 1:
                crosshairTransform.localScale = mediumScale;
                break;

            case 2:
                crosshairTransform.localScale = largeScale; 
                break;

        }
    }

    private void LoadCrossHairColour()
    {
        if (PlayerPrefs.HasKey("CrosshairColour"))
        {
            crosshairColour = PlayerPrefs.GetString("CrosshairColour");
        }
        else crosshairColour = "White";


        switch (crosshairColour)
        {
            case "White":
                crosshairGraphic.color = Color.white;
                break;

            case "Red":
                crosshairGraphic.color = Color.red;
                break;

            case "Blue":
                crosshairGraphic.color = Color.blue;
                break;

            case "Yellow":
                crosshairGraphic.color = Color.yellow;
                break;

            case "Orange":
                crosshairGraphic.color = Color.orange;
                break;

        }
    }
}
