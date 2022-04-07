using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour // Written by Fabian (Yilong)
{
    [SerializeField] private AudioSource m_shootingSound;
    [SerializeField] private AudioSource m_reloadingSound;
    [SerializeField] private AudioSource m_EmptyMagSound;

    private bool m_Reloading;
    private bool m_IsShooting;
    private bool m_IsEmpty;
    // Start is called before the first frame update
    void Start()
    {
        m_Reloading = gameObject.GetComponent<Gun>().Getm_Reloading();
        m_IsShooting = gameObject.GetComponent<Gun>().Getm_IsShooting();
        m_IsEmpty = gameObject.GetComponent<Gun>().Getm_IsEmpty();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.GetState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            m_Reloading = gameObject.GetComponent<Gun>().Getm_Reloading();
            m_IsShooting = gameObject.GetComponent<Gun>().Getm_IsShooting();
            m_IsEmpty = gameObject.GetComponent<Gun>().Getm_IsEmpty();
            if (m_Reloading)
            {
                m_reloadingSound.Play();
            }
            if (m_IsShooting)
            {
                m_shootingSound.Play();
            }
            if (m_IsEmpty)
            {
                m_EmptyMagSound.Play();
            }
        }
        else
        {
            m_reloadingSound.Pause();
            m_shootingSound.Pause();
            m_EmptyMagSound.Pause();
        }
        
    }
}
