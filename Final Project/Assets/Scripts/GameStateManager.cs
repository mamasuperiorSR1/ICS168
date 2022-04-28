using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameStateManager : MonoBehaviourPunCallbacks
{
    //Coded by Ed Slee

    //Will be using this GameStateManager as a SceneManager that loads and changes scenes with the correct GameState

    private static GameStateManager Instance;
    private static readonly object padlock = new object();

    public enum MULTIPLAY
    {
        LOCAL,
        NONE,
        ONLINE
    }

    public enum GAMESTATE
    {
        PLAYING,
        GAMEOVER,
        MENU,
        PAUSE,
        SWAP,
        CINEMATIC
    }

    private static MULTIPLAY MultiplayState;

    private static GAMESTATE State;

    [SerializeField]
    private string MainMenuSetter;      //used only as a way to set the static variable, MainMenu

    private static string MainMenuName;

    [SerializeField]
    private string LobbySetter;      //used only as a way to set the static variable, Lobby

    private static string LobbyName;

    [SerializeField]
    private string InstructionsSetter;      //used only as a way to set the static variable, Instructions

    private static string InstructionsName;

    [SerializeField]
    private string GameModeSetter;  //used only as a way to set the static variable, Instructions

    private static string GameModeName;

    [SerializeField]
    private string ControllerSelectionSetter;   //used only as a way to set the static variable, ControllerSelectionName

    private static string ControllerSelectionName;

    public AudioSource audioSource;

    [SerializeField]
    private AudioClip MenuMusic;

    [SerializeField]
    private AudioClip LevelMusic;

    private bool MenuMusicPlaying;  //used for audio system
    private bool IsGameover;        //used for audio system

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }

        MainMenuName = MainMenuSetter;
        LobbyName = LobbySetter;
        InstructionsName = InstructionsSetter;
        GameModeName = GameModeSetter;
        ControllerSelectionName = ControllerSelectionSetter;
        audioSource = GetComponent<AudioSource>();
        MenuMusicPlaying = false;    //this is to account for the openning scene having no state
    }

    public static void ControllerSelection()
    {
        //could be removed if there is no way to get to controller selection
        //without going through the main menu
        State = GAMESTATE.MENU;

        SceneManager.LoadScene(ControllerSelectionName);
    }

    public static void GameMode()
    {
        //could be removed if there is no way to get to controller selection
        //without going through the main menu
        State = GAMESTATE.MENU;

        SceneManager.LoadScene(GameModeName);
    }

    //sets State to GAMEOVER
    public static void Gameover()
    {
        State = GAMESTATE.GAMEOVER;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }

    //returns the current multiplaystate
    public static MULTIPLAY GetMultiplayState()
    {
        return MultiplayState;
    }

    //returns the current gamestate
    public static GAMESTATE GetState()
    {
        return State;
    }

    //load the Instructions
    public static void Instructions()
    {
        //could be removed if there is no way to get to instructions
        //without going through the main menu
        State = GAMESTATE.MENU;
        SceneManager.LoadScene(InstructionsName);
    }

    //load the lobby for online play
    public static void Lobby()
    {
        //could be removed if there is no way to get to lobby
        //without going through the main menu
        State = GAMESTATE.MENU;
        Debug.Log(GetMultiplayState());
        SceneManager.LoadScene(LobbyName);
    }

    //changes Multiplay state to LOCAL
    public static void LocalPlay()
    {
        MultiplayState = MULTIPLAY.LOCAL;
    }

    //set State to MENU, Multiplay to NONE and Load Main Menu
    public static void MainMenu()
    {
        State = GAMESTATE.MENU;
        MultiplayState = MULTIPLAY.NONE;
        SceneManager.LoadScene(MainMenuName);
    }

    //changes Multiplay state to NONE
    public static void NoneState()
    {
        MultiplayState = MULTIPLAY.NONE;
    }

    //changes Multiplay state to ONLINE
    public static void OnlinePlay()
    {
        MultiplayState = MULTIPLAY.ONLINE;
        
    }

    //sets State to PAUSE
    //if we choose to use this, only in local games
    public static void Pause()
    {
        State = GAMESTATE.PAUSE;
        //Only pause the whole game if it is local multiplayer
        if (MultiplayState == MULTIPLAY.LOCAL)
        {
            Time.timeScale = 0f;
        }
    }

    //starts the current scene again
    public static void Restart()
    {
        Start(SceneManager.GetActiveScene().name);
    }

    //set State to PLAYING
    //would be used to resume play after a pause
    public static void Resume()
    {
        State = GAMESTATE.PLAYING;
        Time.timeScale = 1f;
    }

    public static void Cinematic()
    {
        State = GAMESTATE.CINEMATIC;
    }

    public static void SetState(GAMESTATE NewState)
    {
        //providing this conditional, because I only want this in use for the photon stream writing
        //other functions should call their respective functions
        if(MultiplayState == MULTIPLAY.ONLINE)
        {
            State = NewState;
        }
    }

    //set State to PLAYING and load a/the level
    //will also be used to restart after a gameover
    //if there is only one level, we could make the string name a SerializeField instead of a parameter
    public static void Start(string Level)
    {
        
        State = GAMESTATE.PLAYING;

        if(MultiplayState == MULTIPLAY.ONLINE)
        {
            //PhotonNetwork.LoadLevel(Level);
        }
        //must be accessible if NONE or if LOCAL
        else
        {
            MultiplayState = MULTIPLAY.LOCAL;
            SceneManager.LoadScene(Level);
        }

        //this can be removed if Gameover() will not set timescale to 0
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }  

    //set State to SWAP
    //will be used as a way a preventing any kind of pausing, playing or quitting during a player swap
    public static void Swap()
    {
        State = GAMESTATE.SWAP;
    }

    //handles the audio stuff appropriate for swapping states
    private void Update()
    {
        if (!MenuMusicPlaying && State == GAMESTATE.MENU)
        {
            audioSource.Pause();
            audioSource.clip = MenuMusic;
            audioSource.Play();
            MenuMusicPlaying = true;
        }
        else if (MenuMusicPlaying && !IsGameover && State != GAMESTATE.MENU)
        {
            audioSource.Pause();
            audioSource.clip = LevelMusic;
            audioSource.Play();
            MenuMusicPlaying = false;
        }

        //Debug.LogError(State);
        //Debug.LogError(MultiplayState);
    }
}
