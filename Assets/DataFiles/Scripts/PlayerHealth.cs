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
    float health, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.text = health + "/" + maxHealth;
        if (health > maxHealth) {
            health = maxHealth;
        }
        HealthBarFiller();
    }

    public void SetHealth(float _health) 
    {
        health = _health;
    }

    public void SetMaxHealth(float _maxHealth) 
    {
        maxHealth = _maxHealth;
    }

    public float getHealth() 
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
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

}
