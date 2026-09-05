using UnityEngine;
using UnityEngine.Events; // Necessario per collegare le transizioni
using System.Collections; // Necessario per i ritardi di tempo

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Global Cheese Quality")]
    public int maxQuality = 100;
    public int currentQuality;

    [Header("Statistiche Partita")]
    public int[] errorsPerLevel = new int[6]; // Indici da 1 a 5 per i livelli
    public int currentLevel = 1;
    public float runStartTime;

    [Tooltip("Drag the QualityBar UI object here just ONCE!")]
    public QualityBar globalQualityBar; // Riferimento alla tua barra UI
    public GameObject qualityBarContainer; // AGGIUNGI QUESTA RIGA!

    [Header("Story Endings Settings")]
    public GameObject endingPanel;

    [Header("Transition Settings (NOVITA')")]
    [Tooltip("Collega qui la funzione PlayTransition() del tuo SceneTransitioner")]
    public UnityEvent onPlayTransition;
    public float transitionDelay = 1.5f;
    [Header("Loading Screen Universale")]
    public FakeLoadingScreen globalLoadingScreen; // <-- AGGIUNGI QUESTA!
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad RIMOSSO: il gioco vive in una sola scena a pannelli,
        // e questo era il meccanismo che creava i GameManager fantasma.
        currentQuality = maxQuality;
    }
    void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    void Start()
    {
        // All'inizio del gioco, imposta la barra al massimo!
        if (globalQualityBar != null)
        {
            globalQualityBar.SetMaxQuality(maxQuality);
            globalQualityBar.SetQuality(currentQuality);
            globalQualityBar.gameObject.SetActive(false);
        }
    }

    public void DecreaseGlobalQuality(int damage)
    {
        currentQuality -= damage;
        // Segna un errore sul taccuino per il livello in cui ci troviamo!
        if (currentLevel >= 1 && currentLevel <= 5)
        {
            errorsPerLevel[currentLevel]++;
        }
        // Il Pugno Visivo: ogni errore in ogni livello fa tremare lo schermo e ferma il tempo!
        if (GameFeel.Instance != null)
        {
            GameFeel.Instance.Shake();
            GameFeel.Instance.HitStop();
        }
        if (currentQuality <= 0)
        {
            currentQuality = 0;
            Debug.Log("Quality hit zero! Instant Bad Ending!");
            TriggerEnding();

            // --- LA NUOVA RIGA: Spegne (nasconde) l'oggetto UI della barra ---
            if (globalQualityBar != null)
            {
                globalQualityBar.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Global Quality is now: " + currentQuality);

            // Aggiorna la barra solo se abbiamo ancora punti
            if (globalQualityBar != null)
            {
                globalQualityBar.SetQuality(currentQuality);
            }
        }
    }

    public void TriggerEnding()
    {
        Debug.Log("Triggering Ending Panel. Final Quality: " + currentQuality);

        if (endingPanel != null)
        {
            endingPanel.SetActive(true);
        }

        if (qualityBarContainer != null)
        {
            qualityBarContainer.SetActive(false);
        }
    }

    public void ResetQuality()
    {
        currentQuality = maxQuality;
        runStartTime = Time.time; // Fa ripartire il cronometro
        errorsPerLevel = new int[6]; // Azzera il taccuino degli errori
        if (globalQualityBar != null)
        {
            globalQualityBar.SetMaxQuality(maxQuality);
            globalQualityBar.SetQuality(currentQuality);
            globalQualityBar.gameObject.SetActive(false);
        }
        if (endingPanel != null) endingPanel.SetActive(false);
    }

    // --- LA MAGIA DELLE TRANSIZIONI ---
    public void TransitionBetweenPanels(GameObject currentPanel, GameObject nextPanel)
    {
        // 1. Lancia l'animazione di transizione (fumo/bolle)
        if (onPlayTransition != null && onPlayTransition.GetPersistentEventCount() > 0)
        {
            onPlayTransition.Invoke();
            StartCoroutine(SwapAfterDelay(currentPanel, nextPanel, transitionDelay));
        }
        else
        {
            // 2. Fallback di emergenza
            if (nextPanel != null) nextPanel.SetActive(true);
            if (currentPanel != null) currentPanel.SetActive(false);
        }
    }
    // --- IL NUOVO PONTE UNIVERSALE ---
    public void TransitionToNextLevel(GameObject currentLevel, GameObject targetLevel)
    {
        if (globalLoadingScreen != null)
        {
            // 1. Dice al Loading Screen qual è la vera destinazione finale
            globalLoadingScreen.nextLevelPanel = targetLevel;

            // 2. Ti manda al Loading Screen
            TransitionBetweenPanels(currentLevel, globalLoadingScreen.gameObject);
        }
        else
        {
            // Fallback d'emergenza se dimentichi di collegare il Loading Screen
            TransitionBetweenPanels(currentLevel, targetLevel);
        }
    }

    private IEnumerator SwapAfterDelay(GameObject current, GameObject next, float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Realtime non si blocca con la pausa
        if (next != null) next.SetActive(true);
        if (current != null) current.SetActive(false);
    }
}