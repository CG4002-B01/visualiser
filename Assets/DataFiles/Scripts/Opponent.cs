using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public EnemyShieldHealth enemyShieldHealth;
    public GameObject enemyShield;
    public GameObject enemyShieldHealthBar;
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
        enemyHealth.SetEnemyHealth(100);
        enemyHealth.SetMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        ShieldTimerCheck();
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

    public void ActivateShield()
    {
        hasShield = true;
        enemyShield.SetActive(true);
        enemyShieldHealthBar.SetActive(true);

        onCooldown = true;
        shieldTimerObj.SetStartTimer(true);
    }

    public void DeactivateShield()
    {
        hasShield = false;
        enemyShield.SetActive(false);
        enemyShieldHealthBar.SetActive(false);
    }

    // To be used to update with data received from json
    public void SetOpponentHealth(float _health)
    {
        enemyHealth.SetEnemyHealth(_health);
    }

    public void SetOppponentShieldHealth(float _shieldHealth)
    {
        enemyShieldHealth.SetEnemyShieldHealth(_shieldHealth);
    }
}
