using UnityEngine;

public class Level4Manager : MonoBehaviour
{
    private string[] correctSequence =
    {
        "drain",
        "press",
        "flip",
        "press",
        "flip"
    };

    private int currentStep = 0;

    // Becomes true when the player makes a mistake
    private bool sequenceIsWrong = false;


    public void ClickProcess(string process)
    {
        // If they already made a mistake,
        // they need to reset first
        if (sequenceIsWrong)
        {
            Debug.Log("Sequence is wrong. Press RESET to try again.");
            return;
        }

        // Check the current click
        if (process == correctSequence[currentStep])
        {
            Debug.Log("Correct: " + process);

            currentStep++;

            // Check if the entire sequence is complete
            if (currentStep == correctSequence.Length)
            {
                LevelComplete();
            }
        }
        else
        {
            Debug.Log("Wrong! Press RESET to try again.");

            sequenceIsWrong = true;
        }
    }


    public void ResetSequence()
    {
        currentStep = 0;
        sequenceIsWrong = false;

        Debug.Log("Sequence reset. Try again!");
    }


    void LevelComplete()
    {
        Debug.Log("LEVEL 4 COMPLETE!");
    }
}