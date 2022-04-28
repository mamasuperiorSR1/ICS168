using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PausingMenu : MonoBehaviourPunCallbacks
{
    //Coded by Ed Slee

    //this is the class that takes care of the menu and pauses the game

    private int MenuChildrenCount;

    void Awake()
    {
        //initially sets every child in the UI menu to inactive

        MenuChildrenCount = transform.childCount;
        SetMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.GetState() != GameStateManager.GAMESTATE.GAMEOVER && GameStateManager.GetState() != GameStateManager.GAMESTATE.CINEMATIC)
        {
            //if the game is playing, pause
            if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
            {
                SetMenu(true);
                GameStateManager.Pause();
            }

            //if the game is paused, resume
            else if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PAUSE)
            {
                GameStateManager.Resume();
                SetMenu(false);
            }
        }

        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PAUSE)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnClickResume();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                OnClickExit();
            }
        }
    }

    //Sets the menu to be inactive
    private void SetMenu(bool ActiveState)
    {
        for (int i = MenuChildrenCount - 1; i >= 0; i--)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.SetActive(ActiveState);
        }
    }

    //These buttons may not function properly in online

    //Go back to Main Menu
    public void OnClickExit()
    {
        PlayerPrefs.DeleteAll();

        //this is only if the player is online
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            PhotonNetwork.Disconnect();
        }
        GameStateManager.NoneState();
        GameStateManager.MainMenu();
    }

    //Restart the scene
    public void OnClickResume()
    {
        GameStateManager.Resume();
        SetMenu(false);
    }
}
