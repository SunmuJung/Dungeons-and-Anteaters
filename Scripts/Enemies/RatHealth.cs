using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RatHealth : Health
{
    // [SerializeField] HealthBar healthBar;
    private Animator anim;

    private void Start()
    {
        // healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    // Increases the player health by given value.
    // Updates the health bar.
    public override void Heal(float heal)
    {
        base.Heal(heal);
        // healthBar.SetHealth(health);
    }

    // Decreases the player health by given value.
    // Updates the health bar.
    public override void Damage(float damage)
    {
        base.Damage(damage);
        // healthBar.SetHealth(health);
    }
    public override void OnDead()
    {
        // Destroy(gameObject);
        anim.SetTrigger("Dead");
        StartCoroutine("Dead");
    }

    IEnumerator Dead()
    {
        float deadAnimTime = 0f;
        foreach(AnimationClip animClip in anim.runtimeAnimatorController.animationClips)
        {
            if (animClip.name == "RatDead")
            {
                deadAnimTime = animClip.length;
                break;
            }
        }
        yield return new WaitForSecondsRealtime(deadAnimTime - 0.125f);

        gameObject.SetActive(false);
    }
}
