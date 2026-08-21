using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern to access this script from anywhere
    public static GameManager instance;

    [Header("Global Cheese Quality")]
    public int maxQuality = 100;
    public int currentQuality;

    [Header("Story Endings (Scene Names)")]
    public string highQualityScene = "EndingHigh";
    public string mediumQualityScene = "EndingMedium";
    public string lowQualityScene = "EndingLow";

    void Awake()
    {
        // Ensure only one GameManager exists and it survives scene changes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Set perfect quality at the very beginning of the game
            currentQuality = maxQuality;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Other scripts will call this function to deal damage
    public void DecreaseGlobalQuality(int damage)
    {
        currentQuality -= damage;

        if (currentQuality < 0) currentQuality = 0;

        Debug.Log("Global Quality is now: " + currentQuality);
    }

    // Call this exactly when the final level is completed!
    public void TriggerEnding()
    {
        if (currentQuality >= 80)
        {
            Debug.Log("Triggering High Quality Ending!");
            SceneManager.LoadScene(highQualityScene);
        }
        else if (currentQuality >= 40)
        {
            Debug.Log("Triggering Medium Quality Ending!");
            SceneManager.LoadScene(mediumQualityScene);
        }
        else
        {
            Debug.Log("Triggering Low Quality Ending! Mold monster!");
            SceneManager.LoadScene(lowQualityScene);
        }
    }
}