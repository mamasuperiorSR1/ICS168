using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Worked on by Joshua Wolfe

    public List<GameObject> models;
    [HideInInspector]
    public List<GameObject> players;
    private int index;
    [SerializeField]
    private float maxCameraTimer = 5.0f;
    private float cameraTimerCopy;
    [SerializeField]
    private float SwapTimer = .5f;
    [SerializeField]
    private string Player1Tag;
    [SerializeField]
    private string Player2Tag;
    [SerializeField]
    private RawImage P1;
    [SerializeField]
    private RawImage P2;
    [SerializeField]
    private Slider swapProgress;
    private float TotalTime;

    [SerializeField]
    private Mesh[] Meshes;
    private bool keyboardAndJoystick;

    void Start()
    {
        //This should make it Keyboard and Joystick since ControllerPrefs exists and isn't the default value
        //This is done by Ed Slee
        if (PlayerPrefs.GetInt("ControllerPref", 0) != 0)
        {
            keyboardAndJoystick = true;
        }
            //This should make it Keyboard only
        else
        {
            keyboardAndJoystick = false;
        }
        PlayerSpawn();
        RandomMeshes();
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

    public void PlayerSpawn()
    {
        GameStateManager.Resume();
        players = new List<GameObject>();

        //local multiplayer spawning
        foreach (Transform child in transform)
        {
            players.Add(Instantiate(models[Random.Range(0, models.Count)], child.gameObject.transform.position, Quaternion.identity));
        }

        index = Random.Range(0, players.Count);
        TotalTime = 0;


        /*
          NOTE: I know the "Find" function is controversial, but there are some reasons why "Find" is acceptable here:
          1: FindTag only finds ACTIVIVE game objects; since this code works by activating and deacting the camera child object of the player,
             that function will not work here.
          2: "Main Camera" is a universial name and is very unlikely for it to be changed at some point.
        */

        //Disable every player's "Main Camera" and activate the one randomly chosen above
        foreach (GameObject player in players)
        {
            player.transform.Find("Main Camera").gameObject.SetActive(false);
        }
        players[index].transform.Find("Main Camera").gameObject.SetActive(true);

        cameraTimerCopy = maxCameraTimer;

        swapProgress.gameObject.SetActive(false);

        TagAssignment();
    }


    private void TagAssignment()
    {
        //Set tags for the player
        int playerCount = 1;
        foreach (GameObject playerr in players)
        {
            playerr.tag = playerCount.ToString();
            playerCount += 1;
        }
        PlayerUISwapper(players[index].tag);

        if (keyboardAndJoystick)
        {
            players[1].name += " (joystick)";
        }
        
    }

    void Update()
    {   
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
        {
            //When the timer expires, it disables the current player's camera and activates the next one
            if (cameraTimerCopy <= 0.0f)
            {
                players[index % players.Count].transform.Find("Main Camera").gameObject.SetActive(false);
                index += 1;
                players[index % players.Count].transform.Find("Main Camera").gameObject.SetActive(true);
                cameraTimerCopy = maxCameraTimer;
                PlayerUISwapper(players[index % players.Count].tag);
                swapProgress.gameObject.SetActive(true);

                GameStateManager.Swap();
                StartCoroutine(SwapDelay());
            }
            TimerUI.SetTimerUI((int)Mathf.Ceil(cameraTimerCopy));
            cameraTimerCopy -= Time.deltaTime;

            //This is so that integer overflow cannot occur
            if (index == 100)
            {
                index = index % players.Count;
            }

        }
    }

    private void PlayerUISwapper(string ptag)
    {
        //This is done by Ed Slee

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

    private IEnumerator SwapDelay()
    {
        //This is done by Ed Slee
        while (TotalTime < SwapTimer)
        {
            TotalTime += Time.deltaTime;
            //if after adding delta time this condition is met, we want it to fill to 100% and nothing above
            if(TotalTime >= SwapTimer)
            {
                swapProgress.value = 1f;
            }
            else
            {
                swapProgress.value = TotalTime / SwapTimer;
            }
            yield return null;
        }
        TotalTime = 0f;
        swapProgress.value = 0f;
        swapProgress.gameObject.SetActive(false);
        GameStateManager.Resume();
    }
}
