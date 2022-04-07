using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Written by Benedict
/// <summary>
/// This script will change between the two camera states
/// The game is built to only have two players
/// We decided that more players might be bad game design
/// </summary>
public class CameraStateManager : MonoBehaviourPunCallbacks
{
    private float currentTime;
    [SerializeField] private float switchTime;
    
    public enum CAMERASTATE
    {
        player1sTurn,
        player2sTurn
    }

    private static CAMERASTATE cameraState;

    private void Start()
    {
        currentTime = Time.time;
        //Start off by being player1's turn
        cameraState = CAMERASTATE.player1sTurn;
    }

    private void Update()
    {
        if(GameStateManager.GetState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            //Switch the player's turn when time is up
            if (Time.time - currentTime > switchTime)
            {
                //Switch to player2
                if (cameraState == CAMERASTATE.player1sTurn)
                {
                    cameraState = CAMERASTATE.player2sTurn;
                }
                //Switch to player 1
                else if (cameraState == CAMERASTATE.player2sTurn)
                {
                    cameraState = CAMERASTATE.player1sTurn;
                }
                //Reset the time
                currentTime = Time.time;
            }
        }
    }

    public static CAMERASTATE GetCameraState()
    {
        return cameraState;
    }

}
