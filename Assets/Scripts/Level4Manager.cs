using UnityEngine;
using System.Collections; // Fondamentale per le Coroutine (animazioni)!

public class Level4Manager : MonoBehaviour
{
    [Header("Co-op Sequence")]
    public string[] correctSequence = { "drain", "press", "flip", "press", "flip" };
    private int currentStep = 0;

    [Header("Penalty Settings")]
    public int wrongActionPenalty = 20;

    [Header("Level Transitions")]
    public GameObject NextLevel;
    public GameObject CurrentLevel;

    [Header("Graphics to Animate")]
    [Tooltip("Trascina qui l'immagine della Pressa (Rossa)")]
    public RectTransform pressGraphic;
    [Tooltip("Trascina qui l'immagine del Formaggio (Giallo)")]
    public RectTransform flipGraphic;
    [Tooltip("Trascina qui l'immagine della Tela (Scolatura)")]
    public RectTransform drainGraphic;

    // Blocco di sicurezza: impedisce di cliccare mentre un'animazione è in corso
    private bool isAnimating = false;

    public void ClickProcess(string process)
    {
        // Se un'animazione è in corso, ignora i nuovi click
        if (isAnimating) return;

        // --- AZIONE CORRETTA ---
        if (process == correctSequence[currentStep])
        {
            Debug.Log("Correct: " + process);
            currentStep++;

            // Lancia l'animazione specifica in base alla parola
            if (process == "press") StartCoroutine(SquishAnimation(pressGraphic));
            else if (process == "flip") StartCoroutine(RotateAnimation(flipGraphic));
            else if (process == "drain") StartCoroutine(VibrateAnimation(drainGraphic));

            // Controlla se l'intera sequenza è finita
            if (currentStep == correctSequence.Length)
            {
                StartCoroutine(CompleteLevelRoutine());
            }
        }
        // --- AZIONE SBAGLIATA ---
        else
        {
            Debug.Log("Wrong! RESETing sequence.");
            ResetSequence();

            // Sottrae punti alla barra globale usando il GameManager[cite: 1, 4]
            if (GameManager.instance != null)
            {
                GameManager.instance.DecreaseGlobalQuality(wrongActionPenalty);
            }
        }
    }

    public void ResetSequence()
    {
        currentStep = 0;
        Debug.Log("Sequence reset. Try again!");
    }

    // ==========================================
    // LE 3 ANIMAZIONI (COROUTINES)
    // ==========================================

    IEnumerator SquishAnimation(RectTransform target)
    {
        if (target == null) yield break;
        isAnimating = true;
        Vector3 originalScale = target.localScale;

        // Schiaccia in basso (Y) e allarga (X)
        target.localScale = new Vector3(originalScale.x * 1.15f, originalScale.y * 0.7f, originalScale.z);
        yield return new WaitForSeconds(0.15f); // Aspetta un istante

        // Rimbalza alla normalità
        target.localScale = originalScale;
        isAnimating = false;
    }

    IEnumerator RotateAnimation(RectTransform target)
    {
        if (target == null) yield break;
        isAnimating = true;

        float duration = 0.3f; // Ci mette 0.3 secondi a girare
        float elapsed = 0f;
        Quaternion startRot = target.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, -180f); // Ruota di mezzo giro

        while (elapsed < duration)
        {
            // Movimento fluido da startRot a endRot
            target.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localRotation = endRot;
        isAnimating = false;
    }

    IEnumerator VibrateAnimation(RectTransform target)
    {
        if (target == null) yield break;
        isAnimating = true;

        Vector3 originalPos = target.localPosition;
        float duration = 0.4f; // Vibra per 0.4 secondi
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Sposta il target a caso di pochissimi pixel per simulare lo scuotimento
            target.localPosition = originalPos + new Vector3(Random.Range(-6f, 6f), Random.Range(-4f, 4f), 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Torna esattamente al centro
        target.localPosition = originalPos;
        isAnimating = false;
    }

    // ==========================================
    // FINE LIVELLO
    // ==========================================

    IEnumerator CompleteLevelRoutine()
    {
        isAnimating = true; // Blocca tutto
        Debug.Log("LEVEL 4 COMPLETE!");

        // Pausa di mezzo secondo per far finire l'ultima animazione prima di cambiare livello
        yield return new WaitForSeconds(0.5f);

        GoToNextLevel();
    }

    void GoToNextLevel()
    {
        // Accende il nuovo UI e spegne il vecchio[cite: 1, 6]
        if (NextLevel != null) NextLevel.SetActive(true);
        if (CurrentLevel != null) CurrentLevel.SetActive(false);
    }
}