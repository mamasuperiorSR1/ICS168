using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

//Written by Benedict; watched a tutorial
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private string playerManagerName;
    private GameObject playerManager;

    //Getters
    public GameObject PlayerManager { get => playerManager;}

    private void Awake()
    {
        //Singleton code
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    //Subscribe to a scene
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Check that scene that was subscribed and see if it is the right scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Check if we're in the game scene
        if(scene.name == gameSceneName)
        {
            playerManager = PhotonNetwork.Instantiate(playerManagerName, Vector3.zero, Quaternion.identity);
        }
    }
}
