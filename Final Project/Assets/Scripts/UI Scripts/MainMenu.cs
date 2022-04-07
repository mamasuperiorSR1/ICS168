using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //Coded by Ed Slee

    //this is the script that handles the buttons in the main menu

    //[SerializeField]
    //private string FirstScene;

    [SerializeField]
    private string TrainingModeScene;

    //Start a New Local Game by sending player to the controller selector and changing the multiplay state to Local
    public void OnClickLocalGame()
    {
        GameStateManager.LocalPlay();
        GameStateManager.GameMode();
    }

    //Enter the Lobby
    public void OnClickOnlineGame()
    {
        GameStateManager.OnlinePlay();
        GameStateManager.Lobby();
    }

    //Start the Training Mode Level
    public void OnClickTrainingMode()
    {
        GameStateManager.Start(TrainingModeScene);
    }

    //Read the Instructions
    public void OnClickInstructions()
    {
        Debug.Log("THIS WORKS");
        GameStateManager.Instructions();
    }

    //Quit the Game
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
