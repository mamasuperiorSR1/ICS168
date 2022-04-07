using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Written by Benedict
/// <summary>
/// This allows the cameras to switch back and forth in multiplayer
/// We simply only need to check tags and check the camera state
/// This game constraint is two players max. We do not want more than two.
/// Completely different compared to the local system
/// </summary>

public class CameraSwitchMechanic : MonoBehaviour
{
    [SerializeField] private float switchTime;
    [SerializeField] private GameObject camera;
    [SerializeField] private string MasterTag;
    [SerializeField] private string PlayerTag;
    [SerializeField] private GameObject guns;
    private float currentTime;
    private bool firstSwap;

    private void Start()
    {
        currentTime = Time.time;

        //Basically no one has their weapons till the first swap happens
        firstSwap = true;
        if(gameObject.tag == MasterTag)
        {
            camera.SetActive(true);
            Debug.Log("Activated the Master Camera");
        }
        else
        {
            camera.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            //If it's player 1's turn and they are the master, turn on the camera
            if(CameraStateManager.GetCameraState() == CameraStateManager.CAMERASTATE.player1sTurn && gameObject.tag == MasterTag)
            {
                camera.SetActive(true);
            }
            //If it's player 2's turn and they are not the master, turn on the camera
            else if(CameraStateManager.GetCameraState() == CameraStateManager.CAMERASTATE.player2sTurn && gameObject.tag == PlayerTag)
            {
                camera.SetActive(true);
            }
            //Just turn it off if it isn't their time to have their camera on
            else
            {
                camera.SetActive(false);
            }
            //Activate the guns if it is the first swap and now player 2's turn
            if (firstSwap && CameraStateManager.GetCameraState() == CameraStateManager.CAMERASTATE.player2sTurn)
            {
                firstSwap = false;
                guns.SetActive(true);
            }
        }
    }
}
