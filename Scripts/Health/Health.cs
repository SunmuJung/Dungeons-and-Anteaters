//Coder: Brandon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth, health;

    //Heals the health of the object.
    public virtual void Heal(float heal){
        if(health + heal <= maxHealth){
            health += heal;
        }
        else{
            health = maxHealth;
        }
    }

    //Damages the health of the object
    public virtual void Damage(float damage){
        if(health - damage <= 0f){
            OnDead();
        }
        else{
            health -= damage;
        }
        Debug.Log("health;" + health);
    }

    //Does something when health is 0.
    public virtual void OnDead(){
        Destroy(gameObject);
    }
}
