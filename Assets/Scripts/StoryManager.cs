using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Global Cheese Quality")]
    public int maxQuality = 100;
    public int currentQuality;

    [Tooltip("Drag the QualityBar UI object here just ONCE!")]
    public QualityBar globalQualityBar; // Riferimento alla tua barra UI
    public GameObject qualityBarContainer; // AGGIUNGI QUESTA RIGA!

    [Header("Story Endings Settings")]
    public GameObject endingPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentQuality = maxQuality;
        }
        else
        {
            Destroy(gameObject);
        }
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
}