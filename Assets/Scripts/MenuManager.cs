using System;
using System.IO;


using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject NextLevel;
    public GameObject CurrentLevel;
    public GameObject canvas;
    public GameObject StartPage;
    public GameObject StoryPage;
    public GameObject TutorialPage;
    public GameObject PlayerSelectPage;

    public GameObject Lvl1;

    // START BUTTON
    public void StartGame()
    {
        StartPage.SetActive(false);
        StoryPage.SetActive(true);
    }

    // NEXT BUTTON ON STORY PAGE
    public void GoToPlayerSelection()
    {
        StoryPage.SetActive(false);
        PlayerSelectPage.SetActive(true);
    }

    // TUTORIAL BUTTON
    public void OpenTutorial()
    {
        StartPage.SetActive(false);
        TutorialPage.SetActive(true);
    }

    // BACK BUTTON ON TUTORIAL
    public void BackToStart()
    {
        TutorialPage.SetActive(false);
        StartPage.SetActive(true);
    }

    // PLAYER 1
    public void SelectPlayer1()
    {
        Debug.Log("Player 1 selected");

        OpenPlayer1PDF();
    }

    // PLAYER 2
    public void SelectPlayer2()
    {
        Debug.Log("Player 2 selected");

        PlayerSelectPage.SetActive(false);

        Lvl1.SetActive(true);
    }

  void OpenPlayer1PDF()
    {
        string pdfPath =
            Path.Combine(Application.streamingAssetsPath, "ChitRecipe.pdf");

        string pdfURL = new Uri(pdfPath).AbsoluteUri;

        Application.OpenURL(pdfURL);
    }

    // EXIT BUTTON
    public void ExitGame()
    {
        Debug.Log("Exit game");

        Application.Quit();
    }
    void GoToNextLevel()
    {
        if (NextLevel != null && canvas != null)
        {
            var newLevel = Instantiate(NextLevel);

            // Passing 'false' prevents the UI from scaling weirdly when parented
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
    }
}