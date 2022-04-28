using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIvPlayerGameManager : MonoBehaviour
{
    //Worked on by Joshua Wolfe
    [HideInInspector]
    public List<GameObject> players;
    private int index;
    [SerializeField]
    private float maxCameraTimer = 5.0f;
    private float cameraTimerCopy;
    [SerializeField]
    private string Player1Tag;

    [SerializeField]
    private RawImage P1;
    [SerializeField]
    private RawImage P2;

    [SerializeField]
    private Mesh[] Meshes;

    private GameObject player;
    private GameObject dummy;

    [SerializeField] private Camera StartCamera;
    [SerializeField] private Transform m_PlayerCamera;
    [SerializeField] private float CamSpeed = 5f;
    [SerializeField] private float CameraMoveSpeed = 10f;
    private bool StartCameraEnabled;


    void Start()
    {
        cameraTimerCopy = maxCameraTimer;
        StartCameraEnabled = true;
        PlayerAssignment();
        RandomMeshes();
        GameStateManager.Cinematic();
        //StartCoroutine(MoveToPlayer());
    }

    /*
    IEnumerator MoveToPlayer()
    {
        StartCamera.transform.position = Vector3.MoveTowards(StartCamera.transform.position, m_PlayerCamera.position, CameraMoveSpeed * Time.deltaTime);
        yield return new WaitForSeconds(2);
        StartCamera.enabled = false;
        StartCameraEnabled = false;
        GameStateManager.Resume();
        //GameStateManager.GAMESTATE = GameStateManager.GAMESTATE.PLAYING;
    }
    */

    private void Move()
    {
        StartCamera.enabled = false;
        GameStateManager.Resume();
    }

    private void RandomMeshes()
    {
        //This is done by Ed Slee
        //This script is providing each player in the game a mesh
        foreach (GameObject player in players)
        {
            int RandomIndex = Random.Range(0, Meshes.Length);
            player.GetComponent<MeshFilter>().sharedMesh = Meshes[RandomIndex];
        }
    }

    public void PlayerAssignment()
    {
        GameStateManager.Resume();
        players = new List<GameObject>();

        player = GameObject.FindGameObjectWithTag("Player");
        dummy = GameObject.FindGameObjectWithTag("Dummy");
        players.Add(player);
        players.Add(dummy);

        index = Random.Range(0, players.Count);


        /*
          NOTE: I know the "Find" function is controversial, but there are some reasons why "Find" is acceptable here:
          1: FindTag only finds ACTIVIVE game objects; since this code works by activating and deacting the camera child object of the player,
             that function will not work here.
          2: "Main Camera" is a universial name and is very unlikely for it to be changed at some point.
        */

        //Disable every player's "Main Camera" and activate the one randomly chosen above
        foreach (GameObject playerr in players)
        {
            playerr.transform.Find("Main Camera").gameObject.SetActive(false);
        }
        players[index].transform.Find("Main Camera").gameObject.SetActive(true);

        if (dummy.transform.Find("Main Camera").gameObject.activeSelf)
        {
            dummy.GetComponent<ShootDummy>().enabled = true;
            dummy.GetComponent<Dummy>().enabled = false;
        }
        else
        {
            dummy.GetComponent<ShootDummy>().enabled = false;
            dummy.GetComponent<Dummy>().enabled = true;
        }

        PlayerUISwapper(players[index].tag);

        cameraTimerCopy = maxCameraTimer;
    }

    void Update()
    {
        Debug.Log(GameStateManager.GetState());
        /*
        if (StartCameraEnabled == true)
        {
            GameStateManager.Cinematic();
            //StartCameraEnabled = false;
            StartCoroutine(MoveToPlayer());
        }
        */
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            StartCamera.transform.position = Vector3.MoveTowards(StartCamera.transform.position, m_PlayerCamera.position, CameraMoveSpeed * Time.deltaTime);
            if (StartCameraEnabled)
            {
                Invoke("Move", 2f);
                StartCameraEnabled = false;
            }
        }
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
        {
            //Debug.Log(cameraTimerCopy);
            //When the timer expires, it disables the current player's camera and activates the next one
            if (cameraTimerCopy <= 0.0f)
            {
                players[index % players.Count].transform.Find("Main Camera").gameObject.SetActive(false);
                index += 1;
                players[index % players.Count].transform.Find("Main Camera").gameObject.SetActive(true);
                if (dummy.transform.Find("Main Camera").gameObject.activeSelf)
                {
                    dummy.GetComponent<ShootDummy>().enabled = true;
                    dummy.GetComponent<Dummy>().enabled = false;
                }
                else
                {
                    dummy.GetComponent<ShootDummy>().enabled = false;
                    dummy.GetComponent<Dummy>().enabled = true;
                }
                cameraTimerCopy = maxCameraTimer;
                PlayerUISwapper(players[index % players.Count].tag);

                //GameStateManager.Swap();
            }
            TimerUI.SetTimerUI((int)Mathf.Ceil(cameraTimerCopy));
            cameraTimerCopy -= Time.deltaTime;
            //GameStateManager.Resume();

            //This is so that integer overflow cannot occur
            if (index == 100)
            {
                index = index % players.Count;
            }

        }
    }

    private void PlayerUISwapper(string ptag)
    {
        //checks to see if the tag that was added to the player in index's tag is Player1's tag
        if (ptag.Equals(Player1Tag))
        {
            P1.color = new Color32(255, 255, 255, 255);
            P2.color = new Color32(0, 0, 0, 100);
        }
        else
        {
            P2.color = new Color32(255, 255, 255, 255);
            P1.color = new Color32(0, 0, 0, 100);
        }
    }
}
