using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    [SerializeField] HealEffect healEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        healEffect.Apply(collision.gameObject);
    }
}
