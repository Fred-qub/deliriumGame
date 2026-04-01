using UnityEngine;

public class CrosshairManager : MonoBehaviour
{

    public int crosshairSize;
    public RectTransform uiElement; // Assign in Inspector
    public Vector3 smallScale = new Vector3(0.06f, 0.06f, 0.06f);
    public Vector3 mediumScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 largeScale = new Vector3(0.3f, 0.3f, 0.3f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCrossHairSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCrossHairSize()
    {
        if (PlayerPrefs.HasKey("CrosshairSize"))
        {
            crosshairSize = PlayerPrefs.GetInt("CrosshairSize");
        }
        else crosshairSize = 0;


        switch (crosshairSize)
        {
            case 0:
                uiElement.localScale = smallScale; // 9am
                break;

            case 1:
                uiElement.localScale = mediumScale;
                break;

            case 2:
                uiElement.localScale = largeScale; // 11pm
                break;



        }
    }
}
