using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    //Coded by Ed Slee


    [SerializeField]
    private string LevelToLoad;

    private void Start()
    {
        //this is done just in case somebody has exited the game by closing it without going through the Gameover Screen
        PlayerPrefs.DeleteAll();
    }

    public void OnClickPVP()
    {
        //Sends players to ControllerSelection screen
        GameStateManager.ControllerSelection();
    }

    public void OnClickPVAI()
    {
        //Starts the AI game
        GameStateManager.Start(LevelToLoad);
    }
}
