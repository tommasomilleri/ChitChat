using UnityEngine;
using UnityEngine.UI;

public class QualityBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxQuality(int maxQuality)
    {
        slider.maxValue = maxQuality;
        slider.value = maxQuality;
    }

    public void SetQuality(int quality)
    {
        slider.value = quality;
    }
}