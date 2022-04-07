using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSelection : MonoBehaviour
{
    //Ed Slee
    //this is code for the ControllerSelection Menu 

    private int JoystickChoice = 1;

    [SerializeField]
    private string LevelToLoad;

    private void Start()
    {
        //this is done just in case somebody has exited the game by closing it without going through the Gameover Screen
        //PlayerPrefs.DeleteAll();
    }

    public void OnClickKeyboard()
    {
        //Does not create the player pref
        GameStateManager.Start(LevelToLoad);
    }

    public void OnClickJoystick()
    {
        //Does create the player pref and sets it to a value that is not 0
        PlayerPrefs.SetInt("ControllerPref", JoystickChoice);
        GameStateManager.Start(LevelToLoad);

    }
}
