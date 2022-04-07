using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Benedict 3/5/2022
public class RefillAmmoPlayer : MonoBehaviour
{
    [SerializeField] private Gun[] gunInformation;
    [SerializeField] private GunUI[] reload;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RefillAmmo()
    {
        foreach(Gun obj in gunInformation)
        {
            obj.CurrentAmmo = obj.MaxAmmo;
            obj.TotalAmmo = 90;
            obj.AmmoFired = 0;
        }
        foreach (GunUI obj in reload)
        {
            obj.UpdateAmmoUI();
        }
    }
}
