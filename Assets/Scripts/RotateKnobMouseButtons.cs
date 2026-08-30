using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings (Termometro Realistico)")]
    public Gradient gradient;
    public Image thermometerFill;

    [Header("Rotazione Visiva Manopola (Limiti)")]
    [Tooltip("Angolo della manopola quando il liquido è a 0 (es. 140)")]
    public float minKnobAngle = 140f;
    [Tooltip("Angolo della manopola quando il liquido è a 1 (es. -140)")]
    public float maxKnobAngle = -140f;

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
    public float degreesPerTick = 30f;
    public float clockOffset = -140f;

    [Header("Fisica dell'Inerzia (Difficoltà)")]
    [Range(0.01f, 1f)] public float clickForce = 0.15f;
    [Range(0.1f, 5f)] public float friction = 1.5f;

    [Header("Audio SFX (Pentola & Manopola)")]
    public AudioSource audioSource; // Usa questo SOLO per pentola e manopola
    public AudioClip knobClickSound;
    public AudioClip steamSound;
    public AudioClip bubblesSound;

    [Header("Audio SFX (Orologio)")]
    public AudioSource clockAudioSource; // NUOVO: Dedicato solo all'orologio
    public AudioClip clockTickSound;     // NUOVO: La tua traccia da 8 secondi

    [Header("Game Balance")]
    public float rotationStep = 15f;
    [Range(1f, 24f)] public float cookingTimeRequired = 12f;
    public int winsNeeded = 3;
    public float errorTolerance = 1f;

    // --- VARIABILI INTERNE ---
    private float currentFillAmount = 0f;
    private float fillVelocity = 0f;
    private float currentKnobAngle = 0f;

    private int currentZone = 0;
    private int targetZone = 0;
    private int potState = 0;
    private int currentWins = 0;

    private float currentCookingTime = 0f;
    private float currentErrorTime = 0f;
    private float checkTimer = 0f;

    private bool levelCompleted = false;
    private bool hasStarted = false;
    private Vector3 originalKnobScale;

    void Start()
    {
        originalKnobScale = transform.localScale;
        if (thermometerFill != null) currentFillAmount = thermometerFill.fillAmount;

        currentKnobAngle = Mathf.Lerp(minKnobAngle, maxKnobAngle, currentFillAmount);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentKnobAngle);

        RandomizePotState();
    }

    void Update()
    {
        if (levelCompleted) return;

        // =========================================================
        // 1. FISICA DELL'INERZIA (SOLO PER IL LIQUIDO)
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
        // 3. TIMER DI COTTURA SPIETATO & AUDIO OROLOGIO
        // =========================================================
        if (hasStarted && check.activeSelf == false)
        {
            if (currentZone == targetZone)
            {
                currentErrorTime = 0f;
                currentCookingTime += Time.deltaTime;

                // GESTIONE AUDIO OROLOGIO CONTINUO
                if (clockAudioSource != null && clockTickSound != null)
                {
                    if (!clockAudioSource.isPlaying)
                    {
                        clockAudioSource.clip = clockTickSound;
                        clockAudioSource.loop = true; // Assicura che la traccia da 8 secondi riparta
                        clockAudioSource.Play();
                    }
                }

                if (currentCookingTime >= cookingTimeRequired)
                {
                    RoundWon();
                }
            }
            else
            {
                currentCookingTime = 0f;
                currentErrorTime += Time.deltaTime;

                // FERMA L'OROLOGIO: Il liquido è fuori zona, il timer si azzera!
                if (clockAudioSource != null && clockAudioSource.isPlaying)
                {
                    clockAudioSource.Stop();
                }

                if (currentErrorTime >= errorTolerance)
                {
                    currentErrorTime = 0f;
                }
            }
        }
        else
        {
            // Silenzia l'orologio durante le pause o prima del primissimo click
            if (clockAudioSource != null && clockAudioSource.isPlaying)
            {
                clockAudioSource.Stop();
            }
        }

        // =========================================================
        // 4. OROLOGIO ANALOGICO
        // =========================================================
        if (timerHand != null)
        {
            float displayTime;

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
                hasStarted = false;
                RandomizePotState();
            }
        }
    }

    void RoundWon()
    {
        currentWins++;
        currentCookingTime = 0f;
        fillVelocity = 0f;

        check.SetActive(true);
        checkTimer = 0f;

        if (audioSource != null) audioSource.Stop(); // Ferma la pentola
        if (clockAudioSource != null) clockAudioSource.Stop(); // Ferma l'orologio

        if (currentWins >= winsNeeded) LevelComplete();
    }

    void RandomizePotState()
    {
        potState = Random.Range(0, 3);
        if (steam != null) steam.SetActive(potState == 1);
        if (foam != null) foam.SetActive(potState == 2);

        if (audioSource != null)
        {
            audioSource.Stop();

            if (potState == 1 && steamSound != null)
            {
                audioSource.clip = steamSound;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if (potState == 2 && bubblesSound != null)
            {
                audioSource.clip = bubblesSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        int startingColumn = 0;
        if (currentFillAmount <= 0.33f) startingColumn = 0;
        else if (currentFillAmount <= 0.66f) startingColumn = 1;
        else startingColumn = 2;

        if (potState == 0) // Pentola Vuota
        {
            if (startingColumn == 0) targetZone = 2;
            else if (startingColumn == 1) targetZone = 0;
            else if (startingColumn == 2) targetZone = 1;
        }
        else if (potState == 1) // Vapore
        {
            if (startingColumn == 0) targetZone = 1;
            else if (startingColumn == 1) targetZone = 2;
            else if (startingColumn == 2) targetZone = 0;
        }
        else if (potState == 2) // Bolle
        {
            if (startingColumn == 0) targetZone = 0;
            else if (startingColumn == 1) targetZone = 1;
            else if (startingColumn == 2) targetZone = 2;
        }
    }

    // =========================================================
    // CONTROLLI MOUSE - SCATTI ISTANTANEI
    // =========================================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelCompleted) return;

        if (!hasStarted) hasStarted = true;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            fillVelocity -= clickForce;
            currentKnobAngle += rotationStep;
            ApplyClampedKnobRotation();
            PlayClickSound();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            fillVelocity += clickForce;
            currentKnobAngle -= rotationStep;
            ApplyClampedKnobRotation();
            PlayClickSound();
        }
    }

    void ApplyClampedKnobRotation()
    {
        float min = Mathf.Min(minKnobAngle, maxKnobAngle);
        float max = Mathf.Max(minKnobAngle, maxKnobAngle);

        currentKnobAngle = Mathf.Clamp(currentKnobAngle, min, max);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentKnobAngle);
    }

    void PlayClickSound()
    {
        if (audioSource != null && knobClickSound != null)
        {
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
        if (audioSource != null) audioSource.Stop();
        if (clockAudioSource != null) clockAudioSource.Stop();
        Invoke(nameof(GoToNextLevel), 1.5f);
    }

    void GoToNextLevel()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (NextLevel != null) NextLevel.SetActive(true);
        if (CurrentLevel != null) CurrentLevel.SetActive(false);
    }
}