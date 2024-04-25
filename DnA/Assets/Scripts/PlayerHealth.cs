using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] HealthBar healthBar;


    void Start()
    {
        MaxHealth = 100;
        currentHealth = MaxHealth;
    }

    // Increases the player health by given value.
    // Updates the health bar.
    public void IncreaseHealth(int healing)
    {
        currentHealth += healing;
        healthBar.SetHealth(currentHealth);
    }

    // Decreases the player health by given value.
    // Updates the health bar.
    public void DecreaseHealth(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
