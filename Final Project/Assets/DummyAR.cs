using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAR : MonoBehaviour //Created by Fabian (Yilong) for shooting dummy only to fix firing sound issue
{
    [SerializeField] private float m_Damage = 10f;
    [SerializeField] private float m_Range = 100f;
    [SerializeField] private float m_FireRate = 15f;

    [SerializeField] private int m_TotalAmmo = 90;
    [SerializeField] private int m_MaxAmmo = 30;
    [SerializeField] private int m_CurrentAmmo = 30;
    [SerializeField] private int m_AmmoFired = 0;
    [SerializeField] private float m_ReloadTime = 1f;
    private bool m_IsReloading = false;

    [SerializeField] private Dummy m_Dummy;
    [SerializeField] private ParticleSystem m_MuzzleFlash;
    [SerializeField] private GameObject m_ImpactEffect;
    public AudioSource m_shootingSound;
    public AudioSource m_reloadingSound;
    public AudioSource m_EmptyMagSound;

    private float m_NextTimeToFire = 0f;

    private HealthManager enemy;

    public int Getm_TotalAmmo()
    {
        return m_TotalAmmo;
    }

    public int Getm_CurrentAmmo()
    {
        return m_CurrentAmmo;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentAmmo = m_MaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
        {
            if (m_IsReloading)
                return;
            else
            {
                GetComponent<GunUI>().UpdateAmmoUI();
            }
            if (m_CurrentAmmo <= 0 && m_TotalAmmo > 0)
            {
                StartCoroutine(Reload());
                StartCoroutine(GetComponent<GunUI>().ReloadUI());
                return;
            }
        }
    }
    
    IEnumerator Reload()
    {
        m_IsReloading = true;
        if (m_AmmoFired < m_TotalAmmo && m_TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            m_CurrentAmmo = m_MaxAmmo;
            m_TotalAmmo -= m_AmmoFired;
            m_AmmoFired = 0;
        }
        if (m_AmmoFired >= m_TotalAmmo && m_TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            m_CurrentAmmo = m_TotalAmmo;
            m_TotalAmmo = 0;
            m_AmmoFired = 0;
        }
        m_IsReloading = false;
        yield return new WaitForSeconds(.25f); // Reload time
    }

    public void RefillAmmo()
    {
        m_CurrentAmmo = m_MaxAmmo;
        m_TotalAmmo = 90;
        m_AmmoFired = 0;
    }
    
    public void Shoot()
    {
        //Debug.Log("SHOOOOOOOOOOOOOOOT");
        m_CurrentAmmo--;
        m_AmmoFired += 1;
        m_MuzzleFlash.Play();
        m_shootingSound.Play();
        RaycastHit hit;
        if (Physics.Raycast(m_Dummy.transform.position + new Vector3(0,1,0), m_Dummy.transform.forward, out hit, m_Range))
        {
            enemy = hit.transform.GetComponent<HealthManager>();
            //Check if enemy exists
            if (enemy != null)
            {
                enemy.TakeDamage(m_Damage);
            }
            GameObject impactGO = Instantiate(m_ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }
}
