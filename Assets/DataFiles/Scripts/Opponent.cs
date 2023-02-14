using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    const int ShieldCapacity = 3;
    const int AmmoCapacity = 6;
    const int GrenadeCapacity = 2;
    public EnemyHealth enemyHealth;
    public GameObject enemyShield;
    bool hasDied;
    bool hasShield;
    int ammoCount;
    int grenadeCount;
    int shieldCount;

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
        
    }

    void ActivateShield()
    {
        hasShield = true;
        enemyShield.SetActive(true);
    }

    void DeactivateShield()
    {
        hasShield = false;
        enemyShield.SetActive(false);
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
}
