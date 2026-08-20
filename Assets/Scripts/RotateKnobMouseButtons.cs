using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    [Tooltip("Drag the Thermometer Slider from the Hierarchy here")]
    public Slider thermometerSlider;

    [Header("Rotation & Temperature Settings")]
    [Tooltip("Degrees of visual rotation for each single click")]
    public float rotationStep = 15f;

    [Tooltip("How much the temperature increases/decreases on the slider per click")]
    public float temperatureStep = 2f;

    [Header("Level Progression Settings")]
    [Tooltip("The number the counter must reach to trigger the next level")]
    public int targetCounter = 5;

    // Internal counter tracking the player's progress
    private int currentCounter = 0;

    // Safety check to prevent firing the end-level logic multiple times
    private bool levelCompleted = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Ignore clicks if the target has already been reached
        if (levelCompleted) return;

        // LEFT CLICK: Turn left -> DECREASE temperature and counter
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Rotate the knob left (counter-clockwise)
            transform.Rotate(0f, 0f, rotationStep);

            // Decrease the slider
            if (thermometerSlider != null)
            {
                thermometerSlider.value -= temperatureStep;
            }

            // Decrease the counter
            currentCounter--;
            Debug.Log("Counter decreased. Current count: " + currentCounter);
        }
        // RIGHT CLICK: Turn right -> INCREASE temperature and counter
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Rotate the knob right (clockwise)
            transform.Rotate(0f, 0f, -rotationStep);

            // Increase the slider
            if (thermometerSlider != null)
            {
                thermometerSlider.value += temperatureStep;
            }

            // Increase the counter
            currentCounter++;
            Debug.Log("Counter increased. Current count: " + currentCounter);

            // Check if the target number has been reached
            if (currentCounter >= targetCounter)
            {
                GoToNextLevel();
            }
        }
    }

    void GoToNextLevel()
    {
        levelCompleted = true;
        Debug.Log("Target temperature reached! Moving to the next level...");

        // NOTE: Here you will integrate your level transition logic.
        // If your LevelManager is on the same GameObject, you can call it like this:
        // GetComponent<LevelManager>().GoToNextLevel();

        // Alternatively, if you want to use public GameObjects to toggle UI panels 
        // like we did in the previous script, you can add them at the top of this script 
        // and do NextLevel.SetActive(true) and CurrentLevel.SetActive(false) here.
    }
}