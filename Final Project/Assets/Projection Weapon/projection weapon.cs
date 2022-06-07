using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//public enum FireMode { Semi, Auto }

// Written by Fabian (Yilong)
public class projection_weapon : MonoBehaviourPunCallbacks
{
    PhotonView view;

    [SerializeField] private float m_Damage = 10f;
    [SerializeField] private float m_Range = 100f;
    [SerializeField] private float m_FireRate = 15f;

    [SerializeField] private int m_TotalAmmo = 90;
    [SerializeField] private int m_MaxAmmo = 30;
    [SerializeField] private int m_CurrentAmmo = 30;
    [SerializeField] private int m_AmmoFired = 0;
    [SerializeField] private float m_ReloadTime = 1f;

    private bool m_IsReloading = false;
    private bool m_IsShooting = false;
    private bool m_IsEmpty = false;
    private bool m_Reloading = false;

    [SerializeField] private FireMode _FireMode = FireMode.Semi;

    [SerializeField] private Camera m_PlayerCam;
    [SerializeField] private ParticleSystem m_MuzzleFlash;
    [SerializeField] private GameObject m_ImpactEffect;
    [SerializeField] private string m_ImpactEffectName;

    //[SerializeField] private Animator m_ReloadAnimator; No animation atm

    private float m_NextTimeToFire = 0f;

    public int TotalAmmo { get => m_TotalAmmo; set => m_TotalAmmo = value; }
    public int MaxAmmo { get => m_MaxAmmo; set => m_MaxAmmo = value; }
    public int CurrentAmmo { get => m_CurrentAmmo; set => m_CurrentAmmo = value; }
    public int AmmoFired { get => m_AmmoFired; set => m_AmmoFired = value; }

    private string shootAxis;
    private string reloadAxis;
    private string modeSwitchAxis;

    private string Axis;

    private HealthManager enemy;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();

        CurrentAmmo = MaxAmmo;
        //Debug.Log(this.transform.parent.parent.parent.name);

        //Input Axis selection done by Joshua Wolfe
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            if (this.transform.parent.parent.parent.name.Contains("(joystick)"))
            {
                shootAxis = "JoystickFire";
                reloadAxis = "JoystickReload";
                modeSwitchAxis = "JoystickFireModeSwitch";
            }
            else
            {
                shootAxis = "KeyboardFire";
                reloadAxis = "KeyboardReload";
                modeSwitchAxis = "KeyboardFireModeSwitch";
            }
        }

        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            shootAxis = "KeyboardFire";
            reloadAxis = "KeyboardReload";
            modeSwitchAxis = "KeyboardFireModeSwitch";
            //Debug.Log(shootAxis);
        }

        //Debug.Log(reloadAxis);
        //Debug.Log(modeSwitchAxis);
    }

    public bool Getm_IsReloading()
    {
        return m_IsReloading;
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
        //Only run this code if it's local multiplayer
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            if (GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
            {
                if (Input.GetButtonDown(modeSwitchAxis))  // Press B once to reload
                {
                    if ((int)_FireMode < 1) // Determine the current fire mode and update the fire mode UI
                    {
                        _FireMode += 1;
                    }
                    else
                    {
                        _FireMode = 0;
                    }
                    GetComponent<GunUI>().UpdateFiringModeUI((int)_FireMode);
                }
                if (m_IsReloading)
                {
                    //Debug.Log("Here");
                    return;
                }
                else
                {
                    GetComponent<GunUI>().UpdateAmmoUI();
                }

                if (CurrentAmmo == 0 && TotalAmmo > 0)
                {
                    StartCoroutine(IsReload());
                    StartCoroutine(Reload());
                    StartCoroutine(GetComponent<GunUI>().ReloadUI());
                    return;
                }

                if (TotalAmmo == 0 && CurrentAmmo == 0) // Weapon cannot fire again and will make empty mag sound
                {
                    EmptyMag();
                }


                if (Input.GetButtonDown(reloadAxis) && CurrentAmmo < MaxAmmo) // Press R to reload
                {
                    if (TotalAmmo > 0)
                    {
                        StartCoroutine(IsReload());
                        StartCoroutine(Reload());
                        StartCoroutine(GetComponent<GunUI>().ReloadUI());
                    }
                }

                // M1 (Left mouse click) will be the button to shoot
                if (_FireMode == FireMode.Semi)
                {
                    if (Input.GetButtonDown(shootAxis) && CurrentAmmo > 0) // Weapon fire once when M1 is pressed
                    {
                        //Depending on the firerate, that will be how fast you shoot the gun
                        Shoot();
                    }
                }
                if (_FireMode == FireMode.Auto)
                {
                    if (Input.GetButton(shootAxis) && Time.time >= m_NextTimeToFire && CurrentAmmo > 0) // Weapon keeps firing when M1 is pressed
                    {
                        //Depending on the firerate, that will be how fast you shoot the gun
                        m_NextTimeToFire = Time.time + 1f / m_FireRate;
                        Shoot();
                    }
                }
            }
        }

        //Only shoot if you are the correct player in control and not paused
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE &&
            GameStateManager.GetState() != GameStateManager.GAMESTATE.PAUSE)
        {
            if (view.IsMine)
            {
                if (Input.GetButtonDown(modeSwitchAxis))  // Press B once to reload
                {
                    if ((int)_FireMode < 1) // Determine the current fire mode and update the fire mode UI
                    {
                        _FireMode += 1;
                    }
                    else
                    {
                        _FireMode = 0;
                    }
                    GetComponent<GunUI>().UpdateFiringModeUI((int)_FireMode);
                }
                if (m_IsReloading)
                {
                    //Debug.Log("Here");
                    return;
                }
                else
                {
                    GetComponent<GunUI>().UpdateAmmoUI();
                }

                if (CurrentAmmo == 0 && TotalAmmo > 0)
                {
                    StartCoroutine(IsReload());
                    StartCoroutine(Reload());
                    StartCoroutine(GetComponent<GunUI>().ReloadUI());
                    return;
                }

                if (TotalAmmo == 0 && CurrentAmmo == 0) // Weapon cannot fire again and will make empty mag sound
                {
                    EmptyMag();
                }


                if (Input.GetButtonDown(reloadAxis) && CurrentAmmo < MaxAmmo) // Press R to reload
                {
                    if (TotalAmmo > 0)
                    {
                        StartCoroutine(IsReload());
                        StartCoroutine(Reload());
                        StartCoroutine(GetComponent<GunUI>().ReloadUI());
                    }
                }

                // M1 (Left mouse click) will be the button to shoot
                if (_FireMode == FireMode.Semi)
                {
                    if (Input.GetButtonDown(shootAxis) && CurrentAmmo > 0) // Weapon fire once when M1 is pressed
                    {
                        //Depending on the firerate, that will be how fast you shoot the gun
                        Shoot();
                    }
                }
                if (_FireMode == FireMode.Auto)
                {
                    if (Input.GetButton(shootAxis) && Time.time >= m_NextTimeToFire && CurrentAmmo > 0) // Weapon keeps firing when M1 is pressed
                    {
                        //Depending on the firerate, that will be how fast you shoot the gun
                        m_NextTimeToFire = Time.time + 1f / m_FireRate;
                        Shoot();
                    }
                }
            }
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
        m_IsReloading = true;
        if (AmmoFired < TotalAmmo && TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            TotalAmmo -= AmmoFired;
            AmmoFired = 0;
            CurrentAmmo = 0 + MaxAmmo;
        }
        if (AmmoFired >= TotalAmmo && TotalAmmo > 0) // Check if there is ammo left to reload
        {
            yield return new WaitForSeconds(m_ReloadTime - .25f);
            TotalAmmo = 0;
            AmmoFired = 0;
            CurrentAmmo = 0 + MaxAmmo;
        }
        //m_ReloadAnimator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f); // Reload time
        m_IsReloading = false;

    }

    IEnumerator IsShooting()
    {
        m_IsShooting = true;
        yield return new WaitForSeconds(0.001f);
        m_IsShooting = false;
    }

    public void Shoot()
    {
        if (!m_IsReloading && GameStateManager.GetState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            StartCoroutine(IsShooting());
            CurrentAmmo--;
            AmmoFired++;
            m_MuzzleFlash.Play();
            //Cast a ray and check if you hit something
            RaycastHit hit;
            if (Physics.Raycast(m_PlayerCam.transform.position, m_PlayerCam.transform.forward, out hit, m_Range))
            {
                enemy = hit.transform.GetComponent<HealthManager>();
                //Check if enemy exists
                if (enemy != null)
                {
                    enemy.TakeDamage(m_Damage);
                }

                if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
                {
                    GameObject impactGO = Instantiate(m_ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 1f);
                }
                else if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
                {
                    GameObject impactGO = PhotonNetwork.Instantiate(m_ImpactEffectName, hit.point, Quaternion.LookRotation(hit.normal));
                    StartCoroutine(PhotonDestroy(impactGO));
                }
            }
        }
    }

    public IEnumerator PhotonDestroy(GameObject impactGO)
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(impactGO);
    }


    IEnumerator IsEmpty()
    {
        m_IsEmpty = true;
        yield return new WaitForSeconds(0.001f);
        m_IsEmpty = false;
    }

    private void EmptyMag() // No ammo left and weapon will not fire nor reload
    {
        if (Input.GetButtonDown(shootAxis))
        {
            StartCoroutine(IsEmpty());
        }
        if (Input.GetButtonDown(reloadAxis))
        {
            StartCoroutine(IsEmpty());
        }
    }
}