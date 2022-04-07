using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Benedict 3/5/2022
public class ShrinkPlayer : MonoBehaviour
{
    private float currentTime;
    private float duration;

    //Check if the effect was ever activated
    private bool activated = false;

    public void Shrink(float shrinkRatio, float duration)
    {
        activated = true;
        currentTime = Time.time;
        this.duration = duration;
        //Shrink the player
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x / shrinkRatio, localScale.y / shrinkRatio, localScale.z / shrinkRatio);
    }
    private void Update()
    {
        //Unshrink after the duration
        if (currentTime + duration < Time.time)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (activated)
            {
                activated = false;
                GetComponent<PowerupUI>().SetPowerUpText("No Effect");
            }
        }
    }
}
