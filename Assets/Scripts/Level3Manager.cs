using UnityEngine;
using TMPro;

public class Level3Manager : MonoBehaviour
{
    [Header("Recipe Settings")]
    public string[] correctOrder = { "Milk", "Rennet", "Salt", "Annatto" };
    private int currentStep = 0;

    [Header("UI & Level Management")]
    public TMP_Text orderText;
    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject canvas;

    [Header("Quality Settings")]
    public QualityBar qualityBar;
    public int wrongAnswerPenalty = 20;

    void Start()
    {
        UpdateText();

        // Sync the local QualityBar with the global GameManager score at start!
        if (qualityBar != null && GameManager.instance != null)
        {
            qualityBar.SetMaxQuality(GameManager.instance.maxQuality);
            qualityBar.SetQuality(GameManager.instance.currentQuality);
        }
    }

    public void CheckIngredient(DraggableIngredient ingredient)
    {
        // Prevents IndexOutOfRangeException if extra ingredients are triggered
        if (currentStep >= correctOrder.Length) return;

        if (ingredient == null)
        {
            Debug.LogWarning("No ingredient passed to CheckIngredient!");
            return;
        }

        if (ingredient.ingredientName == correctOrder[currentStep])
        {
            Debug.Log("Correct ingredient!");
            currentStep++;
            ingredient.gameObject.SetActive(false);
            UpdateText();

            if (currentStep == correctOrder.Length)
            {
                LevelComplete();
            }
        }
        else
        {
            Debug.Log("Wrong ingredient! Quality drops.");

            // 1. Decrease global quality
            if (GameManager.instance != null)
            {
                GameManager.instance.DecreaseGlobalQuality(wrongAnswerPenalty);

                // 2. Update the local UI bar
                if (qualityBar != null)
                {
                    qualityBar.SetQuality(GameManager.instance.currentQuality);
                }

                // 3. Trigger immediate ending if quality hits zero
                if (GameManager.instance.currentQuality <= 0)
                {
                    Debug.Log("Quality hit zero! Triggering ending scene.");
                    GameManager.instance.TriggerEnding();
                }
            }

            // Note: You can add logic here to snap the ingredient back to its starting position!
        }
    }

    void UpdateText()
    {
        if (orderText != null)
        {
            orderText.text = "Added: " + currentStep + " / " + correctOrder.Length;
        }
        else
        {
            Debug.LogWarning("orderText has not been assigned in the Inspector!");
        }
    }

    void LevelComplete()
    {
        Debug.Log("LEVEL 3 COMPLETE!");
        GoToNextLevel();
    }

    void GoToNextLevel()
    {
        if (NextLevel != null && canvas != null)
        {
            var newLevel = Instantiate(NextLevel);
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
    }
}