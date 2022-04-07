using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Written by Benedict
public class PlayerCameraMovement : MonoBehaviour
{
    //Mouse settings
    [SerializeField] private float m_MouseSensitivity;
    [SerializeField] private Transform m_PlayerBody;
    [SerializeField] private Transform m_PlayerCamera;

    [SerializeField] private string axis;

    //Multiplayer settings
    [SerializeField] private PhotonView view;

    private float m_xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //Fix camera bug for multiplayer
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            if (!view.IsMine)
            {
                GetComponentInChildren<Camera>().gameObject.SetActive(false);
            }
        }
        //Lock the cursor 
        Cursor.lockState = CursorLockMode.Locked;

        //Input Axis selection done by Joshua Wolfe
        if (this.gameObject.name.Contains("(joystick)"))
        {
            axis = "Joystick (right)";
        }
        else
        {
            axis = "Mouse";
        }
    }

    private void Awake()
    {
        //When a player is spawned, they immediately start playing
        //GameStateManager.Resume();

        //Get the PhotonView component
        view = GetComponent<PhotonView>();

        //Lock the cursor 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        Look();

        //Unlock the cursor when in pause or gameover menu
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PAUSE ||
            GameStateManager.GetState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Look()
    {
        //Only run this code if it's local multiplayer
        if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            //Joshua added the Find function  
            if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING && this.transform.Find("Main Camera").gameObject.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Locked;

                //Mouse movement
                float X = Input.GetAxis(axis + " X") * m_MouseSensitivity;
                float Y = Input.GetAxis(axis + " Y") * m_MouseSensitivity;

                //Move the player head left and right
                m_PlayerBody.Rotate(Vector3.up * X);

                //Move the player head up and down
                m_xRotation -= Y;
                m_xRotation = Mathf.Clamp(m_xRotation, -60f, 60f);

                m_PlayerCamera.localRotation = Quaternion.Euler(m_xRotation, 0, 0);

                //Debug.Log("In if statement");
            }
        }
        //You do not want the players to control both cameras
            if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
            {
                if (view.IsMine)
                {
                    if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        
                        //Mouse movement
                        float X = Input.GetAxis(axis + " X") * m_MouseSensitivity;
                        float Y = Input.GetAxis(axis + " Y") * m_MouseSensitivity;

                        //Move the player head left and right
                        m_PlayerBody.Rotate(Vector3.up * X);

                        //Move the player head up and down
                        m_xRotation -= Y;
                        m_xRotation = Mathf.Clamp(m_xRotation, -60f, 60f);

                        m_PlayerCamera.localRotation = Quaternion.Euler(m_xRotation, 0, 0);

                        //Debug.Log("In if statement");
                    }
                }
            }
        
    }
}
