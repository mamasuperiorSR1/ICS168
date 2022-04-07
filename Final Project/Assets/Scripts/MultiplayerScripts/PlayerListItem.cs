using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//Created by Benedict 
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    private Text text;
    Player player;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    //Sets up the playerlist prefab 
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    //Destroy the game object if they leave the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    //If you leave the room, destroy the object
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
        Debug.Log("DESTROYED");
    }

}
