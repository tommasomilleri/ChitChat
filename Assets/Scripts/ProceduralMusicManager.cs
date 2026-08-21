using UnityEngine;

// This automatically adds an AudioSource to your GameObject if you forget to!
[RequireComponent(typeof(AudioSource))]
public class ProceduralMusicManager : MonoBehaviour
{
    // Singleton reference to keep this object alive across levels
    public static ProceduralMusicManager instance;

    [Header("Music Settings")]
    [Tooltip("Drag your .wav or .mp3 background music here")]
    public AudioClip backgroundTrack;

    // We make this private because the script will find it automatically now
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern: ensures the music doesn't stop when changing scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the music alive across all levels!
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates on scene reload
            return;
        }

        // Automatically grab the AudioSource from this GameObject
        audioSource = GetComponent<AudioSource>();

        // Play the track if it has been assigned in the Inspector
        if (backgroundTrack != null)
        {
            audioSource.clip = backgroundTrack;
            audioSource.loop = true;
            audioSource.playOnAwake = true;

            // Start playing only if it isn't playing already
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // If nothing is heard, this will tell you exactly why in the Console!
            Debug.LogWarning("MUSIC ERROR: No audio clip found! Please drag a WAV file into the 'Background Track' slot in the Inspector.");
        }
    }
}