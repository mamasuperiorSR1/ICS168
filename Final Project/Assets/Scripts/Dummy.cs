using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour // Created by Fabian (Yilong), Rocka (Weilin), and Joshua Wolfe
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask Ground, Player;

    // variables for patroling
    private Vector3 walkPoint;
    private bool walkPointSet = false;
    [SerializeField]
    private float walkPointRange = 5f;

    private void Awake()
    {
        //player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    bool CanSeeTarget() //use a raycast to check if there is anything between dummy and player
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.position - this.transform.position;
        //perform a raycast to determine if there's anything between the agent and the target
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.CompareTag("Player")) //means dummy is in sight of player
            {
                return true;
            }
        }
        return false;
    }

    private void Evade() //generate a vector for dummy to hide
    {
        if (!walkPointSet)
        {
            Vector3 targetDir = player.position - agent.transform.position;
            float lookAhead = targetDir.magnitude * -2f; //change the direction of the vector so dummy runs away from player
            Flee(player.position + player.forward * lookAhead);
        }
    }

    private void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - agent.transform.position;
        walkPoint = agent.transform.position - fleeVector;
        walkPointSet = checkInBounds(walkPoint);
        if (!walkPointSet)
        {
            GenerateWalkPoint();
        }
        agent.SetDestination(walkPoint);
    }

    private void Patrol()
    {
        if(!walkPointSet)
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

    private void Update()
    {
        if (!CanSeeTarget())
        {
            Patrol();
        }

        if (CanSeeTarget())// && seeCoolDown)
        {
            Evade();
        }
        //check if dummy reached the walk point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1.5f)
        {
            walkPointSet = false;
        }
        agent.SetDestination(walkPoint);
    }
}
