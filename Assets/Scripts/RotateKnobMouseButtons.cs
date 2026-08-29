using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings (Termometro Realistico)")]
    public Gradient gradient;
    [Tooltip("Trascina qui l'immagine del liquido (Impostata su Image Type: Filled)")]
    public Image thermometerFill;

    [Header("I Tre Topolini (Zone Target)")]
    public GameObject blueMouse;   // Zona 0: 0.0 - 0.33
    public GameObject yellowMouse; // Zona 1: 0.33 - 0.66
    public GameObject redMouse;    // Zona 2: 0.66 - 1.0

    [Header("UI & Level Management")]
    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject foam;  // Bolle
    public GameObject steam; // Vapore
    public GameObject check;

    [Header("Cronometro Analogico")]
    public RectTransform timerHand;

    [Header("Fisica Fluida (Stabile)")]
    [Tooltip("Più il valore è basso, più il liquido scivola velocemente. (Consigliato: 0.15)")]
    [Range(0.05f, 0.5f)] public float fluidViscosity = 0.15f;

    [Header("Audio Dinamico")]
    public AudioSource audioSource;
    public AudioClip knobClickSound;
    public float minPitch = 0.7f;
    public float maxPitch = 1.5f;

    [Header("Game Balance")]
    public float rotationStep = 15f;

    [Tooltip("Percentuale di riempimento per ogni click (es. 0.15 = 15%)")]
    [Range(0.01f, 0.5f)]
    public float temperatureStep = 0.15f;

    public float cookingTimeRequired = 8f;
    public int winsNeeded = 3;

    public int wrongPenalty = 15;
    public float errorTolerance = 4f;

    // --- VARIABILI INTERNE ---
    private float targetFillAmount = 0f;
    private float currentFillAmount = 0f;
    private float fillVelocity = 0f;

    private int currentZone = 0;
    private int targetZone = 0;
    private int potState = 0;
    private int currentWins = 0;

    private float currentCookingTime = 0f;
    private float currentErrorTime = 0f;
    private float checkTimer = 0f;
    private bool levelCompleted = false;
    private Vector3 originalKnobScale;

    void Start()
    {
        originalKnobScale = transform.localScale;

        if (thermometerFill != null)
        {
            currentFillAmount = thermometerFill.fillAmount;
            targetFillAmount = currentFillAmount;
        }

        RandomizePotState();
    }

    void Update()
    {
        if (levelCompleted) return;

        // =========================================================
        // 1. FISICA FLUIDA STABILE (Niente esplosioni o blocchi!)
        // =========================================================
        if (thermometerFill != null)
        {
            // SmoothDamp insegue il bersaglio in modo morbido senza mai superare il limite
            currentFillAmount = Mathf.SmoothDamp(currentFillAmount, targetFillAmount, ref fillVelocity, fluidViscosity);

            thermometerFill.fillAmount = currentFillAmount;
            thermometerFill.color = gradient.Evaluate(Mathf.Clamp01(currentFillAmount));
        }

        // =========================================================
        // 2. LOGICA DELLE ZONE (Dalle zampe alla testa)
        // =========================================================
        if (currentFillAmount <= 0.33f) currentZone = 0;      // Blu
        else if (currentFillAmount <= 0.66f) currentZone = 1; // Giallo
        else currentZone = 2;                                 // Rosso

        UpdateMiceVisuals();

        // =========================================================
        // 3. LOGICA SPIETATA DEL TIMER
        // =========================================================
        if (currentZone == targetZone) // Sei nella zona giusta!
        {
            currentErrorTime = 0f;
            currentCookingTime += Time.deltaTime;

            if (currentCookingTime >= cookingTimeRequired)
            {
                RoundWon();
            }
        }
        else // Sei uscito dalla zona!
        {
            currentCookingTime = 0f; // Il timer si azzera ISTANTANEAMENTE!
            currentErrorTime += Time.deltaTime;

            if (currentErrorTime >= errorTolerance)
            {
                currentErrorTime = 0f;
                // Danneggia il GameManager globale[cite: 1]
                if (GameManager.instance != null) GameManager.instance.DecreaseGlobalQuality(wrongPenalty);
                Debug.Log("Il latte si sta rovinando!");
            }
        }

        // =========================================================
        // 4. UI (Cronometro e Spunta)
        // =========================================================
        if (timerHand != null)
        {
            float rotationAngle = (currentCookingTime / cookingTimeRequired) * -360f;
            timerHand.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }

        if (check.activeSelf)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= 1.5f)
            {
                check.SetActive(false);
                checkTimer = 0f;
            }
        }
    }

    void RoundWon()
    {
        currentWins++;
        currentCookingTime = 0f;
        check.SetActive(true);
        checkTimer = 0f;

        if (currentWins >= winsNeeded) LevelComplete();
        else RandomizePotState();
    }

    void RandomizePotState()
    {
        potState = Random.Range(0, 3);
        if (steam != null) steam.SetActive(potState == 1);
        if (foam != null) foam.SetActive(potState == 2);

        if (potState == 0) targetZone = 2;
        else if (potState == 1) targetZone = 0;
        else if (potState == 2) targetZone = 1;
    }

    // =========================================================
    // CONTROLLI MOUSE E AUDIO DINAMICO
    // =========================================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelCompleted) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            targetFillAmount = Mathf.Clamp(targetFillAmount - temperatureStep, 0f, 1f);
            transform.Rotate(0f, 0f, rotationStep);
            PlayDynamicClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            targetFillAmount = Mathf.Clamp(targetFillAmount + temperatureStep, 0f, 1f);
            transform.Rotate(0f, 0f, -rotationStep);
            PlayDynamicClick();
        }
    }

    void PlayDynamicClick()
    {
        if (audioSource != null && knobClickSound != null)
        {
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, targetFillAmount);
            audioSource.PlayOneShot(knobClickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { transform.localScale = originalKnobScale * 1.1f; }
    public void OnPointerExit(PointerEventData eventData) { transform.localScale = originalKnobScale; }

    void UpdateMiceVisuals()
    {
        if (blueMouse != null) blueMouse.SetActive(currentZone == 0);
        if (yellowMouse != null) yellowMouse.SetActive(currentZone == 1);
        if (redMouse != null) redMouse.SetActive(currentZone == 2);
    }

    void LevelComplete()
    {
        levelCompleted = true;
        if (check != null) check.SetActive(true);
        Invoke(nameof(GoToNextLevel), 1.5f);
    }

    void GoToNextLevel()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Transizione UI ottimizzata[cite: 1]
        if (NextLevel != null) NextLevel.SetActive(true);
        if (CurrentLevel != null) CurrentLevel.SetActive(false);
    }
}