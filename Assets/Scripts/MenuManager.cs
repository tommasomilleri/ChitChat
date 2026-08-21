using UnityEngine;

public class MenuManager : MonoBehaviour
{
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
        // We will add the PDF code here
        Debug.Log("Open Player 1 PDF");
    }

    // EXIT BUTTON
    public void ExitGame()
    {
        Debug.Log("Exit game");

        Application.Quit();
    }
}