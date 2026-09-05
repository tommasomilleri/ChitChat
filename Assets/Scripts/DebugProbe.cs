using UnityEngine;
public class DebugProbe : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"[PROBE] timeScale={Time.timeScale} | " +
                      $"GameManager={(GameManager.instance != null ? "OK" : "NULL")} | " +
                      $"qualityBar={(GameManager.instance != null && GameManager.instance.globalQualityBar != null ? "OK" : "NULL")} | " +
                      $"GameFeel={(GameFeel.Instance != null ? "OK" : "NULL")} | " +
                      $"EventSystem={(UnityEngine.EventSystems.EventSystem.current != null ? "OK" : "NULL")}");
        }
    }
}