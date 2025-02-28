using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    // Increases the player health by given value.
    // Updates the health bar.
    public override void Heal(float heal){
        base.Heal(heal);
        healthBar.SetHealth(health);
    }

    // Decreases the player health by given value.
    // Updates the health bar.
    public override void Damage(float damage)
    {
        base.Damage(damage);
        healthBar.SetHealth(health);
    }
}
