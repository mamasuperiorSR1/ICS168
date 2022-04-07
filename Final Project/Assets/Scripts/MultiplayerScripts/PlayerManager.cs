using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

//Written by Ed
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private string PrefabName;
    [SerializeField] private string MasterTag;
    [SerializeField] private string PlayerTag;

    private PhotonView view;

    private bool TagsSetUp;

    private int NumberOfPlayersAllowed = 2;     //the whole system is built for only 2 players, but didn't want to have magic numbers

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        TagsSetUp = false;
    }

    private void Start()
    {
        if (view.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
        GameObject InstantiatedPlayer = PhotonNetwork.Instantiate(PrefabName, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawned");

        if (PhotonNetwork.IsMasterClient)
        {
            InstantiatedPlayer.tag = MasterTag;
        }
    }

    //this shares information between the two players on line
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //this section shares the GameState
            if(GameStateManager.GetState() != GameStateManager.GAMESTATE.MENU)
            {
                stream.SendNext(GameStateManager.GetState());
            }
            else
            {
                GameStateManager.SetState(GameStateManager.GAMESTATE.PLAYING);
            }

            //this section shares the PlayerCount
            if(PlayerCount.GetCount() == 1)
            {
                Debug.Log("Streaming GetCount()");
                stream.SendNext(PlayerCount.GetCount());
            }
        }
        else 
        {
            //this way you don't try to typecast the wrong thing
            object Received = stream.ReceiveNext();
            if(Received is GameStateManager.GAMESTATE)
            {
                //this section receives the GameState
                //this is important for GAMEOVER, SWAP, and PLAYING States, but NOT PAUSE or MENU
                if (GameStateManager.GetState() != GameStateManager.GAMESTATE.PAUSE)
                {
                    GameStateManager.GAMESTATE ReceivedState = (GameStateManager.GAMESTATE)Received;
                    if (ReceivedState == GameStateManager.GAMESTATE.MENU)
                    {
                        GameStateManager.SetState(GameStateManager.GAMESTATE.PLAYING);
                    }
                    else if (ReceivedState != GameStateManager.GAMESTATE.PAUSE)
                    {
                        GameStateManager.SetState(ReceivedState);
                    }
                    else
                    {
                        //Do nothing with the ReceivedState if Paused
                    }
                }
            }
            else if(Received is int)
            {
                //received for the party that only has 2
                if((int)Received == 1 && PlayerCount.GetCount() == 2)
                {
                    PlayerCount.DecreaseCount();
                }
            }
        }
    }

    private void Update()
    {
        //This is so that on the perspective of the nonMasterClient, the MasterClient has their MasterTag
        if (!PhotonNetwork.IsMasterClient && !TagsSetUp)
        {
            GameObject[] Players = GameObject.FindGameObjectsWithTag(PlayerTag);
            foreach(GameObject Playerr in Players)
            {
                if (!Playerr.GetComponent<PhotonView>().IsMine)
                {
                    Playerr.tag = MasterTag;
                    TagsSetUp = true;
                    break;
                }
            }
        }
    }
}
