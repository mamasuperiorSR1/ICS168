using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Benedict
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject graphics;

    //Hide the visuals of the spawnpoint
    private void Awake()
    {
        graphics.SetActive(false);
    }
}
