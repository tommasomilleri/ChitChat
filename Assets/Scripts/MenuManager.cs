using System;
using System.IO;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject StartPage;
    public GameObject StoryPage;
    public GameObject TutorialPage;
    public GameObject PlayerSelectPage;

    [Header("Levels")]
    public GameObject Lvl1;

    void Start()
    {
        // 1. Nasconde il Quality Meter appena si apre il menu usando il NUOVO GameManager
        if (GameManager.instance != null && GameManager.instance.qualityBarContainer != null)
        {
            GameManager.instance.qualityBarContainer.SetActive(false);
        }

        // 2. FORZATURA DI SICUREZZA: Assicura che al riavvio della scena ci sia solo lo StartMenu
        if (StartPage != null) StartPage.SetActive(true);
        if (StoryPage != null) StoryPage.SetActive(false);
        if (TutorialPage != null) TutorialPage.SetActive(false);
        if (PlayerSelectPage != null) PlayerSelectPage.SetActive(false);
        if (Lvl1 != null) Lvl1.SetActive(false);
    }

    // START BUTTON
    public void StartGame()
    {
        if (StartPage != null) StartPage.SetActive(false);
        if (StoryPage != null) StoryPage.SetActive(true);
    }

    // NEXT BUTTON ON STORY PAGE
    public void GoToPlayerSelection()
    {
        if (StoryPage != null) StoryPage.SetActive(false);
        if (PlayerSelectPage != null) PlayerSelectPage.SetActive(true);
    }

    // TUTORIAL BUTTON
    public void OpenTutorial()
    {
        if (StartPage != null) StartPage.SetActive(false);
        if (TutorialPage != null) TutorialPage.SetActive(true);
    }

    // BACK BUTTON ON TUTORIAL
    public void BackToStart()
    {
        if (TutorialPage != null) TutorialPage.SetActive(false);
        if (StartPage != null) StartPage.SetActive(true);
    }

    // PLAYER 1 (The Reader)
    public void SelectPlayer1()
    {
        Debug.Log("Player 1 (Reader) selected. Opening PDF...");
        OpenPlayer1PDF();
        // IMPORTANT: We deliberately DO NOT hide the PlayerSelectPage here!
    }

    // PLAYER 2 (The Chef)
    public void SelectPlayer2()
    {
        Debug.Log("Player 2 (Chef) selected. Starting Level 1...");

        if (PlayerSelectPage != null)
        {
            PlayerSelectPage.SetActive(false);
        }

        if (Lvl1 != null)
        {
            Lvl1.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Lvl1 is missing! Drag it into the MenuManager Inspector.");
        }

        // Riaccende la barra della qualità usando il NUOVO GameManager
        if (GameManager.instance != null && GameManager.instance.qualityBarContainer != null)
        {
            GameManager.instance.qualityBarContainer.SetActive(true);
        }
    }

    void OpenPlayer1PDF()
    {
        try
        {
            string pdfPath = Path.Combine(Application.streamingAssetsPath, "ChitRecipe.pdf");
            string pdfURL = new Uri(pdfPath).AbsoluteUri;
            Application.OpenURL(pdfURL);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to open the PDF recipe! Error: " + e.Message);
        }
    }

    // EXIT BUTTON
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}