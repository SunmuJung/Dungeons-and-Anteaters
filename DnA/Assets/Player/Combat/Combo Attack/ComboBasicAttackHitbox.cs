using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBasicAttackHitbox : MonoBehaviour
{
    [SerializeField]
    private float basicAttackDamage;

    private void OnEnable()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        Vector2 boxCenter = transform.position;
        Vector2 posOffset = new Vector2(0f, 0f); //Vector2(1.25f, 0);
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(boxCenter + posOffset, new Vector2(3.5f, 3), 0f);

        foreach(Collider2D overlap in overlaps)
        {
            if (overlap.CompareTag("Enemy"))
            {
                Health enemyHealth = overlap.GetComponent<Health>();
                enemyHealth.Damage(basicAttackDamage);
            }
        }
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            RatHealth ratHealth = collision.collider.GetComponent<Health>() as RatHealth;
            ratHealth.Damage(20f);
        }
    }
    */
}
