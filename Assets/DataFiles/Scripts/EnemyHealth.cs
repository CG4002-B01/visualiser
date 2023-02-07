using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    float health, maxHealth;
    void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float getHealth() 
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }
    public void SetEnemyHealth(float _health)
    {
        health = _health;
        slider.value = _health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
        fill.color = gradient.Evaluate(1f);
        // Conditional check to set current health to max health depending on shield
    }
}
