using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Coded by Ed Slee

public class GoBack : MonoBehaviour
{
    //Go back to Main Menu
    public void OnClickBack()
    {
        GameStateManager.NoneState();
        GameStateManager.MainMenu();
    }
}
