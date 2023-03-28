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
    public GameObject enemyShieldHealthBar;
    public Player player;
    public Opponent opponent;
    public HUDText hudTexts;
    public GrenadeThrower grenadeThrower;
    public EnemyGrenadeThrower enemyGrenadeThrower;
    public RayGun ammoFirer;
    public Console serverComms;
    public JSONReader dataReceived;
    int connectedPlayer = GlobalStates.GetPlayerNo(); 
    // int connectedPlayer = 1; //For testing only
    int enemyPlayer;
    bool enemyVisible;
    int ownPacketId;
    int enemyPacketId;
    int isP1ShieldActivated;
    int isP2ShieldActivated;

    // Start is called before the first frame update
    void Start()
    {
        enemyShieldHealthBar.SetActive(false);
        enemyShield.SetActive(false);
        // Used with integration
        ownPacketId = 0;
        enemyPacketId = 0;
        isP1ShieldActivated = 0;
        enemyPlayer = (connectedPlayer == 1) ? 2 : 1;
        // Connect on scene Change
        serverComms.connect();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDTexts();
        UpdateHealth();
        UpdateShield();
        UpdateActions();
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
        float playerShieldHealth = (float)dataReceived.getOwnShieldHealth(connectedPlayer);
        float enemyShieldHealth = (float)dataReceived.getEnemyShieldHealth(enemyPlayer);
        player.SetOwnHealth((float)dataReceived.getOwnHealth(connectedPlayer) + playerShieldHealth);
        opponent.SetOpponentHealth((float)dataReceived.getEnemyHealth(enemyPlayer));
        opponent.SetOppponentShieldHealth(enemyShieldHealth);
        player.SetOwnMaxHealth(100 + playerShieldHealth);
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
                if (isP2ShieldActivated != dataReceived.isEnemyShieldActivated(enemyPlayer))
                {
                    if (dataReceived.isEnemyShieldActivated(connectedPlayer) == 0)
                    {
                        opponent.DeactivateShield();
                    }
                }
                break;
        }
    }

    void UpdateActions()
    {
        switch (connectedPlayer)
        {
            case 1:
                if (dataReceived.getOwnId(connectedPlayer) != ownPacketId)
                {
                    ownPacketId = dataReceived.getOwnId(connectedPlayer);
                    // Process Own Actions
                    string ownAction = dataReceived.getOwnAction(connectedPlayer);
                    ProcessActions(ownAction, 1);
                }
                if (dataReceived.getEnemyId(enemyPlayer) != enemyPacketId)
                {
                    enemyPacketId = dataReceived.getEnemyId(enemyPlayer);
                    // Process Enemy Actions
                    string enemyAction = dataReceived.getEnemyAction(enemyPlayer);
                    ProcessActions(enemyAction, 2);
                }
                break;
            case 2:
                if (dataReceived.getOwnId(connectedPlayer) != ownPacketId)
                {
                    ownPacketId = dataReceived.getOwnId(connectedPlayer);
                    // Process Own Actions
                    string ownAction = dataReceived.getOwnAction(connectedPlayer);
                    ProcessActions(ownAction, 2);
                }
                if (dataReceived.getEnemyId(enemyPlayer) != enemyPacketId)
                {
                    enemyPacketId = dataReceived.getEnemyId(enemyPlayer);
                    // Process Enemy Actions
                    string enemyAction = dataReceived.getEnemyAction(enemyPlayer);
                    ProcessActions(enemyAction, 1);
                }
                break;
        }
    }

    void ProcessActions(string playerAction, int caller)
    {
        switch (playerAction)
        {
            case "shoot":
                // For P1
                if (caller == connectedPlayer)
                {
                    HandlePlayerShoots();
                }
                break;
            case "reload":
                // For P1
                HandlePlayerReload();
                break;
            case "shield":
                if (caller == connectedPlayer)
                {
                    HandlePlayerShield();
                }
                else
                {
                    HandleEnemyShield();
                }
                break;
            case "grenade_hit":
                // Receive Damage from getting grenaded
                if (caller == connectedPlayer)
                {
                    PlayerReceiveGrenadeDamage();
                }
                break;
            case "grenade_miss":
                break;
            case "hit":
                // Receive Damage for getting shot
                if (caller == connectedPlayer)
                {
                    PlayerReceiveDamage();
                }
                break;
            case "grenade":
                // Check if player is visible, update engine result
                if (caller == connectedPlayer)
                {
                    HandleThrowGrenade();
                }
                break;
        }
    }

    void HandlePlayerShoots()
    {
        ammoFirer.bulletAnimation();
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

    void HandleEnemyShield()
    {
        isP2ShieldActivated = dataReceived.isEnemyShieldActivated(enemyPlayer);
        opponent.ActivateShield();
    }

    void HandleThrowGrenade()
    {
        // Animation
        grenadeThrower.ThrowGrenade();
        serverComms.setGrenadeCheck(true);

        // Return enemy visibility to game engine
        if (enemyVisible)
        {
            Debug.Log("Grenade Hit");
            serverComms.setGrenadeHit(true);
        }
        else
        {
            serverComms.setGrenadeHit(false);
        }
    }

    void PlayerReceiveDamage()
    {
        Debug.Log("Hit by bullet");
        player.ReceiveDamage();
    }

    void PlayerReceiveGrenadeDamage()
    {
        Debug.Log("Hit by Grenade");
        enemyGrenadeThrower.ThrowGrenade();
        player.ReceiveDamage();
    }

    public void showEnemyHealthBar()
    {
        enemyHealthbarCanvas.SetActive(true);
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

    // public void DealGrenadeDamageP2()
    // {
        // if (opponent.GetGrenadeCount() > 0 && opponent.enemyHealth.getHealth() > 0)
        // {
        //     enemyGrenadeThrower.ThrowGrenade();
        //     // grenadeThrower.ThrowGrenade();
        //     Invoke("GrenadeDamageP2", 2.5f);
        //     opponent.GrenadeThrown();
        // }
    // }
}
