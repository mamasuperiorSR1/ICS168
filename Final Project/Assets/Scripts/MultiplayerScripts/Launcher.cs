using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

//Created by Benedict; watched a tutorial
namespace MultiplayerTest
{   public class Launcher : MonoBehaviourPunCallbacks
    {
        //Max number of players in a lobby
        [SerializeField] private byte maxPlayersPerRoom = 2;

        public static Launcher launcherInstance;

        //Levels
        [SerializeField] private string multiplayerLevel;

        //UI Stuff
        [SerializeField] private InputField createInputField;
        [SerializeField] private InputField joinInputField;
        [SerializeField] private Text roomNameText;
        [SerializeField] private Text errorText;
        [SerializeField] private Transform playerListContent;
        [SerializeField] private GameObject playerListPrefab;
        [SerializeField] private GameObject startGameButton;
        

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(multiplayerLevel);
            GameStateManager.SetState(GameStateManager.GAMESTATE.PLAYING);
        }

        private void Awake()
        {
            launcherInstance = this;
        }

        //Close the application
        public void QuitGame()
        {
            PhotonNetwork.Disconnect();
            GameStateManager.NoneState();
            GameStateManager.MainMenu();
        }

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1f;
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork
        }

        //When you connect to the master you want to join the lobby
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinLobby();

            //All clients should switch scenes with the host
            PhotonNetwork.AutomaticallySyncScene = true;

        }

        //You join the lobby and you are assigned a nickname for now <- prob permanent unless we add a username system in the future
        public override void OnJoinedLobby()
        {
            MenuManager.menuManagerInstance.OpenMenu("title");
            Debug.Log("Joined the Lobby");
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
           
        }

        //Create a room 
        public void CreateRoom()
        {
            //Make sure there is text
            if(string.IsNullOrEmpty(createInputField.text))
            {
                return;
            }
            //Creates a room with a max number of players
            PhotonNetwork.CreateRoom(createInputField.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            MenuManager.menuManagerInstance.OpenMenu("loading");
        }

        //Join a room
        public void JoinRoom()
        {
            //Make sure there is text
            if (string.IsNullOrEmpty(joinInputField.text))
            {
                return;
            }
            PhotonNetwork.JoinRoom(joinInputField.text);
        }

        //Open the room menu
        public override void OnJoinedRoom()
        {
            MenuManager.menuManagerInstance.OpenMenu("room");
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;

            Player[] players = PhotonNetwork.PlayerList;

            //Clear the list
            foreach(Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Length; i++)
            {
                Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }

            //Only seen for the host
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        //Built-in host switch
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        //Open the error menu
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorText.text = "Room Creation Failed:" + message;
            MenuManager.menuManagerInstance.OpenMenu("error");
        }

        //Leave a room 
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.menuManagerInstance.OpenMenu("loading");
        }

        //Open the menu after leaving
        public override void OnLeftRoom()
        {
            MenuManager.menuManagerInstance.OpenMenu("title");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        }
    }
}

