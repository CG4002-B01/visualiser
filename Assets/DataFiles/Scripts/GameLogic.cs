using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    const int AmmoCapacity = 6;
    const int GrenadeCapacity = 2;
    const int ShieldCapacity = 3;
    public GameObject enemyShield;
    public GameObject enemyHealthbarCanvas;
    public Player player;
    public Opponent opponent;
    public HUDText hudTexts;
    public GrenadeThrower grenadeThrower;
    public EnemyGrenadeThrower enemyGrenadeThrower;
    public RayGun ammoFirer;
    public Console serverComms;
    public JSONReader dataReceived;
    // int connectedPlayer = GlobalStates.GetPlayerNo(); 
    int connectedPlayer = 1; //For testing only
    int enemyPlayer;
    bool enemyVisible;
    int p1packetId;
    int p2packetId;
    int isP1ShieldActivated;

    // Start is called before the first frame update
    void Start()
    {
        // Used with integration
        p1packetId = 0;
        p2packetId = 0;
        isP1ShieldActivated = 0;
        enemyPlayer = (connectedPlayer == 1) ? 2 : 1;
        // Connect on scene Change
        serverComms.connect();
    }

    // Update is called once per frame
    void Update()
    {
        // Used for both integration and app only demo
        UpdateHUDTexts();
        // For integration
        // UpdateServer();
        UpdateHealth();
        UpdateShield();
        UpdateActions();
    }

    // Integration Functions
    // void UpdateServer()
    // {
    //     if (serverComms.hasGrenadeCheck() && enemyVisible)
    //     {
    //         serverComms.setGrenadeHit(true);
    //         serverComms.setGrenadeCheck(false);
    //     }
    // }

    void UpdateHUDTexts()
    {
        // For Integration
        hudTexts.SetKillCount(dataReceived.getOwnKills(connectedPlayer).ToString());
        hudTexts.SetDeathCount(dataReceived.getOwnDeaths(connectedPlayer).ToString());
        hudTexts.SetAmmoText(dataReceived.getOwnBulletNum(connectedPlayer) + "/" + AmmoCapacity);
        hudTexts.SetGrenadeText(dataReceived.getOwnGrenade(connectedPlayer) + "/" + GrenadeCapacity);
        hudTexts.SetShieldText(dataReceived.getOwnShieldNum(connectedPlayer) + "/" + ShieldCapacity);
    }

    void UpdateHealth()
    {
        float playerShieldHealth = (float)dataReceived.getOwnShieldHealth(connectedPlayer);
        float enemyShieldHealth = (float)dataReceived.getEnemyShieldHealth(enemyPlayer);
        player.SetOwnHealth((float)dataReceived.getOwnHealth(connectedPlayer) + playerShieldHealth);
        opponent.SetOpponentHealth((float)dataReceived.getEnemyHealth(enemyPlayer) + enemyShieldHealth);
        player.SetOwnMaxHealth(100 + playerShieldHealth);
        // Set 1 for opponent too
    }

    void UpdateShield()
    {
        switch (connectedPlayer)
        {
            case 1:
                if (isP1ShieldActivated != dataReceived.isOwnShieldActivated(connectedPlayer))
                {
                    if (dataReceived.isOwnShieldActivated(connectedPlayer) == 0)
                    {
                        player.DeactivateShield();
                    }
                }
                break;
            case 2:
                break;
        }
    }

    void UpdateActions()
    {
        switch (connectedPlayer)
        {
            case 1:
                if (dataReceived.getOwnId(connectedPlayer) != p1packetId)
                {
                    p1packetId = dataReceived.getOwnId(connectedPlayer);
                    // Process Actions
                    string p1Action = dataReceived.getOwnAction(connectedPlayer);
                    ProcessActions(p1Action, 1);
                }
                break;
            case 2:
                break;
        }
    }

    void ProcessActions(string playerAction, int caller)
    {
        switch (playerAction)
        {
            case "shoot":
                HandlePlayerShoots();
                break;
            case "reload":
                HandlePlayerReload();
                break;
            case "shield":
                if (caller == connectedPlayer)
                {
                    HandlePlayerShield();
                }
                else
                {
                    // HandlePlayerShield(enemyShieldHealth);
                    // Or should this be handle enemy shield. 
                }
                break;
            case "throw":
                // Check if player is visible, update engine result
                // Show grenade animation regardless for thrower
                HandleThrowGrenade();
                break;
        }
    }

    void HandlePlayerShoots()
    {
        ammoFirer.bulletAnimation();
        // Damage Screen for enemy
    }

    void HandlePlayerReload()
    {
        player.ReloadAmmo();
    }

    void HandlePlayerShield()
    {
        isP1ShieldActivated = dataReceived.isOwnShieldActivated(connectedPlayer);
        player.ActivateShield();
    }

    void HandleThrowGrenade()
    {
        Debug.Log("Throw Grenade");
        // Animation
        grenadeThrower.ThrowGrenade();

        // Return enemy visibility to game engine
        if (enemyVisible)
        {
            serverComms.setGrenadeHit(true);
        }
    }

    public void showEnemyHealthBar()
    {
        enemyHealthbarCanvas.SetActive(true);
        if (opponent.GetHasShield())
        {
            enemyShield.SetActive(true);
        }
        else
        {
            enemyShield.SetActive(false);
        }
        enemyVisible = true;
    }

    public void hideEnemyHealthBar()
    {
        enemyHealthbarCanvas.SetActive(false);
        enemyVisible = false;
    }

    public bool getEnemyVisible()
    {
        return enemyVisible;
    }

    // Testing functions
    public void DealGrenadeDamageP1()
    {
        if (player.GetGrenadeCount() > 0 && player.playerHealth.getHealth() > 0)
        {
            grenadeThrower.ThrowGrenade();
            // if (enemyVisible) Invoke("GrenadeDamageP1", 2.5f);
            // player.grenadeThrown();
        }
    }

    public void DealBulletDamageP2()
    {
        if (opponent.GetAmmoCount() > 0 && opponent.enemyHealth.getHealth() > 0)
        {
            player.ReceiveDamage(10);
            opponent.ShotFired();
        }
    }

    public void DealGrenadeDamageP2()
    {
        if (opponent.GetGrenadeCount() > 0 && opponent.enemyHealth.getHealth() > 0)
        {
            enemyGrenadeThrower.ThrowGrenade();
            // grenadeThrower.ThrowGrenade();
            Invoke("GrenadeDamageP2", 2.5f);
            opponent.GrenadeThrown();
        }
    }
}
