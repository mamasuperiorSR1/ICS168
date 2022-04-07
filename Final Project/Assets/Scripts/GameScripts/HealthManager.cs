using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//Written by Benedict
public class HealthManager : MonoBehaviourPunCallbacks, Health
{
    private PhotonView view;

    //The scriptable object used for this enemy
    [SerializeField] private PlayerInformationScriptableObject m_EnemyScriptableObject;
    [SerializeField] private Text healthDisplay;

    //Health
    private float currentHealth;
    private float maxHealth;
    private float regenTime;
    private float TextHealth; //the number that should be displayed on the UI
    private float outOfCombatTimer = 3; //to manage when you are no longer damaged/in danger

    //Written by Ed
    private MeshRenderer Renderer;
    private float FlashTime;
    private Material OriginalMaterial;
    [SerializeField] private Material WhiteMaterial;

    private bool damaged;

    public float RegenTime { get => regenTime; set => regenTime = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool Damaged { get => damaged; set => damaged = value; }

    public static float StaticCurrentHealth;

    private void Start()
    {
        try
        {
            view = gameObject.GetComponent<PhotonView>();
        }
        catch(NullReferenceException)
        {

        }
        CurrentHealth = m_EnemyScriptableObject.GetHealth();
        //StaticCurrentHealth = CurrentHealth;
        MaxHealth = CurrentHealth;

        damaged = false;
        RegenTime = 1f;
        //Use for testing health related scripts
        //currentHealth = 45;
        //maxHealth = 100;

        //Update the health
        TextHealth = CurrentHealth;
        UpdateHealthUI();

        //Written by Ed
        Renderer = gameObject.GetComponent<MeshRenderer>();
        FlashTime = 0.1f;
        OriginalMaterial = Renderer.material;
    }

    //Wait until however long, then you are out of danger
    IEnumerator NotDamagedCheck()
    {
        yield return new WaitForSeconds(outOfCombatTimer);
        damaged = false;
    }

    //Updates health UI
    public void UpdateHealthUI()
    {
        //CurrentHealth = StaticCurrentHealth;
        healthDisplay.text = /*CurrentHealth*/TextHealth.ToString("0") + "/" + MaxHealth.ToString("0");
    }

    //Take damage
    public void TakeDamage(float damage)
    {
        if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            damaged = true;
            //Update the health

            //flash enemy white
            //Debug.Log("Should call Coroutine");
            StartCoroutine(FlashWhite());

            CurrentHealth -= damage;
            UpdateHealthUI();

            //Check if they die
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        else if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            view.RPC("RPC_TakeDamage", RpcTarget.Others, damage);
            StartCoroutine(FlashWhite());
        }
    }


    //This is only for online and essentially calls damage on another persons screen
    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if (!view.IsMine)
        {
            return;
        }
        Damaged = true;

        //flash enemy white
        Debug.Log("Should call Coroutine");
        StartCoroutine(FlashWhite());

        CurrentHealth -= damage;
        UpdateHealthUI();

        Damaged = false;
        //Check if they die
        if (CurrentHealth <= 0)
        {
            healthDisplay.text = "0/" + MaxHealth.ToString("0");
            Die();
        }
    }

    //coded by Ed 
    //makes this player flash white when hit
    public IEnumerator FlashWhite()
    {
        //Debug.LogError("Should be Flashing");
        Renderer.material = WhiteMaterial;
        yield return new WaitForSeconds(FlashTime);
        Renderer.material = OriginalMaterial;
    }

    //Die
    private void Die()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            //Debug.Log("I am in Die");
            PlayerCount.DecreaseCount();
            Destroy(gameObject);
        }
        else if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            PlayerCount.DecreaseCount();
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (TextHealth != CurrentHealth)
        {
            TextHealth = CurrentHealth;
            UpdateHealthUI();
        }
        if(damaged)
        {
            StartCoroutine(NotDamagedCheck());
        }
    }
}
