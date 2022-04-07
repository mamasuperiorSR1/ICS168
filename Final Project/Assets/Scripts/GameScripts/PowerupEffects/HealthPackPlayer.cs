using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Benedict 3/5/2022
public class HealthPackPlayer : MonoBehaviour
{
    [SerializeField] private HealthManager health;

    private void Start()
    {
        health = GetComponent<HealthManager>();
    }

    //Gain health
    public void GainHealth(float heal)
    {
        //Heal to max if it overheals
        if ((health.CurrentHealth + heal) > health.MaxHealth)
        {
            health.CurrentHealth = health.MaxHealth;
        }
        else
        {
            health.CurrentHealth += heal;
        }
        health.UpdateHealthUI();
    }
}
