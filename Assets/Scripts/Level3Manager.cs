using UnityEngine;
using TMPro;

public class Level3Manager : MonoBehaviour
{
    public string[] correctOrder =
    {
        "Milk",
        "Rennet",
        "Salt",
        "Annatto"
    };

    private int currentStep = 0;

    public TMP_Text orderText;

    void Start()
    {
        UpdateText();
    }

    public void CheckIngredient(DraggableIngredient ingredient)
    {
        // Prevents IndexOutOfRangeException if extra ingredients are triggered after level completion
        if (currentStep >= correctOrder.Length)
        {
            return;
        }

        // Safety check to prevent NullReferenceException
        if (ingredient == null)
        {
            Debug.LogWarning("No ingredient passed to CheckIngredient!");
            return;
        }

        if (ingredient.ingredientName == correctOrder[currentStep])
        {
            Debug.Log("Correct ingredient!");

            currentStep++;

            // Ingredient disappears after being added
            ingredient.gameObject.SetActive(false);

            UpdateText();

            if (currentStep == correctOrder.Length)
            {
                LevelComplete();
            }
        }
        else
        {
            Debug.Log("Wrong ingredient!");
            // Optional: You can call a function here to reset the wrong ingredient's position
        }
    }

    void UpdateText()
    {
        if (orderText != null)
        {
            // Replaced the hardcoded "4" with correctOrder.Length to make the code dynamic
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
        // Logic to load the next level or show the victory menu will go here
    }
}