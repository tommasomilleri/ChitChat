using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int correctImage;
    public int wrongAnswerPenalty = 20;

    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject canvas;

    // Triggered by the UI Buttons
    public void selectImage(int imageNumber)
    {
        if (imageNumber == correctImage)
        {
            Debug.Log("Correct choice! The cheesemaking process continues.");
            GoToNextLevel();
        }
        else
        {
            Debug.Log("Oh no! Wrong ingredient, cheese quality drops.");

            // Chiama il GameManager globale invece del CheeseQualityManager!
            if (GameManager.instance != null)
            {
                GameManager.instance.DecreaseGlobalQuality(wrongAnswerPenalty);
            }
            else
            {
                Debug.LogWarning("GameManager is missing from the scene!");
            }
        }
    }

    void GoToNextLevel()
    {
        if (NextLevel != null && canvas != null)
        {
            var newLevel = Instantiate(NextLevel);

            // Passing 'false' prevents the UI from scaling weirdly when parented
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
    }
}