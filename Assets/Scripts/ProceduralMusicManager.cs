using UnityEngine;
using System.Collections; // Necessario per le Coroutine

[RequireComponent(typeof(AudioSource))]
public class ProceduralMusicManager : MonoBehaviour
{
    public static ProceduralMusicManager instance;

    [Header("Music Settings")]
    [Tooltip("Trascina qui il tuo file .wav o .mp3")]
    public AudioClip backgroundTrack;

    [Tooltip("Quanti secondi ci mette la musica ad arrivare al massimo volume? (Fade-In)")]
    public float fadeInDuration = 4f;

    [Tooltip("Il volume massimo desiderato (da 0 a 1)")]
    [Range(0f, 1f)]
    public float targetVolume = 0.4f;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern per non interrompere la musica al cambio livello
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (backgroundTrack != null)
        {
            audioSource.clip = backgroundTrack;
            audioSource.loop = true;
            audioSource.playOnAwake = false; // Lo gestiamo noi via codice!

            // Impostiamo il volume a 0 per preparare il Fade-In
            audioSource.volume = 0f;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                // Facciamo partire la magia dell'ingresso graduale
                StartCoroutine(FadeInMusic());
            }
        }
        else
        {
            Debug.LogWarning("MUSIC ERROR: Nessuna traccia audio inserita!");
        }
    }

    // Coroutine che alza dolcemente il volume nel tempo
    IEnumerator FadeInMusic()
    {
        float currentTime = 0;

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            // Calcola la sfumatura morbida da 0 al volume massimo
            audioSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeInDuration);
            yield return null;
        }

        // Assicuriamoci che arrivi esattamente al volume target alla fine
        audioSource.volume = targetVolume;
    }
}