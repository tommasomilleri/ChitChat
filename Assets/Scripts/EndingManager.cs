using UnityEngine;
using TMPro; // Required for TextMeshPro UI elements!

public class EndingManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag the TextMeshPro UI element here")]
    public TextMeshProUGUI storyText;

    [Header("Story Texts")]
    [TextArea(4, 8)]
    public string highEnding = "At long last, the Grand Fromagerie has yielded its ultimate masterpiece. Through perfect alchemy and unbreakable fellowship, you have forged a cheese so divine it has awakened the dormant strength of our kin. We are no longer mere shadows scurrying in the dark! Armed with the legendary recipe, the rats march back to the sunlit surface to overthrow their oppressors. The greedy humans, who for generations hoarded the dairy treasures of the world, can only watch in awe and despair as you save the cheese empire from human greed. A new golden age has begun!";
    [TextArea(4, 8)]
    public string mediumEnding = "The vats have cooled, and the final wheel rests upon the cellar shelves, yet a vital spark is missing. You have managed to salvage fragments of the ancient recipe, but it is not quite finished. It is a noble effort that will sustain our kin, but it lacks the magic required to break our chains. Because the cheese is imperfect, the humans remain in power above, their iron grip on the surface unbroken. We must survive in the shadows, sharing our hard-won secrets in whispers, waiting for a future generation to finally complete the great work.";

    [TextArea(4, 8)]
    public string lowEnding = "A foul stench rises from the vat, a putrid testament to your absolute failure. The milk has soured, the sacred temperatures were forsaken, and human greed takes over[cite: 1]. From the ruined curds, a vile and ancient evil awakens: the mold monster is born[cite: 3]! The Queen Rat, gazing upon the corrupted remains of our once-proud empire, has pronounced her final, devastating judgment. You are stripped of your apron and exiled into the cold, unforgiving wilds. The cheese empire crumbles into toxic rot, lost to the shadows forever.";
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