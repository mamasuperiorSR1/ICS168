using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Benedict Hsueh 2/15/2022
public interface IPowerUp
{
    public void ApplyEffect();
    public void Destroy();
}

public interface Health
{
    public void TakeDamage(float damage);
   
}
