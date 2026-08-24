using System.Collections;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The main empty GameObject containing the dark background and the menu")]
    public GameObject pauseMenuContainer;

    [Tooltip("The RectTransform of the parchment image that will slide")]
    public RectTransform menuPanel;

    [Header("Animation Settings")]
    public float slideDuration = 0.4f;
    [Tooltip("Y position when hidden (above screen)")]
    public float hiddenYPos = 1200f;
    [Tooltip("Y position when visible (center screen)")]
    public float visibleYPos = 0f;

    private bool isPaused = false;
    private Coroutine slideCoroutine;

    void Start()
    {
        // Ensure the menu is hidden and the game is running normally at startup
        if (pauseMenuContainer != null)
        {
            pauseMenuContainer.SetActive(false);
        }

        // Snap the menu to the hidden position immediately
        if (menuPanel != null)
        {
            menuPanel.anchoredPosition = new Vector2(menuPanel.anchoredPosition.x, hiddenYPos);
        }
    }

    void Update()
    {
        // Toggle pause with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuContainer.SetActive(true);
        Time.timeScale = 0f; // Freezes the game logic

        // Start the slide-in animation
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideMenu(visibleYPos, false));
    }

    // THIS IS CONNECTED TO THE "PLAY" CHEESE BUTTON
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unfreezes the game immediately

        // Start the slide-out animation
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideMenu(hiddenYPos, true));
    }

    // THIS IS CONNECTED TO THE EXIT BUTTON (Mouse hole)
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Always reset time scale before loading a new scene!
        Debug.Log("Returning to Main Menu...");

        // Uncomment and use this when you have a main menu scene:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // The animation logic
    private IEnumerator SlideMenu(float targetY, bool isSlidingOut)
    {
        float elapsedTime = 0f;
        Vector2 startPos = menuPanel.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, targetY);

        while (elapsedTime < slideDuration)
        {
            // SmoothStep formula for a polished UX curve (starts slow, speeds up, slows down)
            float t = elapsedTime / slideDuration;
            float smoothStep = t * t * (3f - 2f * t);

            // Use unscaledDeltaTime so the UI moves even when Time.timeScale == 0
            menuPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothStep);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Snap precisely to the target at the end
        menuPanel.anchoredPosition = endPos;

        // If sliding out, completely hide the container once the animation finishes
        if (isSlidingOut && pauseMenuContainer != null)
        {
            pauseMenuContainer.SetActive(false);
        }
    }
}