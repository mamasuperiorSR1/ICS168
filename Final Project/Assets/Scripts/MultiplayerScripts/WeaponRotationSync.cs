using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Benedict
/// <summary>
/// Sync the gun with the camera's X rotation to look up and down
/// </summary>
public class WeaponRotationSync : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    //Sync the rotation
    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
