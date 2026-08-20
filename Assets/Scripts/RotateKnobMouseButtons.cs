using UnityEngine;
using UnityEngine.EventSystems;

public class RotateKnobMouseButtons : MonoBehaviour, IPointerClickHandler
{
    [Header("Impostazioni Rotazione")]
    [Tooltip("Gradi di rotazione per ogni singolo click")]
    public float rotationStep = 15f;

    // Questa funzione viene chiamata automaticamente da Unity quando clicchi sull'oggetto
    public void OnPointerClick(PointerEventData eventData)
    {
        // Controlla se è stato premuto il TASTO SINISTRO
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Ruota a sinistra (senso antiorario = asse Z positivo)
            transform.Rotate(0f, 0f, rotationStep);
        }
        // Controlla se è stato premuto il TASTO DESTRO
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Ruota a destra (senso orario = asse Z negativo)
            transform.Rotate(0f, 0f, -rotationStep);
        }
    }
}