using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHealth playerHealth;
    int shieldCount;
    float shieldDamageCount;
    const int ShieldCapacity = 3;
    bool hasShield;
    bool onCooldown;
    const int AmmoCapacity = 6;
    int ammoCount;
    const int GrenadeCapacity = 2;
    int grenadeCount;
    bool receivedDamage;
    bool hasDied;
    public HUDText hudTexts;
    public Timer shieldTimerObj;
    public GameObject shieldScreen;
    public GameObject damageScreen;
    public GameObject reloadScreen;

    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        onCooldown = false;
        shieldScreen.SetActive(false);
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
        shieldCount = ShieldCapacity;
        shieldDamageCount = 0;

        ammoCount = AmmoCapacity;
        grenadeCount = GrenadeCapacity;

        damageScreen.SetActive(false);
        reloadScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDTexts();
        ShieldTimerCheck();
        checkReceivedDamage();
    }

    void UpdateHUDTexts()
    {
        hudTexts.SetAmmoText(ammoCount + "/" + AmmoCapacity);
        hudTexts.SetGrenadeText(grenadeCount + "/" + GrenadeCapacity);
        hudTexts.SetShieldText(shieldCount + "/" + ShieldCapacity);
        hudTexts.SetShieldTimerText(shieldTimerObj.GetTime().ToString("0"));
    }

    void checkReceivedDamage()
    {
        if (receivedDamage && !hasShield)
        {
            activateDamageScreen();
            Invoke("deactivateDamageScreen", 0.5f);
            receivedDamage = false;
        }
    }

    void activateDamageScreen()
    {
        damageScreen.SetActive(true);
    }

    void deactivateDamageScreen()
    {
        damageScreen.SetActive(false);
    }

    void ShieldTimerCheck()
    {
        if (shieldDamageCount > 30 && hasShield)
        {
            DeactivateShield();
        }
        if (Math.Floor(shieldTimerObj.GetTime()) <= 0)
        {
            if (hasShield) DeactivateShield();
            onCooldown = false;
            shieldTimerObj.SetStartTimer(false);
            hudTexts.ToggleTimerText(false);
        }
    }

    void ActivateShield()
    {
        hasShield = true;
        onCooldown = true;
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
        hasShield = false;
        shieldScreen.SetActive(false);
        // shieldTimerObj.SetStartTimer(false);
        // hudTexts.ToggleTimerText(false);
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
        shieldDamageCount = 0;
    }

    void ShowReloadAnimation()
    {
        reloadScreen.SetActive(true);
    }

    void HideReloadAnimation()
    {
        reloadScreen.SetActive(false);
    }

    void ResetAmmoCapacity()
    {
        ammoCount = AmmoCapacity;
    }

    void ResetShieldCount()
    {
        shieldCount = ShieldCapacity;
    }

    void ResetHealth()
    {
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
    }

    void ReloadGrenade()
    {
        if (grenadeCount == 0 || hasDied)
        {
            grenadeCount = GrenadeCapacity;
        }
    }

    public void ReloadAmmo()
    {
        if (hasDied)
        {
            ResetAmmoCapacity();
        }
        if (ammoCount == 0 && !hasDied)
        {
            ShowReloadAnimation();
            Invoke("HideReloadAnimation", 0.5f);
            Invoke("ResetAmmoCapacity", 0.5f);
        }
    }

    public void TogglePlayerShield()
    {
        if (shieldCount > 0)
        {
            if (hasShield == true)
            {
                DeactivateShield();
            }
            else
            {
                if (onCooldown) Debug.Log("Cooldown period");
                if (!onCooldown) ActivateShield();
                // if (shieldTimerObj.GetTime() == 10) ActivateShield();
            }
        }
    }

    public int GetShieldCount()
    {
        return shieldCount;
    }

    public void ReceiveDamage(float damagePoints)
    {
        // For P1
        if (hasShield)
        {
            shieldDamageCount += damagePoints;
        }
        if (playerHealth.getHealth() > 0)
        {
            receivedDamage = true;
            float tempHealth = playerHealth.getHealth() - damagePoints;
            if (tempHealth < 0)
            {
                tempHealth = 0;
            }
            playerHealth.SetHealth(tempHealth);
        }
    }

    public void Respawn()
    {
        ResetShieldCount();
        ReloadAmmo();
        ReloadGrenade();
        ResetHealth();
    }

    // Getters and Setters
    public bool GetHasDied()
    {
        return hasDied;
    }

    public void SetHasDied(bool status)
    {
        hasDied = status;
    }

    public int GetGrenadeCount()
    {
        return grenadeCount;
    }

    public int GetAmmoCount()
    {
        return ammoCount;
    }

    public void shotFired()
    {
        ammoCount--;
    }

    public void grenadeThrown()
    {
        grenadeCount--;
    }

    // For setting with data received from json
    public void SetOwnHealth(float _health)
    {
        playerHealth.SetHealth(_health);
    }
}
