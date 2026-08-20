using UnityEngine;
using TMPro;

public class Level4Manager : Monobehaviour
{
    private string [] correctSequence = 
    { "drain", "press", "flip"
    };

    private int currentStep = 0;

    public TMP_Text progressText; // Reference to the TextMeshProUGUI component for displaying progress

    void Start()
    {
        UpdateProgress();
    }
    public void ClickProcess(stringprocess)

    {
        if (process == correctSequence[currentStep])
        {
            currentStep++;
            UpdateProgress();

            if (currentStep == correctSequence.Length)
           {
            LevelComplete();
           }

        }
        else
        {
            Debug.Log("Wrong step! Cheese quality drops.");
            // You can add code here to decrease cheese quality or handle the wrong step
        }

    }

    void UpdateProgress()
    {
    }

    void LevelComplete()
    {
       
    }

    public void ResetSequence ()

    currentStep = 0;
    UpdateProgress();
    }
}