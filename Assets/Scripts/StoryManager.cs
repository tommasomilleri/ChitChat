using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Global Cheese Quality")]
    public int maxQuality = 100;
    public int currentQuality;

    [Header("Story Endings Settings")]
    [Tooltip("The exact name of your SINGLE ending scene")]
    public string endingSceneName = "EndingScene";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentQuality = maxQuality;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseGlobalQuality(int damage)
    {
        currentQuality -= damage;
        if (currentQuality < 0) currentQuality = 0;

        Debug.Log("Global Quality is now: " + currentQuality);
    }

    // Call this exactly when the final level is completed!
    public void TriggerEnding()
    {
        Debug.Log("Triggering Ending Scene. Final Quality: " + currentQuality);
        SceneManager.LoadScene(endingSceneName);
    }
}