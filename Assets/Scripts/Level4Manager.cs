using UnityEngine;

public class Level4Manager : MonoBehaviour
{
    public GameObject NextLevel;
    public GameObject CurrentLevel; 
    public GameObject canvas;
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
