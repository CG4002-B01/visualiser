using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShieldHealth : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    float shieldHealth, maxShieldHealth;

    void Start()
    {
        maxShieldHealth = 30;
    }

    public float getShieldHealth() 
    {
        return shieldHealth;
    }

    public float getMaxShieldHealth()
    {
        return maxShieldHealth;
    }
    public void SetEnemyShieldHealth(float _health)
    {
        shieldHealth = _health;
        slider.value = _health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxShieldHealth(int _maxHealth)
    {
        maxShieldHealth = _maxHealth;
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
        fill.color = gradient.Evaluate(1f);
        // Conditional check to set current health to max health depending on shield
    }
}
