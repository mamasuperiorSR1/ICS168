using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ShootDummy : MonoBehaviour // Created by Fabian (Yilong), Rocka (Weilin), and Joshua
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask Ground, Player;

    // variables for patroling
    private Vector3 walkPoint;
    private bool walkPointSet = false;
    [SerializeField]
    private float walkPointRange = 5f;

    [SerializeField] private int pointrange = 2;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private Transform point4;
    [SerializeField] private Transform point5;
    [SerializeField] private Transform point6;
    [SerializeField] private Transform point7;
    [SerializeField] private Transform point8;
    [SerializeField] private Transform Shrink;
    [SerializeField] private Transform SpeedBoost;
    private bool shootCoolDown;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        shootCoolDown = true;
    }

    bool CanSeeTarget() //use a raycast to check if there is anything between dummy and player
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.position - this.transform.position;
        //perform a raycast to determine if there's anything between the agent and the target
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), rayToTarget, out raycastInfo))
        {
            if (raycastInfo.collider.CompareTag("Player")) //means dummy is in sight of player
            {
                return true;
            }
        }
        return false;
    }

    private void Patrol()
    {
        if (!walkPointSet)
        {
            GenerateWalkPoint();
            agent.SetDestination(walkPoint);
        }
    }

    private void GenerateWalkPoint()
    {
        do
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //generate random walk point
            walkPointSet = checkInBounds(walkPoint);
        }
        while (!walkPointSet);
    }

    private bool checkInBounds(Vector3 destination)
    {
        if (Physics.Raycast(destination, -transform.up, 2f, Ground))
        {
            return true; //check if the walk point is in play area
        }
        return false;
    }

    private void ChasePlayer()
    {
        walkPoint = player.position;
        walkPointSet = true;
        agent.SetDestination(walkPoint); //chase player :)
            
    }


    void shootCooldownReset()
    {
        if (SceneManager.GetActiveScene().name == "Map 1 with NavMesh") //This is our training mode scene; health should only regen during practice
        {
            player.GetComponent<HealthManager>().CurrentHealth = player.GetComponent<HealthManager>().MaxHealth;
        }
        shootCoolDown = true;
    }

    private void Update()
    {
        //Debug.DrawLine(transform.position, transform.forward);
        //Unlock the cursor when in pause or gameover menu
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PAUSE ||
            GameStateManager.GetState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (!CanSeeTarget() && !walkPointSet)
        {
            //player hiding near point2 cause dummy holding advantage position at point 7
            if (Vector3.Distance(player.position, point1.position) <= pointrange)
            {
                walkPoint = point7.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            else if (Vector3.Distance(player.position, point2.position) <= pointrange)
            {
                walkPoint = point7.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            else if (Vector3.Distance(player.position, point3.position) <= pointrange)
            {
                walkPoint = point7.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            //player hiding near point2 cause dummy holding advantage position at point 8
            else if (Vector3.Distance(player.position, point4.position) <= pointrange)
            {
                walkPoint = point8.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            else if (Vector3.Distance(player.position, point5.position) <= pointrange)
            {
                walkPoint = point8.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            else if (Vector3.Distance(player.position, point6.position) <= pointrange)
            {
                walkPoint = point8.position;
                walkPointSet = true;
                agent.SetDestination(walkPoint);
                //transform.LookAt(player);
            }
            else if (Vector3.Distance(agent.transform.position, Shrink.position) <= 15)
            {
                try
                {
                    walkPoint = Shrink.position;
                    walkPointSet = true;
                    agent.SetDestination(walkPoint);
                }
                catch
                {
                    walkPointSet = false;
                    Patrol();
                }
                
            }
            else if(Vector3.Distance(player.position, agent.transform.position) <= 25)
            {
                try
                {
                    walkPoint = SpeedBoost.position;
                    walkPointSet = true;
                    agent.SetDestination(walkPoint);
                }
                catch
                {
                    walkPointSet = false;
                    Patrol();
                }
            }
            else
            {
                Patrol();
            }
        }

        if (CanSeeTarget())
        {
            transform.LookAt(player);
            ChasePlayer();

            if (shootCoolDown)
            {
                shootCoolDown = false;
                Invoke("shootCooldownReset", 1.5f);
                //Debug.Log("Shoot");
                try
                {
                    gameObject.GetComponentsInChildren<DummyAR>()[0].Shoot();
                }
                catch
                {
                    //Debug.Log(gameObject.transform.GetChild(0).GetChild(1).name);
                    gameObject.transform.GetChild(0).GetChild(1).GetComponentsInChildren<DummyAR>()[0].Shoot();
                }
            }
        }
        //check if dummy reached the walk point

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1.5f)
        {
            walkPointSet = false;
        }
        agent.SetDestination(walkPoint);
        //Debug.Log(shootCoolDown);
    }
}
