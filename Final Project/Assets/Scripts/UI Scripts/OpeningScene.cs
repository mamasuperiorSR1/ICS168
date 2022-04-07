using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : MonoBehaviour
{

    //Coded by Ed Slee

    //this script will be put on an empty object in the openning scene

    // Update is called once per frame
    void Update()
    {
        //If any key is pressed, send the player to the main menu
        if (Input.anyKey)
        {
            GameStateManager.NoneState();
            GameStateManager.MainMenu();
        }
    }
}
