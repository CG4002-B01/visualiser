using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    bool hasShield;
    public PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        SetMaxHealth();
        playerHealth.SetHealth(playerHealth.getMaxHealth());
    }

    // Update is called once per frame
    void Update()
    {
        SetMaxHealth();
    }

    void SetMaxHealth()
    {
        playerHealth.SetMaxHealth(hasShield ? 130 : 100);
    }

    public void Damage(float damagePoints)
    {
        if (playerHealth.getHealth() > 0)
        {
            float tempHealth = playerHealth.getHealth() - damagePoints;
            playerHealth.SetHealth(tempHealth);
        }
    }

    // Testing functions
    public void ToggleShield()
    {
        float currHealth = playerHealth.getHealth();
        float newHealth = hasShield ? currHealth - 30 : currHealth + 30;
        // -30 Assuming no damage
        playerHealth.SetHealth(newHealth);
        hasShield = !hasShield;
    }

    public void DealBulletDamage()
    {
        Damage(10);
    }

    public void DealGrenadeDamage()
    {
        Damage(30);
    }

    public void ResetHealth()
    {
        playerHealth.SetMaxHealth(100);
        playerHealth.SetHealth(playerHealth.getMaxHealth());
    }
}
