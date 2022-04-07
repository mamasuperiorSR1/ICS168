using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Written by Benedict
public class GameUIUpdater : MonoBehaviour
{
    [SerializeField]
    private float maxCameraTimer = 5.0f;
    private float cameraTimerCopy;

    [SerializeField]
    private RawImage P1;
    [SerializeField]
    private RawImage P2;
    private float TotalTime;

    private void Start()
    {
        cameraTimerCopy = maxCameraTimer;
    }

    private void Update()
    {
        UpdateP1P2UI();
        UpdateTimerUI();
    }

    //Update whether it is P1 or P2's turn
    public void UpdateP1P2UI()
    {
        //One will be brighter than the other one
        if(CameraStateManager.GetCameraState() == CameraStateManager.CAMERASTATE.player1sTurn)
        {
            P1.color = new Color32(255, 255, 255, 255);
            P2.color = new Color32(0, 0, 0, 100);
            //Debug.Log("Player 1's Turn");
        }
        else
        {
            P2.color = new Color32(255, 255, 255, 255);
            P1.color = new Color32(0, 0, 0, 100);
            //Debug.Log("Player 2's Turn");
        }
    }

    //Update the timer
    public void UpdateTimerUI()
    {
        if(GameStateManager.GetState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            //Once we hit 0 we need to reset the timer
            if (cameraTimerCopy <= 0.0f)
            {
                GameStateManager.Swap();
                cameraTimerCopy = maxCameraTimer;
            }
            TimerUI.SetTimerUI((int)Mathf.Ceil(cameraTimerCopy));
            cameraTimerCopy -= Time.deltaTime;
            GameStateManager.Resume();
        }
    }


}
