using System.Collections;
using System.Collections.Generic;
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
    int deathCount;
    int killCount;
    bool hasDied;
    public PlayerHealth playerHealth;
    public HUDText hudTexts;
    public Timer shieldTimerObj; 
    public GrenadeThrower grenadeThrower;
    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());

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
        UpdateHUDTexts();
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
        hasShield = true;
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
        hasShield = false;
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
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
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
        if (!hasDied)
        {
            killCount++;
        }
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
}
