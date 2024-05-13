using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;


// This class manages the health bar for the player.
// Health Bar doesn't change the player's current health.
// Instead, it only gets information of health from the player and visualizes.
public class HealthBar : MonoBehaviour
{
    // the slider representing the current health
    private Slider healthSlider;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Sets the max health of the player to the given value,
    // and sets current health to the max value.
    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;

        // changes current health to the new max value.
        // might not be used.
        healthSlider.value = maxHealth;
    }
    
    // sets the health represented by the health bar
    // to the given health value.
    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
}
