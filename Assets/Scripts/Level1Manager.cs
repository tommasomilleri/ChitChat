using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int correctImage;
    public int wrongAnswerPenalty = 20;

    public GameObject Lvl2;
    public GameObject Lvl1;
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

            // Calls the global singleton manager to decrease the quality
            if (CheeseQualityManager.Instance != null)
            {
                CheeseQualityManager.Instance.DecreaseQuality(wrongAnswerPenalty);
            }
            else
            {
                Debug.LogWarning("CheeseQualityManager is missing from the scene!");
            }
        }
    }

    void GoToNextLevel()
    {
        if (Lvl2 != null && canvas != null)
        {
            var newLevel = Instantiate(Lvl2);
            // Passing 'false' prevents the UI from scaling weirdly when parented
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (Lvl1 != null)
        {
            Destroy(Lvl1);
        }
    }
}