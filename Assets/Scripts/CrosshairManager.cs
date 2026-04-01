using UnityEngine;

public class CrosshairManager : MonoBehaviour
{

    public int Crosshair;
    public RectTransform uiElement; // Assign in Inspector
    public Vector3 smallScale = new Vector3(0.06f, 0.06f, 0.06f);
    public Vector3 mediumScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 largeScale = new Vector3(0.3f, 0.3f, 0.3f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCrossHair();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCrossHair()
    {
        if (PlayerPrefs.HasKey("Crosshair"))
        {
            Crosshair = PlayerPrefs.GetInt("Crosshair");
        }
        else Crosshair = 0;


        switch (Crosshair)
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
