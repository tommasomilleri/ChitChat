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
    [Tooltip("L'angolo base per far puntare la lancetta in alto in base al tuo disegno")]
    public float clockOffset = -140f; // <-- Valore reso permanente!

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
    [Range(1f, 24f)] public float cookingTimeRequired = 12f;
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
    private bool hasStarted = false; // LA NOVITÀ: Blocca il gioco finché non clicchi!
    private Vector3 originalKnobScale;

    void Start()
    {
        originalKnobScale = transform.localScale;
        if (thermometerFill != null) currentFillAmount = thermometerFill.fillAmount;

        // Estrae subito lo stato così i giocatori possono comunicare, ma il timer è in pausa!
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
        // 3. TIMER DI COTTURA SPIETATO (Attivo solo dopo il 1° click)
        // =========================================================
        if (hasStarted && check.activeSelf == false)
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
                    if (GameManager.instance != null) GameManager.instance.DecreaseGlobalQuality(wrongPenalty);
                }
            }
        }

        // =========================================================
        // 4. OROLOGIO ANALOGICO
        // =========================================================
        if (timerHand != null)
        {
            float displayTime;

            // Se il gioco non è iniziato, o se c'è la spunta di vittoria, tieni l'orologio fermo a zero!
            if (!hasStarted || check.activeSelf)
            {
                displayTime = 0f;
            }
            else
            {
                displayTime = currentCookingTime;
                if (tickMovement) displayTime = Mathf.Floor(displayTime);
            }

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
        fillVelocity = 0f; // Ferma il liquido per farti respirare

        check.SetActive(true);
        checkTimer = 0f;

        if (currentWins >= winsNeeded) LevelComplete();
    }

    void RandomizePotState()
    {
        potState = Random.Range(0, 3);
        if (steam != null) steam.SetActive(potState == 1);
        if (foam != null) foam.SetActive(potState == 2);

        // 1. Calcoliamo la colonna di partenza (0=Blu, 1=Giallo, 2=Rosso)
        int startingColumn = 0;
        if (currentFillAmount <= 0.33f) startingColumn = 0;
        else if (currentFillAmount <= 0.66f) startingColumn = 1;
        else startingColumn = 2;

        // 2. Logica della Matrice basata su Screenshot 2026-08-30 155550.jpg
        if (potState == 0) // RIGA 1: Pentola Vuota
        {
            if (startingColumn == 0) targetZone = 2;      // Col Blu -> Target Rosso
            else if (startingColumn == 1) targetZone = 0; // Col Gialla -> Target Blu
            else if (startingColumn == 2) targetZone = 1; // Col Rossa -> Target Giallo
        }
        else if (potState == 1) // RIGA 2: Vapore (Steam)
        {
            if (startingColumn == 0) targetZone = 1;      // Col Blu -> Target Giallo
            else if (startingColumn == 1) targetZone = 2; // Col Gialla -> Target Rosso
            else if (startingColumn == 2) targetZone = 0; // Col Rossa -> Target Blu
        }
        else if (potState == 2) // RIGA 3: Bolle (Foam)
        {
            if (startingColumn == 0) targetZone = 0;      // Col Blu -> Target Blu
            else if (startingColumn == 1) targetZone = 1; // Col Gialla -> Target Giallo
            else if (startingColumn == 2) targetZone = 2; // Col Rossa -> Target Rosso
        }
    }

    // =========================================================
    // CONTROLLI MOUSE (Fanno partire il gioco!)
    // =========================================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelCompleted) return;

        // SBLOCCA IL GIOCO AL PRIMO CLICK!
        if (!hasStarted) hasStarted = true;

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