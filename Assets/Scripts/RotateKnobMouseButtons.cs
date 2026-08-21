using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler
{
    [Header("Visual Settings")]
    public Gradient gradient;
    [Tooltip("Drag the 'Fill' Image of the Slider here")]
    public Image fillImage; // Reference to the actual color part of the slider

    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject canvas;
    public Slider thermometerSlider;
    public GameObject foam;
    public GameObject steam;
    public GameObject check;
    private int tempstage = 0;
    private int potstate = 0;
    private int goaltemp = 0;
    private float timer = 0f;
    private float checkTimer = 0f;
    public float checkDuration = 1f;
    public float timeLimit = 10f;
    public float temperatureStep = 32f;
    public int winsneeded = 5;
    private int currentwins = 0;

    [Header("Rotation & Temperature Settings")]
    [Tooltip("Degrees of visual rotation for each single click")]
    public float rotationStep = 15f;
    private bool levelCompleted = false;

    void Start()
    {
        // Set the initial color based on the starting temperature
        UpdateGradientColor();
    }

    void increasestage()
    {
        if (tempstage < 2)
        {
            tempstage++;
            thermometerSlider.value += temperatureStep;
            transform.Rotate(0f, 0f, -rotationStep);

            // Update the color when temperature increases
            UpdateGradientColor();
        }
    }

    void decreasestage()
    {
        if (tempstage > 0)
        {
            tempstage--;
            thermometerSlider.value -= temperatureStep;
            transform.Rotate(0f, 0f, rotationStep);

            // Update the color when temperature decreases
            UpdateGradientColor();
        }
    }

    // --- NEW FUNCTION ---
    void UpdateGradientColor()
    {
        // Safety check to prevent errors if elements are missing
        if (fillImage != null && thermometerSlider != null)
        {
            // normalizedValue converts the slider's value (e.g., 0 to 100) into a perfect 0.0 to 1.0 range
            // Evaluate() picks the exact color on the gradient at that specific point
            fillImage.color = gradient.Evaluate(thermometerSlider.normalizedValue);
        }
    }

    void randompotstate()
    {
        potstate = Random.Range(0, 3);
        Debug.Log("Pot state is: " + potstate);
        updatepotgraphic();
    }

    void updatepotgraphic()
    {
        Debug.Log("Updating pot graphic for state: " + potstate);

        if (potstate == 2)
        {
            foam.SetActive(true);
            steam.SetActive(false);
        }
        else if (potstate == 1)
        {
            foam.SetActive(false);
            steam.SetActive(true);
        }
        else
        {
            foam.SetActive(false);
            steam.SetActive(false);
        }
    }

    void getgoaltemp()
    {
        if (potstate == tempstage)
        {
            goaltemp = 2;
        }
        else if (potstate == tempstage + 1 || potstate == tempstage - 2)
        {
            goaltemp = 1;
        }
        else
        {
            goaltemp = 0;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        checkTimer += Time.deltaTime;

        if (checkTimer >= checkDuration)
        {
            check.SetActive(false);
        }

        if (timer >= timeLimit)
        {
            if (goaltemp == tempstage)
            {
                checkTimer = 0f;
                check.SetActive(true);
                currentwins++;
                Debug.Log("Correct! Current wins: " + currentwins);
            }
            else
            {
                Debug.Log("Incorrect! Current wins: " + currentwins);
            }
            randompotstate();
            getgoaltemp();
            timer = 0f;
        }

        if (currentwins >= winsneeded && !levelCompleted)
        {
            Debug.Log("LEVEL COMPLETE!");
            levelCompleted = true;
            GoToNextLevel();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelCompleted) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            decreasestage();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            increasestage();
        }
    }

    void GoToNextLevel()
    {
        if (NextLevel != null && canvas != null)
        {
            var newLevel = Instantiate(NextLevel);
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }

        // Reset the cursor safely before moving on
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }
}