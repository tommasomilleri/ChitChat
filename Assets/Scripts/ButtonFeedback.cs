using UnityEngine;
using UnityEngine.EventSystems; // Fondamentale per rilevare l'Hover e il Click sulla UI!

public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Audio Settings")]
    [Tooltip("L'AudioSource che suonerà i file. Può essere attaccato a questo bottone o al GameManager.")]
    public AudioSource audioSource;

    [Tooltip("Il suono di HOVER (quando la freccina entra nel bottone)")]
    public AudioClip hoverSound;

    [Tooltip("Il suono di CLICK (quando clicchi il bottone)")]
    public AudioClip clickSound;

    [Header("Randomizzazione (Anti-Noia)")]
    [Tooltip("Pitch minimo (es. 0.9 = leggermente più grave)")]
    public float minPitch = 0.9f;
    [Tooltip("Pitch massimo (es. 1.1 = leggermente più acuto)")]
    public float maxPitch = 1.1f;

    [Header("VFX Settings")]
    [Tooltip("Il prefabbricato (es. polvere o stelline) da far spawnare al click")]
    public GameObject clickVFXPrefab;

    // 1. Questa funzione scatta in automatico appena il mouse PASSA SOPRA il bottone (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySoundWithRandomPitch(hoverSound);
    }

    // 2. Questa funzione scatta in automatico quando CLICCHI il bottone
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySoundWithRandomPitch(clickSound);
        SpawnVFX();
    }

    // Funzione interna che randomizza il pitch e suona la clip
    private void PlaySoundWithRandomPitch(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Cambia il pitch scegliendo un valore a caso tra min e max
            audioSource.pitch = Random.Range(minPitch, maxPitch);

            // Riproduce il suono sovrapponendolo ad altri eventuali suoni
            audioSource.PlayOneShot(clip);
        }
        else if (clip != null && audioSource == null)
        {
            Debug.LogWarning("Manca l'AudioSource sul bottone: " + gameObject.name);
        }
    }

    // Funzione interna per generare l'effetto visivo
    private void SpawnVFX()
    {
        if (clickVFXPrefab != null)
        {
            GameObject spawnedVFX = Instantiate(clickVFXPrefab, transform.position, Quaternion.identity);
            spawnedVFX.transform.SetParent(transform.parent, true);
            Destroy(spawnedVFX, 2f);
        }
    }
}