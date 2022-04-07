using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created by Fabian (Yilong)
public class GunUI : MonoBehaviour
{
    public enum FireMode { Semi, Auto }

    [SerializeField] private Text m_AmmoDisplay;
    [SerializeField] private Text m_FiringModeDisplay;
    private int TotalAmmo;
    private int CurrentAmmo;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAmmoUI();
        UpdateFiringModeUI(0);
    }

    public void UpdateAmmoUI() // Display current ammo in mag and ammo remaining
    {
        if (gameObject.GetComponent<Gun>().isActiveAndEnabled)
        {
            CurrentAmmo = gameObject.GetComponent<Gun>().Getm_CurrentAmmo();
            TotalAmmo = gameObject.GetComponent<Gun>().Getm_TotalAmmo();
        }
        else
        {
            CurrentAmmo = gameObject.GetComponent<DummyAR>().Getm_CurrentAmmo();
            TotalAmmo = gameObject.GetComponent<DummyAR>().Getm_TotalAmmo();
        }
        m_AmmoDisplay.text = $"{CurrentAmmo}/{TotalAmmo}";
    }

    public IEnumerator ReloadUI() // Display current ammo in mag and ammo remaining
    {
        m_AmmoDisplay.text = "Reloading...";
        yield return new WaitForSeconds(3f);
    }

    public void UpdateFiringModeUI(int FireMode) // Display firing mode
    {
        
        if (FireMode < 1) // 1 is Semi, 2 is Full
        {
            m_FiringModeDisplay.text = "Semi Auto";
        }
        else
        {
            m_FiringModeDisplay.text = "Full Auto";
        }
    }
}