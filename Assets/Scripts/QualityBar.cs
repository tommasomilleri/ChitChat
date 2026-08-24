using UnityEngine;
using UnityEngine.UI; // Necessario per usare il componente Image

public class QualityBar : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Trascina qui l'oggetto 'Fill' dalla Hierarchy")]
    public Image moldFillImage;

    private int maxQuality = 100;

    public void SetMaxQuality(int quality)
    {
        maxQuality = quality;
        UpdateMoldVisual(quality);
    }

    public void SetQuality(int quality)
    {
        UpdateMoldVisual(quality);
    }

    private void UpdateMoldVisual(int currentQuality)
    {
        if (moldFillImage != null && maxQuality > 0)
        {
            // 1. Calcola la percentuale di formaggio "sano" (es. 80/100 = 0.8)
            float healthPercent = (float)currentQuality / maxQuality;

            // 2. Inverte la logica: la percentuale di muffa è l'opposto della salute! (es. 1.0 - 0.8 = 0.2)
            float moldPercent = 1f - healthPercent;

            // 3. Applica il riempimento all'immagine
            moldFillImage.fillAmount = moldPercent;
        }
    }
}