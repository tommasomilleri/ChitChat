using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Cheese Quality Settings")]
    public QualityBar qualityBar; // Reference to the QualityBar script
    public int maxQuality = 100;
    private int currentQuality;
    public int wrongAnswerPenalty = 20; // Quality points lost for each mistake
    [Header("Level Settings")]
    public int correctImage;


    public GameObject Lvl2;
    public GameObject Lvl1;
    public GameObject canvas;

    void Start()
    {
        // At the start of the level, the cheese is perfect (Max Quality)
        currentQuality = maxQuality;
        qualityBar.SetMaxQuality(maxQuality);
    }

    public void selectImage(int imageNumber)
    {
        if (imageNumber == correctImage)
        {
            // Correct choice!
            Debug.Log("Correct choice! The cheesemaking process continues.");
            GoToNextLevel();
        }
        else
        {
            // Wrong choice!
            Debug.Log("Oh no! Wrong ingredient, cheese quality drops.");
            DecreaseQuality(wrongAnswerPenalty);
        }
    }

    void DecreaseQuality(int damage)
    {
        currentQuality -= damage;

        // Prevent quality from dropping below zero
        if (currentQuality < 0)
        {
            currentQuality = 0;
        }

        // Update the slider UI by calling the QualityBar script
        qualityBar.SetQuality(currentQuality);

        // Check if the cheese is completely spoiled
        if (currentQuality == 0)
        {
            CheeseSpoiled();
        }
    }

    void GoToNextLevel()
    {
        var newLevel = Instantiate(Lvl2);
        newLevel.transform.SetParent(canvas.transform);
        Destroy(Lvl1);
       
    }

    void CheeseSpoiled()
    {
        // Handle what happens when the game is lost (quality reaches 0)
        Debug.Log("The cheese went bad! A Mold Monster is born!");
    }
}