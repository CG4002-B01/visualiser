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
    int shieldTimer = 10;
    int shieldCount;
    bool hasShield;
    int deathCount;
    int killCount;
    public PlayerHealth playerHealth;
    public HUDText hudTexts;
    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        SetMaxHealth();
        playerHealth.SetHealth(playerHealth.getMaxHealth());

        ammoCount = AmmoCapacity;
        grenadeCount = GrenadeCapacity;
        shieldCount = ShieldCapacity;
        shieldTimer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        SetMaxHealth();
        UpdateHUDTexts();
    }

    void SetMaxHealth()
    {
        playerHealth.SetMaxHealth(hasShield ? 130 : 100);
    }

    void UpdateHUDTexts()
    {
        hudTexts.SetAmmoText(ammoCount + "/" + AmmoCapacity);
        hudTexts.SetGrenadeText(grenadeCount + "/" + GrenadeCapacity);
        hudTexts.SetShieldText(shieldCount + "/" + ShieldCapacity);
    }

    public void Damage(float damagePoints)
    {
        if (playerHealth.getHealth() > 0)
        {
            float tempHealth = playerHealth.getHealth() - damagePoints;
            if (tempHealth < 0) {
                tempHealth = 0;
            } 
            playerHealth.SetHealth(tempHealth);
        }
    }

    // Testing functions
    public void ToggleShield()
    {
        float currHealth = playerHealth.getHealth();
        float newHealth;
        if (hasShield) {
            // -30 Assuming no damage
            newHealth = currHealth - 30;
            shieldCount--;
        } else {
            newHealth = currHealth + 30;
        }
        // float newHealth = hasShield ? currHealth - 30 : currHealth + 30;
        playerHealth.SetHealth(newHealth);
        hasShield = !hasShield;
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
        if (ammoCount == 0)
        {
            ammoCount = AmmoCapacity;
        }
    }

    public void ReloadGrenade()
    {
        if (grenadeCount == 0)
        {
            grenadeCount = GrenadeCapacity;
        }
    }

    public void ResetShieldCount()
    {
        shieldCount = ShieldCapacity;
    }
}
