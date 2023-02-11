using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    const int AmmoCapacity = 6;
    int ammoCount;
    const int GrenadeCapacity = 2;
    int grenadeCount;
    const int ShieldCapacity = 3;
    int shieldCount;
    float shieldDamageCount;
    bool hasShield;
    bool enemyHasShield;
    public GameObject enemyShield;
    int deathCount;
    int killCount;
    bool hasDied;
    bool enemyDied;
    bool enemyVisible;
    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;
    public GameObject enemyHealthbarCanvas;
    public HUDText hudTexts;
    public Timer shieldTimerObj;
    public GameObject shieldScreen;
    public GameObject damageScreen;
    public GrenadeThrower grenadeThrower;
    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
        enemyHealth.SetEnemyHealth(100);
        enemyHealth.SetMaxHealth(100);
        shieldScreen.SetActive(false);
        damageScreen.SetActive(false);

        ammoCount = AmmoCapacity;
        grenadeCount = GrenadeCapacity;
        shieldCount = ShieldCapacity;
        shieldDamageCount = 0;
        deathCount = 0;
        killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        updateDeaths();
        updateKills();
        UpdateHUDTexts();
        ShieldTimerCheck();
    }

    void UpdateHUDTexts()
    {
        hudTexts.SetAmmoText(ammoCount + "/" + AmmoCapacity);
        hudTexts.SetGrenadeText(grenadeCount + "/" + GrenadeCapacity);
        hudTexts.SetShieldText(shieldCount + "/" + ShieldCapacity);
        hudTexts.SetKillCount(killCount.ToString());
        hudTexts.SetDeathCount(deathCount.ToString());
        hudTexts.SetShieldTimerText(shieldTimerObj.GetTime().ToString("0"));
    }

    public void Damage(float damagePoints)
    {
        // For P1
        if (playerHealth.getHealth() > 0)
        {
            float tempHealth = playerHealth.getHealth() - damagePoints;
            if (tempHealth < 0)
            {
                tempHealth = 0;
            }
            playerHealth.SetHealth(tempHealth);
        }
        if (hasShield)
        {
            shieldDamageCount += damagePoints;
        }
        // For P2
        if (enemyHealth.getHealth() > 0)
        {
            float tempEnemyHealth = enemyHealth.getHealth() - damagePoints;
            if (tempEnemyHealth < 0)
            {
                tempEnemyHealth = 0;
            }
            enemyHealth.SetEnemyHealth(tempEnemyHealth);
        }
    }

    void ShieldTimerCheck()
    {
        if (Math.Floor(shieldTimerObj.GetTime()) <= 0)
        {
            DeactivateShield();
        }
    }

    // Testing functions
    public void ToggleShield()
    {
        if (shieldCount > 0)
        {
            if (hasShield == true)
            {
                DeactivateShield();
            }
            else
            {
                ActivateShield();
            }
        }
    }

    void ActivateShield()
    {
        // Testing Rendering of enemy Shield
        enemyHasShield = true;
        enemyShield.SetActive(true);

        hasShield = true;
        shieldScreen.SetActive(true);
        shieldTimerObj.SetStartTimer(true);
        hudTexts.ToggleTimerText(true);
        playerHealth.SetMaxHealth(130);
        float currHealth = playerHealth.getHealth();
        float newHealth = currHealth + 30;
        playerHealth.SetHealth(newHealth);
        shieldCount--;
    }

    void DeactivateShield()
    {
        // Testing Rendering of enemy Shield
        enemyHasShield = false;
        enemyShield.SetActive(false);

        hasShield = false;
        shieldScreen.SetActive(false);
        shieldTimerObj.SetStartTimer(false);
        hudTexts.ToggleTimerText(false);
        playerHealth.SetMaxHealth(100);
        // Assuming no damage
        float currHealth = playerHealth.getHealth();
        float newHealth;

        if (shieldDamageCount < 30)
        {
            newHealth = currHealth - 30;
        }
        else
        {
            newHealth = currHealth;
        }
        playerHealth.SetHealth(newHealth);
    }

    public void DealBulletDamage()
    {
        float currHealth = playerHealth.getHealth();
        if (ammoCount > 0 && currHealth > 0)
        {
            Damage(10);
            ammoCount--;
        }
    }

    public void DealGrenadeDamage()
    {
        float currHealth = playerHealth.getHealth();
        if (grenadeCount > 0 && currHealth > 0)
        {
            grenadeThrower.ThrowGrenade();
            Damage(30);
            grenadeCount--;
        }
    }

    public void ResetHealth()
    {
        // ResetEnemyHealth();
        ResetPlayerHealth();
    }

    void ResetPlayerHealth()
    {
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
    }

    void ResetEnemyHealth()
    {
        enemyHealth.SetMaxHealth(100);
        enemyHealth.SetEnemyHealth(enemyHealth.getMaxHealth());
    }

    public void ReloadAmmo()
    {
        if (ammoCount == 0 || hasDied)
        {
            ammoCount = AmmoCapacity;
        }
    }

    public void ReloadGrenade()
    {
        if (grenadeCount == 0 || hasDied)
        {
            grenadeCount = GrenadeCapacity;
        }
    }

    public void ResetShieldCount()
    {
        shieldCount = ShieldCapacity;
    }

    void updateKills()
    {
        // If player hasn't died
        // if (!hasDied)
        // {
        if (enemyHealth.getHealth() <= 0 && !enemyDied)
        {
            enemyDied = true;
            killCount++;
            ResetEnemyHealth();
        }
        enemyDied = false;
        // }
    }

    void updateDeaths()
    {
        if (playerHealth.getHealth() <= 0)
        {
            hasDied = true;
            deathCount++;
            // Respawn timer here? 
            ResetShieldCount();
            ReloadAmmo();
            ReloadGrenade();
            ResetHealth();
        }
        hasDied = false;
    }

    public void showEnemyHealthBar()
    {
        enemyHealthbarCanvas.SetActive(true);
        if (enemyHasShield)
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
}
