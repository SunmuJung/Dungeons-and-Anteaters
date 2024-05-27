using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is a temporary script for a test purpose.
// temp_Enemy_Health prints current health every 1/2 seconds.

public class temp_Enemy_Health : Health
{
    [SerializeField] bool debug;

    private void Start() 
    {
        debug = true;
        maxHealth = 100;
        health = 100;
    }

    private void Update()
    {
        StartCoroutine(PrintHealth());
    }

    IEnumerator PrintHealth()
    {
        if (debug)
        {
            Debug.Log("Enemy current health: " + health);
            yield return new WaitForSeconds(0.5f);
        }
            
    }
}
