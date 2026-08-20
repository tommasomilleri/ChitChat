using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler
{

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

    void increasestage()
    {
        if (tempstage < 2)
        {
            tempstage++;
            thermometerSlider.value += temperatureStep;
            transform.Rotate(0f, 0f, -rotationStep);
        
        }
    }

    void decreasestage()
    {
        if (tempstage > 0)
        {
            tempstage--;
            thermometerSlider.value -= temperatureStep;
            transform.Rotate(0f, 0f, rotationStep);
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
        // Update the pot graphic based on the potstate
        // This is a placeholder; implement your own logic to change the pot's appearance
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

        if (checkTimer >= checkDuration) {
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
        // Ignore clicks if the target has already been reached
        if (levelCompleted) return;

        // LEFT CLICK: Turn left -> DECREASE temperature and counter
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            decreasestage();
        }
        // RIGHT CLICK: Turn right -> INCREASE temperature and counter
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

            // Passing 'false' prevents the UI from scaling weirdly when parented
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //Cursor.visible = true;
    }
}