using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HealPotion")]
public class Heal : HealEffect
{
    [SerializeField] float heal_amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<Health>().Heal(heal_amount);
        Debug.Log("Healed");
    }
}
