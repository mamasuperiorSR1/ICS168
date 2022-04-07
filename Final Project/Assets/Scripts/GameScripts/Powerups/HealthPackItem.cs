using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Created by Benedict Hsueh 2/15/2022
public class HealthPackItem : MonoBehaviour, IPowerUp
{
    //The amount of health to heal
    [SerializeField] private float healthPack;

    //The gameobject that will recieve health
    private GameObject reciever;


    private void OnTriggerEnter(Collider other)
    {
        //7 is the player layer
        if (other.transform.tag.Equals("Player") || other.gameObject.layer == 7)
        {
            reciever = other.gameObject;
            ApplyEffect();
            Destroy();
        }
    }

    //Gain health
    public void ApplyEffect()
    {
        //Update the UI
        reciever.GetComponent<PowerupUI>().SetPowerUpText("Health Pack");

        reciever.GetComponent<HealthPackPlayer>().GainHealth(healthPack);
    }

    public void Destroy()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            //Always check if it has a parent first before destroying
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            Destroy(gameObject);
        }
        else if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            //Always check if it has a parent first before destroying
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            gameObject.SetActive(false);
        }
    }


}
