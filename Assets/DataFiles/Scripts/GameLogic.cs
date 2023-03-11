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

    // Start is called before the first frame update
    void Start()
    {
        // Used with integration
        p1packetId = 0;
        p2packetId = 0;
        enemyPlayer = (connectedPlayer == 1) ? 2 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Used for app only demo
        // updateDeaths();
        // updateKills();
        // Used for both integration and app only demo
        UpdateHUDTexts();
        // For integration
        UpdateServer();
        UpdateHealth();
        UpdateActions();
    }

    // Integration Functions
    void UpdateServer()
    {
        if (serverComms.hasGrenadeCheck() && enemyVisible)
        {
            serverComms.setGrenadeHit(true);
            serverComms.setGrenadeCheck(false);
        }
    }

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
        player.SetOwnHealth((float)dataReceived.getOwnHealth(connectedPlayer));
        opponent.SetOpponentHealth((float)dataReceived.getEnemyHealth(enemyPlayer));
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
                    ProcessActions(p1Action);
                }
                break;
            case 2:
                break;
        }
    }

    void ProcessActions(string playerAction)
    {
        switch (playerAction)
        {
            case "shoot":
                HandlePlayerShoots();
                break;
        }
    }

    void HandlePlayerShoots()
    {
        ammoFirer.bulletAnimation();
    }

    // App-only demo functions
    void updateKills()
    {
        if (opponent.enemyHealth.getHealth() <= 0 && !opponent.GetHasDied())
        {
            opponent.SetHasDied(true);
            // killCount++;
            opponent.Respawn();
        }
        opponent.SetHasDied(false);
    }

    void updateDeaths()
    {
        if (player.playerHealth.getHealth() <= 0)
        {
            player.SetHasDied(true);
            // deathCount++;
            // Respawn timer here? 
            player.Respawn();
        }
        player.SetHasDied(false);
    }
    // End here 

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
    public void DealBulletDamageP1()
    {
        if (player.GetAmmoCount() > 0 && player.playerHealth.getHealth() > 0)
        {
            if (enemyVisible)
            {
                opponent.ReceiveDamage(10);
            }
            ammoFirer.bulletAnimation();
            player.shotFired();
        }
    }

    public void DealGrenadeDamageP1()
    {
        if (player.GetGrenadeCount() > 0 && player.playerHealth.getHealth() > 0)
        {
            grenadeThrower.ThrowGrenade();
            if (enemyVisible) Invoke("GrenadeDamageP1", 2.5f);
            player.grenadeThrown();
        }
    }

    void GrenadeDamageP1()
    {
        opponent.ReceiveDamage(30);
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

    void GrenadeDamageP2()
    {
        player.ReceiveDamage(30);
    }
}
