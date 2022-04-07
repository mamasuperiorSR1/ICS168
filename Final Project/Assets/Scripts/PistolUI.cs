using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolUI : MonoBehaviour // Written by Fabian (Yilong)
{
    [SerializeField] private Text m_AmmoDisplay;
    [SerializeField] private Text m_FiringModeDisplay;
    private int TotalAmmo;
    private int CurrentAmmo;
    bool m_IsReloading = false;

    private string Axis;
    private string reloadAxis;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = gameObject.GetComponent<pistol>().Getm_CurrentAmmo();
        TotalAmmo = gameObject.GetComponent<pistol>().Getm_TotalAmmo();
        UpdateAmmoUI();
        if (this.transform.parent.parent.parent.name.Contains("(joystick)"))
        {
            reloadAxis = "JoystickReload";
            Axis = "JoystickFireModeSwitch";
        }
        else
        {
            reloadAxis = "KeyboardReload";
            Axis = "KeyboardFireModeSwitch";
        }
    }

    public void UpdateAmmoUI() // Display current ammo in mag and ammo remaining
    {
        CurrentAmmo = gameObject.GetComponent<pistol>().Getm_CurrentAmmo();
        TotalAmmo = gameObject.GetComponent<pistol>().Getm_TotalAmmo();
        m_AmmoDisplay.text = $"{CurrentAmmo}/{TotalAmmo}";
    }

    IEnumerator ReloadUI() // Display current ammo in mag and ammo remaining
    {
        m_IsReloading = true;
        m_AmmoDisplay.text = "Reloading...";
        yield return new WaitForSeconds(3f);
        m_IsReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(reloadAxis))
        {
            StartCoroutine(ReloadUI());
        }
        if (!Input.GetButtonDown(reloadAxis) && !m_IsReloading)
        {
            UpdateAmmoUI();
        }
    }
}