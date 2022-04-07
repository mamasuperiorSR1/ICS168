using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created and Edited by Fabian (Yilong)
public class pistol : MonoBehaviour
{
    [SerializeField] private float m_Damage = 10f;
    [SerializeField] private float m_Range = 100f;
    [SerializeField] private float m_FireRate = 15f;

    [SerializeField] private int m_TotalAmmo = 30;
    [SerializeField] private int m_MaxAmmo = 6;
    [SerializeField] private int m_CurrentAmmo = 6;
    [SerializeField] private int m_AmmoFired = 0;
    [SerializeField] private float m_ReloadTime = 3f;

    private bool m_IsReloading = false;
    private bool m_IsShooting = false;
    private bool m_IsEmpty = false;
    private bool m_Reloading = false;

    [SerializeField] private Camera m_PlayerCam;
    [SerializeField] private ParticleSystem m_MuzzleFlash;
    [SerializeField] private GameObject m_ImpactEffect;

    private string shootAxis;
    private string reloadAxis;
    private string modeSwitchAxis;

    private string Axis;
    //[SerializeField] private Animator m_ReloadAnimator; No animation atm

    private float m_NextTimeToFire = 0f;

    private void Start()
    {
        m_CurrentAmmo = m_MaxAmmo;

        if (this.transform.parent.parent.parent.name.Contains("(joystick)"))
        {
            shootAxis = "JoystickFire";
            reloadAxis = "JoystickReload";
            modeSwitchAxis = "JoystickFireModeSwitch";
            //modeSwitchAxis
            //Axis = "Joystick";
        }
        else
        {
            shootAxis = "KeyboardFire";
            reloadAxis = "KeyboardReload";
            modeSwitchAxis = "KeyboardFireModeSwitch";
            //Axis = "Keyboard";
        }
    }

    public bool Getm_Reloading()
    {
        return m_Reloading;
    }

    public bool Getm_IsShooting()
    {
        return m_IsShooting;
    }

    public bool Getm_IsEmpty()
    {
        return m_IsEmpty;
    }

    public int Getm_TotalAmmo()
    {
        return m_TotalAmmo;
    }

    public int Getm_CurrentAmmo()
    {
        return m_CurrentAmmo;
    }

    private void OnEnable()
    {
        m_IsReloading = false;
        m_IsShooting = false;
        m_IsEmpty = false;
        //m_ReloadAnimator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsReloading)
            return;

        if (m_CurrentAmmo == 0 && m_TotalAmmo >0) // Reload current mag
        {
            StartCoroutine(IsReload());
            StartCoroutine(Reload());
            return;
        }

        if (m_TotalAmmo == 0 && m_CurrentAmmo == 0) // Check if there is ammo left
        {
            EmptyMag();
        }

        if (Input.GetKeyDown(reloadAxis) && m_CurrentAmmo < m_MaxAmmo) // Press R to reload
        {
            if(m_TotalAmmo > 0)
            {
                StartCoroutine(IsReload());
                StartCoroutine(Reload());
            }
        }

        //M1 will be the button to shoot
        if (Input.GetButtonDown(shootAxis) && m_CurrentAmmo > 0)
        {
            m_NextTimeToFire = Time.time + 1f / m_FireRate;
            Shoot();
        }
    }

    IEnumerator IsReload()
    {
        m_Reloading = true;
        yield return new WaitForSeconds(0.001f);
        m_Reloading = false;
    }

    IEnumerator Reload()
    {
        //Update the UI
        //m_Ammo.text = "Reloading...";
        //m_ReloadAnimator.SetBool("Reloading", true);
        m_IsReloading = true;
        if (m_AmmoFired < m_TotalAmmo && m_TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            m_TotalAmmo -= m_AmmoFired;
            m_AmmoFired = 0;
            m_CurrentAmmo = 0 + m_MaxAmmo;
        }
        if (m_AmmoFired >= m_TotalAmmo && m_TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            m_TotalAmmo = 0;
            m_AmmoFired = 0;
            m_CurrentAmmo = 0 + m_MaxAmmo;
        }
        //m_ReloadAnimator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        m_IsReloading = false;
    }

    IEnumerator IsShooting()
    {
        m_IsShooting = true;
        yield return new WaitForSeconds(0.001f);
        m_IsShooting = false;
    }

    private void Shoot()
    {
        if(!m_IsReloading)
        {
            StartCoroutine(IsShooting());
            m_CurrentAmmo--;
            m_AmmoFired++;
            m_MuzzleFlash.Play();
            //Cast a ray and check if you hit something
            RaycastHit hit;
            if (Physics.Raycast(m_PlayerCam.transform.position, m_PlayerCam.transform.forward, out hit, m_Range))
            {
                HealthManager enemy = hit.transform.GetComponent<HealthManager>();
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

    IEnumerator IsEmpty()
    {
        m_IsEmpty = true;
        yield return new WaitForSeconds(0.001f);
        m_IsEmpty = false;
    }

    private void EmptyMag() // No ammo left and weapon will not fire or reload
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(IsEmpty());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(IsEmpty());
        }
    }
}

