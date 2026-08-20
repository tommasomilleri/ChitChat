using UnityEngine;

public class Level5Manager : MonoBehaviour
{
    public int correctSlot = 5;

    public void CheckPosition(int slotNumber)
    {
        if (slotNumber == correctSlot)
        {
            Debug.Log("Correct shelf position!");

            LevelComplete();
        }
        else
        {
            Debug.Log("Wrong shelf position!");
        }
    }

    void LevelComplete()
    {
        Debug.Log("LEVEL 5 COMPLETE!");
    }
}