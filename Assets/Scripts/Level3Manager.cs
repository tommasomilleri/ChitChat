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
        }
    }

    void UpdateText()
    {
        orderText.text = "Added: " + currentStep + " / 4";
    }

    void LevelComplete()
    {
        Debug.Log("LEVEL 3 COMPLETE!");
    }
}