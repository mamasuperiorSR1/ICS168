using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Benedict 3/5/2022
public class HealthRegenPlayer : MonoBehaviour
{
    [SerializeField] private HealthManager health;

    //Check if the effect was ever activated
    private bool activated = false;

    private void Start()
    {
        health = GetComponent<HealthManager>();
    }

    //Regen Health
    public void RegenHealth(float heal)
    {
        activated = true;
        StartCoroutine(RegenHealthRoutine(heal));
    }

    private IEnumerator RegenHealthRoutine(float heal)
    {
        while (health.CurrentHealth < health.MaxHealth)
        {
            if (health.Damaged)
            {
                if (activated)
                {
                    activated = false;
                    GetComponent<PowerupUI>().SetPowerUpText("No Effect");
                }
                yield break;
            }
            health.CurrentHealth += heal;
            //Check if you overheal
            if (health.CurrentHealth > health.MaxHealth)
            {
                health.CurrentHealth = health.MaxHealth;
            }
            health.UpdateHealthUI();
            yield return new WaitForSeconds(health.RegenTime);
            //If you get damaged, you stop healing over time
        }
        if (activated)
        {
            activated = false;
            GetComponent<PowerupUI>().SetPowerUpText("No Effect");
        }
    }
}
