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

    public void ClickProcess(string process)
    {

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
            Debug.Log("Wrong! RESETing sequence.");
            ResetSequence();
        }
    }


    public void ResetSequence()
    {
        currentStep = 0;
        Debug.Log("Sequence reset. Try again!");
    }


    void LevelComplete()
    {
        Debug.Log("LEVEL 4 COMPLETE!");
    }
}