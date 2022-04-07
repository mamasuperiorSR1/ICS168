using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created by Benedict Hsueh
public class SwitchInstructions : MonoBehaviour
{
    [SerializeField] private GameObject multiplayerButton;
    [SerializeField] private GameObject localButton;
    [SerializeField] private GameObject multiplayerInstructions;
    [SerializeField] private GameObject localInstructions;

    //Show the multiplayer instructions and change the buttons and turn off the local stuff
    public void OnClickMultiplayer()
    {
        multiplayerButton.SetActive(false);
        multiplayerInstructions.SetActive(true);

        localButton.SetActive(true);
        localInstructions.SetActive(false);
    }

    //Show the local instructions and change the buttons and turn off the multiplayer stuff
    public void OnClickLocal()
    {
        multiplayerButton.SetActive(true);
        multiplayerInstructions.SetActive(false);

        localButton.SetActive(false);
        localInstructions.SetActive(true);
    }
}
