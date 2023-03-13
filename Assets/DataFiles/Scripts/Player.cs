using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHealth playerHealth;
    bool hasShield;
    bool onCooldown;
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
        if (Math.Floor(shieldTimerObj.GetTime()) <= 0)
        {
            if (hasShield) DeactivateShield();
            onCooldown = false;
            shieldTimerObj.SetStartTimer(false);
            hudTexts.ToggleTimerText(false);
        }
    }

    void ShowReloadAnimation()
    {
        reloadScreen.SetActive(true);
    }

    void HideReloadAnimation()
    {
        reloadScreen.SetActive(false);
    }

    void ResetHealth()
    {
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
    }

    public void ActivateShield()
    {
        hasShield = true;
        onCooldown = true;
        shieldScreen.SetActive(true);
        shieldTimerObj.SetStartTimer(true);
        hudTexts.ToggleTimerText(true);
    }

    public void DeactivateShield()
    {
        hasShield = false;
        shieldScreen.SetActive(false);
    }

    public void ReloadAmmo()
    {
        ShowReloadAnimation();
        Invoke("HideReloadAnimation", 0.5f);
        Invoke("ResetAmmoCapacity", 0.5f);
    }

    public void ReceiveDamage(float damagePoints)
    {
        // For P1
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

    // For setting with data received from json
    public void SetOwnHealth(float _health)
    {
        playerHealth.SetHealth(_health);
    }

    public void SetOwnMaxHealth(float _maxHealth)
    {
        playerHealth.SetMaxHealth(_maxHealth);
    }
}
