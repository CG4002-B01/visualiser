using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public Image[] HealthPoints;
    public Image[] ShieldPoints; 
    bool hasShield; 
    float health, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        hasShield = false;
        SetMaxHealth();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        SetMaxHealth();
        HealthText.text = health + "/" + maxHealth;
        if (health > maxHealth) {
            health = maxHealth;
        }

        HealthBarFiller();
    }

    void SetMaxHealth()
    {
        maxHealth = hasShield ? 130 : 100;
    }

    void HealthBarFiller() 
    {
        for (int i = 0; i < HealthPoints.Length; i++) {
            HealthPoints[i].enabled = !DisplayHealthPoints(health, i);
        }
    }

    bool DisplayHealthPoints(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints) 
    {
        if (health > 0) {
            health = health - damagePoints;
        }
    }

    // Testing functions
    public void ToggleShield() {
        health = hasShield ? health : health + 30; 
        hasShield = !hasShield;
    }

    public void DealBulletDamage() {
        Damage(10);
    }

    public void DealGrenadeDamage() {
        Damage(30);
    }

    public void ResetHealth() {
        maxHealth = 100;
        health = maxHealth;
    }
}
