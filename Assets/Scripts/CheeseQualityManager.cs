using UnityEngine;

public class CheeseQualityManager : MonoBehaviour
{
    // Singleton instance so any script can access this without Inspector references
    public static CheeseQualityManager Instance { get; private set; }

    [Header("Cheese Quality Settings")]
    public QualityBar qualityBar;
    public int maxQuality = 100;

    // Kept public so other scripts can read it, but it's no longer static
    public int currentQuality;

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Uncomment this if you start using multiple Unity Scenes later!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the game
        ResetGame();
    }

    public void DecreaseQuality(int damage)
    {
        currentQuality -= damage;

        if (currentQuality < 0)
        {
            currentQuality = 0;
        }

        if (qualityBar != null)
        {
            qualityBar.SetQuality(currentQuality);
        }

        if (currentQuality == 0)
        {
            CheeseSpoiled();
        }
    }

    void CheeseSpoiled()
    {
        Debug.Log("The cheese went bad! A Mold Monster is born!");
        // Add your Game Over logic here
    }

    public void ResetGame()
    {
        currentQuality = maxQuality;

        if (qualityBar != null)
        {
            qualityBar.SetMaxQuality(maxQuality);
            qualityBar.SetQuality(currentQuality);
        }
    }
}