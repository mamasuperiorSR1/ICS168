using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//Coded by Ed Slee

//this is the code to use for GameoverMenu and activates it at a Gameover and deactivates it at restart

public class GameoverMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text GameoverText;

    private int MenuChildrenCount;
    private bool WasGameover;        //true if gameover state just ends

    private int WinningPlayer;
    private int Player1Score;
    private int Player2Score;
    private int RoundsPlayed;

    private PhotonView view;

    void Awake()
    {
        //initially sets every child in the UI menu to inactive

        MenuChildrenCount = transform.childCount;
        SetMenu(false);

        //Saves the cumulative scores from each round and the round number
        Player1Score = PlayerPrefs.GetInt("P1Score");
        Player2Score = PlayerPrefs.GetInt("P2Score");
        RoundsPlayed = PlayerPrefs.GetInt("RoundPlayed");

        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.GAMEOVER && !WasGameover)
        {
            SetMenu(true);
            WinningPlayer = PlayerCount.Winner();
            
            //find the score and print all end game stats if local
            if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
            {       
                Score();
                GameoverText.text = "Player " + WinningPlayer + " Wins!\nThe score is " + Player1Score + ":" + Player2Score + "\nRounds Played: " + RoundsPlayed;
            }

            //just print the winning player, which should be 1 for Master and 2 for player
            else if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
            {
                //do on both screens
                view.RPC("RPC_WinText", RpcTarget.All, WinningPlayer);
            }
        }
        else if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING && WasGameover)
        {
            SetMenu(false);
        }

        //buttons can be activated with keycodes
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnClickRestart();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                OnClickExit();
            }
        }
    }

    [PunRPC]
    private void RPC_WinText(int winner)
    {
        GameoverText.text = "Gameover!\nPlayer " + winner +" Wins!";
    }

    //Sets the menu to be inactive
    private void SetMenu(bool ActiveState)
    {
        for (int i = MenuChildrenCount - 1; i >= 0; i--)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.SetActive(ActiveState);
        }

        //if you activate the ui, then the state was just turned to GAMEOVER, so WasGameover was false, and now is true
        //if you deactivate the ui, then the state was just turned to PLAYING, so WasGameover was true, and now is false
        WasGameover = ActiveState;
    }

    //These buttons may not function properly in online

    //Go back to Main Menu
    public void OnClickExit()
    {
        //delete all playerprefs so that it can be restarted when local play is selected again
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
    public void OnClickRestart()
    {
        GameStateManager.Restart();
    }

    //Adds one more point to the winning player
    private void Score()
    {
        RoundsPlayed++;
        if (WinningPlayer == 1)
        {
            Player1Score++;
        }
        else if (WinningPlayer == 2)
        {
            Player2Score++;
        }
        PlayerPrefs.SetInt("P1Score", Player1Score);
        PlayerPrefs.SetInt("P2Score", Player2Score);
        PlayerPrefs.SetInt("RoundPlayed", RoundsPlayed);
    }
}
