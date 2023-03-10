using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject enemyShield;
    public GameObject enemyHealthbarCanvas;
    bool enemyVisible;
    int deathCount;
    int killCount;
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

    // Start is called before the first frame update
    void Start()
    {
        // Used with integration
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
        ProcessActions();
    }

    // Integration Functions
    void UpdateServer()
    {
        if (serverComms.hasGrenadeCheck() && enemyVisible) {
            serverComms.setGrenadeHit(true);
            serverComms.setGrenadeCheck(false);
        } 
    }

    void UpdateHUDTexts()
    {
        // For Integration
        hudTexts.SetKillCount(dataReceived.getOwnKills(connectedPlayer).ToString());
        hudTexts.SetDeathCount(dataReceived.getOwnDeaths(connectedPlayer).ToString());
        // hudTexts.SetKillCount(killCount.ToString());
        // hudTexts.SetDeathCount(deathCount.ToString());
    }

    void UpdateHealth()
    {
        player.SetOwnHealth((float)dataReceived.getOwnHealth(connectedPlayer));
        opponent.SetOpponentHealth((float)dataReceived.getEnemyHealth(enemyPlayer));
    }

    void ProcessActions()
    {
        if (connectedPlayer == 1)
        {
            string p1Action = dataReceived.getOwnAction(connectedPlayer);
            switch(p1Action)
            {
                case "Shoot":
                    HandlePlayerShoots();
                    break;
            }
        }
        else if (connectedPlayer == 2)
        {
            string p2Action = dataReceived.getOwnAction(connectedPlayer);
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
            killCount++;
            opponent.Respawn();
        }
        opponent.SetHasDied(false);
    }

    void updateDeaths()
    {
        if (player.playerHealth.getHealth() <= 0)
        {
            player.SetHasDied(true);
            deathCount++;
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
