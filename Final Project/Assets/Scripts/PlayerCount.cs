using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class PlayerCount : MonoBehaviourPunCallbacks
{
    //Coded by Ed Slee

    //This class counts the number of players that exist in the scene, and stores them to be used by other classes

    [SerializeField]
    private string OnlinePlayerTagSetter;    //used only as a way to set the static variable, OnlinePlayerTag

    private static string OnlinePlayerTag;  //not the master

    [SerializeField]
    private string Player1TagSetter;      //used only as a way to set the static variable, Player1Tag

    public static string Player1Tag;

    private static GameObject Player1;

    [SerializeField]
    private string Player2TagSetter;      //used only as a way to set the static variable, Player2Tag

    public static string Player2Tag;

    private static int Count;

    private int NumberOfPlayers = 2;

    private bool Found;

    void Awake()
    {
        Player1Tag = Player1TagSetter;
        Player2Tag = Player2TagSetter;
        OnlinePlayerTag = OnlinePlayerTagSetter;
        Count = NumberOfPlayers;
        Found = false;
    }

    private void Update()
    {
        //this is done in update and not awake because otherwise both players will be null
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL && !Found)
        {
            Player1 = GameObject.FindGameObjectWithTag(Player1Tag);
            Found = true;
        }
    }

    //decreases the count by 1 and calls Gameover if there is only one player left
    public static void DecreaseCount()
    {
        Count--;
        //Debug.Log(Count);
        if (GetCount() == 1)
        {
            GameStateManager.Gameover();
        }
    }

    //returns the number of players left in the game
    public static int GetCount()
    {
        return Count;
    }

    //returns Players
    public static int Winner()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            if (Player1 != null)
            {
                try
                {
                    return Int32.Parse(Player1Tag);
                }
                catch
                {
                    return 1;
                }
                
            }
            else
            {
                try
                {
                    return Int32.Parse(Player2Tag);
                }
                catch
                {
                    return 2;
                }
            }
        }
        else if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            //Master will be represented as Player 1 and Player as PLayer 2
            try
            {
                if (GameObject.FindGameObjectWithTag(OnlinePlayerTag).GetComponent<PhotonView>().IsMine)
                {
                    //this means that the Player with tag Player is the last one left
                    try //Try, Catch block done by Joshua Wolfe
                    {
                        return Int32.Parse(Player2Tag);
                    }
                    catch
                    {
                        return 2;
                    }
                }
                else
                {
                    //is mine is false meaning that the Master still exists and is the one with the screen up, it just found Player in its hierarchy
                    try //Try, Catch block done by Joshua Wolfe
                    {
                        return Int32.Parse(Player1Tag);
                    }
                    catch
                    {
                        return 1;
                    }
                }
            }
            //For these two, Player with tag Player doesn't exist, so Master wins
            catch(ArgumentException)
            {
                try //Try, Catch block done by Joshua Wolfe
                {
                    return Int32.Parse(Player1Tag);
                }
                catch
                {
                    return 1;
                }
            }
            catch (NullReferenceException)
            {
                try //Try, Catch block done by Joshua Wolfe
                {
                    return Int32.Parse(Player1Tag);
                }
                catch
                {
                    return 1;
                }
            }
        }
        else
        {
            return 0;
        }
    }

}
