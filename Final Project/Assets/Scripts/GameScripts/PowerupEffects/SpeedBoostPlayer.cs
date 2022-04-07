using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Written by Benedict 3/5/2022
public class SpeedBoostPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    //SpeedboostSettings
    private float duration;
    private float currentTime;
    private float baseSpeed;

    //Check if the effect was ever activated
    private bool activated = false;

    private bool dummy;

    // Start is called before the first frame update
    void Start()
    {
        //Dummy assignment and logic done by Joshua Wolfe; checks if the player is an AI or person to use the appropriate componenets
        if (gameObject.tag == "Dummy")
        {
            dummy = true;
        }
        else
        {
            dummy = false;
        }

        if (dummy)
        {
            baseSpeed = GetComponent<NavMeshAgent>().speed;
        }
        else
        {
            playerMovement = GetComponent<PlayerMovement>();
            baseSpeed = playerMovement.PlayerSpeed;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get rid of speed boost once time is up
        if (currentTime + duration < Time.time)
        {
            if (dummy)
            {
                GetComponent<NavMeshAgent>().speed = baseSpeed;
            }
            else
            {
                playerMovement.PlayerSpeed = baseSpeed;
            }
            if(activated)
            {
                activated = false;
                GetComponent<PowerupUI>().SetPowerUpText("No Effect");
            }
        }
    }

    //Get the speedboost
    public void GetSpeedBoost(float speedBoost, float duration)
    {
        if (dummy)
        {
            GetComponent<NavMeshAgent>().speed += speedBoost;
        }
        else
        {
            playerMovement.PlayerSpeed += speedBoost;
        }
        this.duration = duration;
        currentTime = Time.time;
        activated = true;
    }
}
