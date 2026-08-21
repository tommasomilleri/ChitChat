using UnityEngine;
using TMPro; // Required for TextMeshPro UI elements!

public class EndingManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag the TextMeshPro UI element here")]
    public TextMeshProUGUI storyText;

    [Header("Story Texts")]
    [TextArea(3, 5)]
    public string highEnding = "You saved the cheese empire from human greed!";

    [TextArea(3, 5)]
    public string mediumEnding = "You saved the cheese recipe but human greed took over....";

    [TextArea(3, 5)]
    public string lowEnding = "The queen";

    void Start()
    {
        // Safety check to ensure the global GameManager exists
        if (GameManager.instance != null)
        {
            int quality = GameManager.instance.currentQuality;

            // High Quality (80 to 100)
            if (quality >= 80)
            {
                storyText.text = highEnding;
            }
            // Medium Quality (40 to 79)
            else if (quality >= 40)
            {
                storyText.text = mediumEnding;
            }
            // Low Quality (below 40)
            else
            {
                storyText.text = lowEnding;
            }
        }
        else
        {
            Debug.LogWarning("GameManager not found! Start the game from the first scene to test.");
        }
    }
}