using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Written by Benedict
public class PlayerMovement : MonoBehaviour
{
    // Player objects
    [SerializeField] private Transform m_PlayerBody;
    [SerializeField] private CharacterController m_PlayerController;

    // Characters' default settings
    [SerializeField] public int m_PlayerNumber;
    [SerializeField] private float m_PlayerSpeed = 2f;
    [SerializeField] private float m_PlayerGravity = -9.81f;

    // GroundCheckSettings
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private float m_GroundDistance;
    [SerializeField] private LayerMask m_GroundMask;

    //MultiplayerSettings
    [SerializeField] private PhotonView view;

    private Vector3 m_PlayerVelocity;
    private bool m_IsGrounded;          // check whether the character is on the ground now.

    public float PlayerSpeed { get => m_PlayerSpeed; set => m_PlayerSpeed = value; }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        // get the player's number assigned by game manager (done by Joshua Wolfe)
        try
        {
            m_PlayerNumber = int.Parse(this.gameObject.tag);
            if (this.gameObject.name.Contains("(joystick)"))
            {
                m_PlayerNumber = 3;
            }
        }
        catch
        {
            if (this.gameObject.tag == "Player")
            {
                m_PlayerNumber = 1;
            }
            else
            {
                m_PlayerNumber = 0;
            }
        }
    }

    // Update is called once per frame, it updates the position information.
    
    void Update()
    {
        Move();
        // Reset the velocity when the character is already on the floor.
        if (m_IsGrounded && m_PlayerVelocity.y < 0)
        {
            m_PlayerVelocity.y = -2f;
        }

        // Physics for falling when character is not on the floor.
        m_PlayerVelocity.y += m_PlayerGravity * Time.deltaTime;
        m_PlayerController.Move(m_PlayerVelocity * Time.deltaTime);

    }

    private void Move()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING && m_PlayerNumber != 0)
            {
                // Written by Weilin Zhou(Rocka)
                // Get the latest input (WASD and arrow key).
                float x = Input.GetAxis("Horizontal" + m_PlayerNumber);
                float z = Input.GetAxis("Vertical" + m_PlayerNumber);
                // Updates character's position based on the inputs.
                Vector3 move = m_PlayerBody.right * x + m_PlayerBody.forward * z;
                m_PlayerController.Move(move * PlayerSpeed * Time.deltaTime);
            }
        }
        //You do not want the players to control both cameras
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            if (view.IsMine)
            {
                
                // Get the latest input (WASD and arrow key).
                float x = Input.GetAxis("Horizontal1");
                float z = Input.GetAxis("Vertical1");
                // Updates character's position based on the inputs.
                Vector3 move = m_PlayerBody.right * x + m_PlayerBody.forward * z;
                m_PlayerController.Move(move * PlayerSpeed * Time.deltaTime);
            }
        }
    }
}