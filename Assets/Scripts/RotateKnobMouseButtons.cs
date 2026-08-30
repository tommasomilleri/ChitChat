using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings (Termometro Realistico)")]
    public Gradient gradient;
    public Image thermometerFill;

    [Header("I Tre Topolini (Zone Target)")]
    public GameObject blueMouse;
    public GameObject yellowMouse;
    public GameObject redMouse;

    [Header("UI & Level Management")]
    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject foam;
    public GameObject steam;
    public GameObject check;

    [Header("Orologio Analogico (Infallibile)")]
    public RectTransform timerHand;
    public bool tickMovement = true;
    [Tooltip("I gradi di distanza tra un numero e l'altro sull'orologio (Lascia 30)")]
    public float degreesPerTick = 30f;
    [Tooltip("L'angolo base per far puntare la lancetta in alto (Lascia 90)")]
    public float clockOffset = 90f;

    [Header("Fisica dell'Inerzia (Difficoltà)")]
    [Range(0.01f, 1f)] public float clickForce = 0.15f;
    [Range(0.1f, 5f)] public float friction = 1.5f;

    [Header("Audio Dinamico")]
    public AudioSource audioSource;
    public AudioClip knobClickSound;
    public float minPitch = 0.7f;
    public float maxPitch = 1.5f;

    [Header("Game Balance")]
    public float rotationStep = 15f;
    [Range(1f, 12f)] public float cookingTimeRequired = 8f;
    public int winsNeeded = 3;

    public int wrongPenalty = 15;
    public float errorTolerance = 4f;

    // --- VARIABILI INTERNE ---
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
        if (thermometerFill != null) currentFillAmount = thermometerFill.fillAmount;
        RandomizePotState();
    }

    void Update()
    {
        if (levelCompleted) return;

        // =========================================================
        // 1. FISICA DELL'INERZIA E DELLO SCIVOLAMENTO
        // =========================================================
        if (thermometerFill != null)
        {
            fillVelocity = Mathf.Lerp(fillVelocity, 0f, friction * Time.deltaTime);
            currentFillAmount += fillVelocity * Time.deltaTime;

            if (currentFillAmount <= 0f || currentFillAmount >= 1f)
            {
                fillVelocity = 0f;
                currentFillAmount = Mathf.Clamp01(currentFillAmount);
            }

            thermometerFill.fillAmount = currentFillAmount;
            thermometerFill.color = gradient.Evaluate(currentFillAmount);
        }

        // =========================================================
        // 2. LOGICA DELLE ZONE
        // =========================================================
        if (currentFillAmount <= 0.33f) currentZone = 0;
        else if (currentFillAmount <= 0.66f) currentZone = 1;
        else currentZone = 2;

        UpdateMiceVisuals();

        // =========================================================
        // 3. TIMER DI COTTURA SPIETATO
        // =========================================================
        if (check.activeSelf == false)
        {
            if (currentZone == targetZone)
            {
                currentErrorTime = 0f;
                currentCookingTime += Time.deltaTime;

                if (currentCookingTime >= cookingTimeRequired)
                {
                    RoundWon();
                }
            }
            else
            {
                currentCookingTime = 0f;
                currentErrorTime += Time.deltaTime;

                if (currentErrorTime >= errorTolerance)
                {
                    currentErrorTime = 0f;
                    if (GameManager.instance != null) GameManager.instance.DecreaseGlobalQuality(wrongPenalty); // [cite: 1]
                }
            }
        }

        // =========================================================
        // 4. OROLOGIO ANALOGICO (FORMULA MATEMATICA PERFETTA)
        // =========================================================
        if (timerHand != null)
        {
            float displayTime;

            if (check.activeSelf)
            {
                // VITTORIA! Congela la lancetta fiera sulla Corona (0 secondi rimasti)
                displayTime = 0f;
            }
            else
            {
                displayTime = cookingTimeRequired - currentCookingTime;
                displayTime = Mathf.Max(0f, displayTime);

                // Arrotonda per il tic-tac meccanico
                if (tickMovement) displayTime = Mathf.Ceil(displayTime);
            }

            // Formula Infallibile: L'offset a 90 corregge l'immagine, e 30 sono i gradi per ogni ora
            float rotationAngle = clockOffset - (displayTime * degreesPerTick);
            timerHand.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }

        // =========================================================
        // 5. SPUNTA VERDE E RIPARTENZA
        // =========================================================
        if (check.activeSelf)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= 1.5f)
            {
                check.SetActive(false);
                checkTimer = 0f;
                RandomizePotState();
            }
        }
    }

    void RoundWon()
    {
        currentWins++;
        currentCookingTime = 0f;
        fillVelocity = 0f; // Ferma il liquido

        check.SetActive(true);
        checkTimer = 0f;

        if (currentWins >= winsNeeded) LevelComplete();
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
    // CONTROLLI MOUSE (AGGIUNGONO VELOCITÀ AL LIQUIDO)
    // =========================================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelCompleted) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            fillVelocity -= clickForce;
            transform.Rotate(0f, 0f, rotationStep);
            PlayDynamicClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            fillVelocity += clickForce;
            transform.Rotate(0f, 0f, -rotationStep);
            PlayDynamicClick();
        }
    }

    void PlayDynamicClick()
    {
        if (audioSource != null && knobClickSound != null)
        {
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, currentFillAmount);
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
        if (NextLevel != null) NextLevel.SetActive(true);
        if (CurrentLevel != null) CurrentLevel.SetActive(false);
    }
}