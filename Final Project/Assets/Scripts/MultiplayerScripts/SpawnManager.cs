using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Benedict; watched a tutorial
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private SpawnPoint[] spawnPoints;

    private void Awake()
    {
        Instance = this;
        //Get all the spawn points
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    //Get a random spawn point 
    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }

}
