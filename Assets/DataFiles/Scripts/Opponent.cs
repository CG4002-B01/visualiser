using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    const int ShieldCapacity = 3;
    const int AmmoCapacity = 6;
    const int GrenadeCapacity = 2;
    public EnemyHealth enemyHealth;
    public GameObject enemyShield;
    public TimerOpp shieldTimerObj;
    bool hasDied;
    bool hasShield;
    bool onCooldown;
    int ammoCount;
    int grenadeCount;
    int shieldCount;
    int shieldDamageCount;

    // Start is called before the first frame update
    void Start()
    {
        ammoCount = AmmoCapacity;
        grenadeCount = GrenadeCapacity;
        shieldCount = ShieldCapacity;

        enemyHealth.SetEnemyHealth(100);
        enemyHealth.SetMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        ShieldTimerCheck();
    }

    void ActivateShield()
    {
        hasShield = true;
        enemyShield.SetActive(true);

        onCooldown = true;
        shieldTimerObj.SetStartTimer(true);
        enemyHealth.SetMaxHealth(130);
        float currHealth = enemyHealth.getHealth();
        float newHealth = currHealth + 30;
        enemyHealth.SetEnemyHealth(newHealth);
        shieldCount--;
    }

    void DeactivateShield()
    {
        hasShield = false;
        enemyShield.SetActive(false);

        enemyHealth.SetMaxHealth(100);
        // Assuming no damage
        float currHealth = enemyHealth.getHealth();
        float newHealth;

        if (shieldDamageCount < 30)
        {
            newHealth = currHealth - 30;
        }
        else
        {
            newHealth = currHealth;
        }
        enemyHealth.SetEnemyHealth(newHealth);
        shieldDamageCount = 0;
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
        }
    }

    void ResetEnemyHealth()
    {
        enemyHealth.SetMaxHealth(100);
        enemyHealth.SetEnemyHealth(enemyHealth.getMaxHealth());
    }

    public void ReceiveDamage(float damagePoints)
    {
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

    public void ToggleEnemyShield()
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

    public void Respawn()
    {
        ResetEnemyHealth();
    }

    // Getters and Setters
    public bool GetHasShield()
    {
        return hasShield;
    }

    public bool GetHasDied()
    {
        return hasDied;
    }

    public void SetHasDied(bool status)
    {
        hasDied = status;
    }

    public int GetAmmoCount()
    {
        return ammoCount;
    }

    public int GetGrenadeCount()
    {
        return grenadeCount;
    }

    public void ShotFired()
    {
        ammoCount--;
    }

    public void GrenadeThrown()
    {
        grenadeCount--;
    }

    // To be used to update with data received from json
    public void SetOpponentHealth(float _health)
    {
        enemyHealth.SetEnemyHealth(_health);
    }
}
