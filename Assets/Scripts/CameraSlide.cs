using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Aggiunto nel caso ti serva per il cambio scena

public class CameraSlide : MonoBehaviour
{
    public float slideDuration = 2.0f; // Durata dello spostamento in secondi

    public void SlideTo(Transform target) // CORRETTO: Trasform -> Transform
    {
        StartCoroutine(MoveCamera(target)); // CORRETTO: aggiunto ;
    }

    IEnumerator MoveCamera(Transform target)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.position;

        // Salviamo anche la rotazione per un effetto più bello
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = target.rotation;

        float time = 0;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float progress = time / slideDuration;

            // MATHF.SMOOTHSTEP: Ammorbidisce il movimento (accelera e decelera dolcemente)
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // Aggiorniamo posizione e rotazione
            transform.position = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, smoothProgress);

            yield return null;
        }

        // Assicuriamoci che la camera finisca ESATTAMENTE sul target
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // ---> CAMBIO SCENA <---
        // Se alla fine del movimento vuoi caricare una nuova scena, 
        // togli il commento alla riga qui sotto e inserisci il nome della tua scena:

        // SceneManager.LoadScene("NomeDellaTuaScena");
    }
}